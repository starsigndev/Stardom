using StardomEngine.RenderTarget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public delegate void RenderContents(IControl owner);

    public class IRenderTarget : IControl
    {
        public RenderContents OnRenderContents
        {
            get;
            set;
        }
        public RenderTarget2D RT
        {
            get;
            set;
        }

       

        public IRenderTarget(int width,int height)
        {
            RT = new RenderTarget2D(width, height);
        }

        public override void PreRender()
        {
            //base.PreRender();
            RT.Bind();
            OnRenderContents?.Invoke(this);
            RT.Release();

        }

        public override void Render()
        {


            GameUI.This.DrawRect(RT.BB, RenderPosition, Size, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), 0, true);
            //base.Render();
            int b = 5;
        }

    }
}
