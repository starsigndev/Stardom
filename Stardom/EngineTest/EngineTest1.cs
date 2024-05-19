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
using StardomEngine.Scene;
using StardomEngine.Scene.Nodes;

namespace EngineTest
{
    public class EngineTest1 : StarApp
    {

        public EngineTest1(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native) { }

        Texture2D Tex1;

        Scene2D Scene1;
        SmartDraw Draw;


        float rotation = 0;
        float scale = 0.1f;
        SceneSprite s1, s2;
        public override void InitApp()
        {
            //base.InitApp();
            Tex1 = new Texture2D("Data/test1.png");
            Draw = new SmartDraw();
            Scene1 = new Scene2D();
            s1 = new SceneSprite();
            s2 = new SceneSprite();
            s1.Image = Tex1;
            s2.Image = Tex1;

            Scene1.AddNode(s1);
            Scene1.AddNode(s2);

            s1.Position = new Vector3(StarApp.FrameWidth/2,StarApp.FrameHeight/2, 1.0f);
            s2.Position = new Vector3(100, 100, 1.0f);

        }
        Random rnd = new Random();
        public override void RenderApp()
        {
            //base.RenderApp();

            Scene1.Camera.Rotation = Scene1.Camera.Rotation + 1.0f;
            Scene1.Render();

      //      Draw.Begin();

           // for (int i = 0; i < 1; i++)
           // {



            //    Draw.DrawQuad(Tex1, new Vector2(20,20),new Vector2(256, 256), new Vector4(1, 1, 1, 1));
    //        Draw.DrawSprite(Tex1, new Vector2(500, 400), new Vector2(256, 256), rotation, scale, new Vector4(1, 1, 1, 1));
            // }
  //          rotation += 0.3f;
//            scale += 0.01f;

            //Draw.End();



        }

    }

   

}
