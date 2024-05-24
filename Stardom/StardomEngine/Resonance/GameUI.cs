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
using StardomEngine.Resonance.Controls;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;

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

        public List<IWindow> Windows
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

        int CurrentKey = -1;
        bool FirstKey = true;
        int NextKey = 0;


        public GameUI()
        {

            Theme = new UITheme("Arc");
            Draw = new SmartDraw();
            RootControl = new IControl();
            Windows = new List<IWindow>();
            This = this;
            Draw.DrawNormal = new Shader.ShaderModule("data/shader/drawUIVS.glsl", "data/shader/drawUIFS.glsl");
            SystemFont = new kFont("data/ui/fonts/f1.pf");
            OverControl = null;
            PressedControl = null;
            ActiveControl = null;
        }

        public void AddWindow(IWindow window)
        {
            Windows.Add(window);
        }

        public void DrawRect(Texture2D image,Vector2 position,Vector2 size,Vector4 color,float blur=0.0f,bool flip=false,Texture2D mask=null,float refract=0.0f,Texture2D refracter=null)
        {
            Vector4 ext = new Vector4(0,0,0,0);

            Draw.Begin();

            if (flip)
            {
              
                if (mask != null)
                {
                 
                    var call = Draw.DrawQuad(image, position + new Vector2(0, size.Y), new Vector2(size.X, -size.Y), color, new Vector4(blur, 1.0f, refract, 0),mask);
                }
                else
                {

                    var call = Draw.DrawQuad(image, position + new Vector2(0, size.Y), new Vector2(size.X, -size.Y), color, new Vector4(blur, 0, refract, 0), mask); ;
                }
            }
            else
            {
                
                
                if (mask != null)
                {
                 
                    Draw.DrawQuad(image, position, size, color, new Vector4(blur, 1.0f, refract, 0));
                }
                else
                {
              
                    Draw.DrawQuad(image, position, size, color, new Vector4(blur, 0, refract, 0));
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
        bool[] used = new bool[512];

        public void Update()
        {


            var list = GetControlList();

            foreach(var control in list)
            {
                control.Update();
            }


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
                        if (OverControl != ActiveControl)
                        {
                            if (ActiveControl != null)
                            {
                                ActiveControl.OnDeactivate();
                                ActiveControl.Active = false;
                            }
                            PressedControl.OnActivate();
                            PressedControl.Active = true;
                            ActiveControl = PressedControl;
                        }

                    }

                }
            }
            else
            {
                if (GameInput.MouseButton[0] == false)
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
                else
                {
                    if (PressedControl != OverControl)
                    {
                        if (ActiveControl != null && ActiveControl!=OverControl)
                        {
                            ActiveControl.OnDeactivate();
                            ActiveControl.Active = false;

                            ActiveControl = null;
                        }
                        if (OverControl != null)
                        {
                            if (PressedControl != ActiveControl)
                            {
                                PressedControl = OverControl;
                                PressedControl.OnActivate();
                                PressedControl.OnMouseDown(0);
                                ActiveControl = PressedControl;
                                ActiveControl.Active = true;
                            }
                        }
                    }
                }
            }

            if (PressedControl != null)
            {
                PressedControl.OnMoved(GameInput.MouseDelta);
                PressedControl.OnDragged?.Invoke((int)GameInput.MouseDelta.X, (int)GameInput.MouseDelta.Y);
            }

            //kb
            int pkey = 0;
            for(int i = 0; i < 512; i++)
            {
                if (GameInput.KeyButton[i])
                {
                    if (ActiveControl != null)
                    {
                        ActiveControl.OnKeyPressed((OpenTK.Windowing.GraphicsLibraryFramework.Keys)i);
                    }
                }
                if (i == (int)Keys.LeftShift)
                {
                    continue;
                }
                if (i == (int)Keys.RightShift)
                {
                    continue;
                }
                if (GameInput.KeyButton[i] && used[i]==false)
                {
                    if (i != CurrentKey)
                    {
                        FirstKey = true;
                    }
                    CurrentKey = i;
                    used[i] = true;
                }
                else if (GameInput.KeyButton[i]==false && used[i])
                {
                    used[i] = false;
                }

            }

            if (GameInput.KeyButton[(int)Keys.LeftShift] || GameInput.KeyButton[(int)Keys.RightShift])
            {
                if (ActiveControl != null)
                {
                    ActiveControl.ShiftOn = true;
                    Console.WriteLine("Shift!");
                }
            }
            else
            {
                if (ActiveControl != null)
                {
                    ActiveControl.ShiftOn = false;
                    //Console.WriteLine("Shift Off");

                }
            }

            if(CurrentKey!=-1 && GameInput.KeyButton[CurrentKey]==false)
            {
                CurrentKey = -1;
                FirstKey = true;
            }

            if (ActiveControl != null)
            {
                if (CurrentKey != -1)
                {
                    if (FirstKey)
                    {
                        ActiveControl.OnKey((Keys)CurrentKey);
                        FirstKey = false;
                        NextKey = Environment.TickCount + 500;
                    }
                    else
                    {
                        int time = Environment.TickCount;
                        if (time > NextKey)
                        {
                            ActiveControl.OnKey((Keys)CurrentKey);
                            NextKey = time + 150;
                        }
                    }
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
            foreach(var window in Windows)
            {
                AddControlToList(list, window);
            }
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
            foreach(var window in Windows)
            {
                window.Render();
            }

          

          

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
