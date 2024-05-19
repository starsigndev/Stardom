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

        private static int _Width = 0;
        private static int _Height = 0;

        public StarApp(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native)
        {
            // InitApp();
            _Width = native.ClientSize.X;
            _Height = native.ClientSize.Y;
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

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            //base.OnUpdatFrame(args);
            //
            UpdateApp();

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //base.OnRenderFrame(args);
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderApp();
        
            SwapBuffers();

        }

    }
}
