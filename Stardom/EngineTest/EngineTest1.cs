using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;
using StardomEngine.App;
using StardomEngine.Draw;
using StardomEngine.Texture;
using OpenTK.Mathematics;

namespace EngineTest
{
    public class EngineTest1 : StarApp
    {

        public EngineTest1(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native) { }

        Texture2D Tex1;

        SmartDraw Draw;

        public override void InitApp()
        {
            //base.InitApp();
            Tex1 = new Texture2D("Data/test1.png");
            Draw = new SmartDraw();



        }

        public override void RenderApp()
        {
            //base.RenderApp();

            Draw.Begin();

            Draw.DrawQuad(Tex1, new Vector2(20, 20), new Vector2(256, 256), new Vector4(1, 1, 1, 1));

            Draw.End();


        }

    }

   

}
