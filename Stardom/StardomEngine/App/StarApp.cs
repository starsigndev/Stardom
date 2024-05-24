using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StardomEngine.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace StardomEngine.App
{
    public class StarApp : GameWindow
    {


        public static int FrameWidth
        {
            get
            {
                return _Width;
            }
        }

        public static int FrameHeight
        {
            get
            {
                return _Height;
            }
        }

        //privates
        private static int _Width = 0;
        private static int _Height = 0;

        private int NextFrame=0;
        private int FPS = 0;
        private int Frame = 0;

        public StarApp(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native)
        {
            // InitApp();
            _Width = native.ClientSize.X;
            _Height = native.ClientSize.Y;
            VSync = VSyncMode.Off;
            GameInput.InitInput();
        }

        public virtual void InitApp()
        {

        }

        public virtual void UpdateApp()
        {
        }

        public virtual void RenderApp()
        {

        

        }


        protected override void OnLoad()
        {
            //base.OnLoad();
            InitApp();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //base.OnKeyDown(e);
            GameInput.KeyButton[(int)e.Key] = true;
            if (e.Key == Keys.LeftShift)
            {
             //   Environment.Exit(1);
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            //base.OnKeyUp(e);
            GameInput.KeyButton[(int)e.Key] = false;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            //base.OnUpdatFrame(args);
            //
            var state = MouseState;

            GameInput.MouseDelta = new Vector2(state.X, state.Y) - GameInput.MousePosition;
            GameInput.MousePosition = new Vector2(state.X, state.Y);
            GameInput.MouseButton[0] = state.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left);
            GameInput.MouseButton[1] = state.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right);
            GameInput.MouseButton[2] = state.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle);
            GameInput.MouseWheel = state.ScrollDelta.Y;

            if (GameInput.MouseWheel != 0)
            {
            //    Console.WriteLine("MD:" + GameInput.MouseWheel);
            }

            UpdateApp();

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //base.OnRenderFrame(args);


            int time = Environment.TickCount;

            if (time > NextFrame)
            {
                NextFrame = time + 1000;
                FPS = Frame;
                Frame = 0;

                Console.WriteLine("Fps:" + FPS);
            }
            Frame++;


            GL.ClearColor(0.1f, 0.1f, 0.1f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderApp();
        
            SwapBuffers();

        }

    }
}
