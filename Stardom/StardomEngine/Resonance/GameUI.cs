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

        public void DrawRect(Texture2D image,Vector2 position,Vector2 size,Vector4 color,float blur=0.0f)
        {
            Vector4 ext = new Vector4(0,0,0,0);

            var call = Draw.DrawQuad(image, position, size, color);

            call.Ext = new Vector4(blur, 0, 0, 0);



          // Draw.DrawSprite(image, position, size,0.0f,1.0f, color,new Vector4(0,0,0,0));
        }
        
        public void DrawText(string text,Vector2 position,Vector4 color,float scale =1.0f)
        {

            SystemFont.Scale = scale;
            SystemFont.DrawString(text,(int)position.X, (int)position.Y, color.X, color.Y, color.Z, color.W, Draw);

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

            Draw.Begin();

            RootControl.Render();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Draw.DrawNormal.Bind();
            Draw.End();
            Draw.DrawNormal.Release();

        }

    }
}
