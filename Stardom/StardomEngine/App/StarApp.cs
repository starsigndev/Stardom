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

        public StarApp(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native)
        {
            InitApp();
        }

        public virtual void InitApp()
        {

        }

        public virtual void UpdateApp()
        {
        }

        public virtual void RenderApp()
        {

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();

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
            RenderApp();
        }

    }
}
