using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace StardomEngine.RenderTarget
{
    public class RenderTarget2D 
    {

        public int FrameBufferHandle
        {
            get;
            set;
        }

        public Texture2D BB
        {
            get;
            set;
        }

        private int RenderBufferHandle
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public RenderTarget2D(int width,int height)
        {
            
            Width = width;
            Height = height;

            FrameBufferHandle = GL.CreateFramebuffer();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferHandle);
            BB = new Texture2D(width, height,4,false);
            //DB = new TextureDepth(w, h);

            RenderBufferHandle = GL.CreateRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderBufferHandle);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RenderBufferHandle);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, BB.Handle, 0);
            DrawBufferMode db = DrawBufferMode.ColorAttachment0;
            GL.DrawBuffers(1, db);
            var fs = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fs != FramebufferStatus.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
                throw (new Exception("Framebuffer failed:" + fs.ToString()));
            }
            Console.WriteLine("Framebuffer success.");
            //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            //GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer,0);

        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBufferHandle);
            // SetVP.Set(0, 0, IW, IH);
            App.StarApp.BoundRT2D = this;


            GL.Viewport(0, 0, Width, Height);
            //  GL.ClearColor(0, 0, 0, 1);
            //GL.ClearDepthf(0.0f);
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //    GL.ClearColor(1, 0, 0, 1);
        }


        public void Release()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer,0);
            // SetVP.Set(0, 0, AppInfo.W, AppInfo.H);
            App.StarApp.BoundRT2D = null;
            GL.Viewport(0, 0, App.StarApp.FrameWidth, App.StarApp.FrameHeight);
        }

    }
}
