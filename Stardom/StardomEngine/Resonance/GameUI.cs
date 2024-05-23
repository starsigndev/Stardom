using StardomEngine.Draw;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Vivid.Font;
using StardomEngine.Input;

namespace StardomEngine.Resonance
{
    public class GameUI
    {

        public static SmartDraw Draw
        {
            get;
            set;
        }

        public static UITheme Theme
        {
            get;
            set;
        }

        public static GameUI This
        {
            get;
            set;
        }

        public IControl RootControl
        {
            get;
            set;
        }

        public static kFont SystemFont
        {
            get;
            set;
        }

        public IControl OverControl
        {
            get;
            set;
        }

        public IControl PressedControl
        {
            get;
            set;
        }

        public IControl ActiveControl
        {
            get;
            set;
        }

        public GameUI()
        {

            Theme = new UITheme("Arc");
            Draw = new SmartDraw();
            RootControl = new IControl();
            This = this;
            Draw.DrawNormal = new Shader.ShaderModule("data/shader/drawUIVS.glsl", "data/shader/drawUIFS.glsl");
            SystemFont = new kFont("data/ui/fonts/f1.pf");
            OverControl = null;
            PressedControl = null;
            ActiveControl = null;
        }

        public void DrawRect(Texture2D image,Vector2 position,Vector2 size,Vector4 color,float blur=0.0f,bool flip=false,Texture2D mask=null,float refract=0.0f,Texture2D refracter=null)
        {
            Vector4 ext = new Vector4(0,0,0,0);

            Draw.Begin();

            if (flip)
            {
                var call = Draw.DrawQuad(image, position + new Vector2(0, size.Y), new Vector2(size.X, -size.Y), color);
                call.Normals = mask;
                if (mask != null)
                {
                    call.Ext = new Vector4(blur, 1.0f, refract, 0);
                }
                else
                {
                    call.Ext = new Vector4(blur, 0, refract, 0);
                }
            }
            else
            {
                var call = Draw.DrawQuad(image, position, size, color);
                call.Normals = mask;
                if (mask != null)
                {
                    call.Ext = new Vector4(blur, 1.0f, refract, 0);
                }
                else
                {
                    call.Ext = new Vector4(blur, 0, refract, 0);
                }
            }
            Draw.DrawNormal.Bind();
            if (mask != null)
            {
                mask.Bind(1);
                Draw.DrawNormal.SetInt("se_MaskTexture", 1);
            }
            if (refracter!=null)
            {
                refracter.Bind(2);
                Draw.DrawNormal.SetInt("se_RefractTexture", 2);
            }
            Draw.End();
            Draw.DrawNormal.Release();


            // Draw.DrawSprite(image, position, size,0.0f,1.0f, color,new Vector4(0,0,0,0));
        }
        
        public void DrawText(string text,Vector2 position,Vector4 color,float scale =1.0f)
        {

            Draw.Begin();

            SystemFont.Scale = scale;
            SystemFont.DrawString(text,(int)position.X, (int)position.Y, color.X, color.Y, color.Z, color.W, Draw);

            Draw.DrawNormal.Bind();
            Draw.End();
            Draw.DrawNormal.Release();

        }

        public float TextWidth(string text,float scale=1.0f)
        {
            SystemFont.Scale = scale;
            return SystemFont.StringWidth(text);

        }

        public float TextHeight(string text, float scale =1.0f)
        {

            SystemFont.Scale = scale;
            return SystemFont.StringHeight();

        }

        public void Update()
        {

            Vector2 mouse_pos = GameInput.MousePosition;

            var over = GetOver(mouse_pos);

            if (PressedControl == null)
            {
                if (over != null)
                {

                    if (over != OverControl)
                    {
                        over.OnEnter();
                        if (OverControl != null)
                        {
                            OverControl.OnLeave();
                        }
                        OverControl = over;
                    }
                }
                else
                {
                    if (OverControl != null)
                    {
                        OverControl.OnLeave();
                        OverControl = null;
                    }
                }
            }

            if (PressedControl == null)
            {
                if (OverControl != null)
                {

                    if (GameInput.MouseButton[0])
                    {
                        PressedControl = OverControl;
                        PressedControl.OnMouseDown(0);

                    }

                }
            }
            else
            {
                if (GameInput.MouseButton[0]==false)
                {
                    PressedControl.OnMouseUp(0);
                    if (OverControl != PressedControl)
                    {
                        PressedControl.OnLeave();
                    }
                    else
                    {
                        PressedControl.OnEnter();
                    }
                    PressedControl = null;
                }
            }

            if (PressedControl != null)
            {
                PressedControl.OnMoved(GameInput.MouseDelta);
                PressedControl.OnDragged?.Invoke((int)GameInput.MouseDelta.X, (int)GameInput.MouseDelta.Y);
            }

        }

        public IControl GetOver(Vector2 pos)
        {

            var list = GetControlList();
            list.Reverse();

            foreach(var control in list)
            {

                if (control.InBounds(pos))
                {
                    return control;
                }

            }
            return null;

        }

        public List<IControl> GetControlList()
        {

            List<IControl> list = new List<IControl>(512);
            AddControlToList(list,RootControl);
            return list;


        }

        public void AddControlToList(List<IControl> list,IControl control)
        {
            list.Add(control);
            foreach (var child in control.Controls)
            {
                AddControlToList(list, child);
            }
        }

        public void Render()
        {

//            Draw.Begin();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.DepthTest);

            RootControl.Render();

          

          

        }

        public Texture2D PreviousBG = null;

        public Texture2D GrabBG(Vector2 position,Vector2 size)
        {
            if (PreviousBG != null)
            {
                PreviousBG.Delete();
            }
            var tex = new Texture2D((int)size.X, (int)size.Y, 3,false);

            tex.Grab(new Vector2(position.X, position.Y));
            PreviousBG = tex;
            //.Delete();


            return tex;

        }

    }
}
