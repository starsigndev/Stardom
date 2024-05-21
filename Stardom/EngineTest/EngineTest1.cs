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
using StardomEngine.Input;
using System.Security.Cryptography;

namespace EngineTest
{
    public class EngineTest1 : StarApp
    {

        public EngineTest1(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native) { }

        Texture2D Tex1;

        Scene2D Scene1;
        SmartDraw Draw;
        SceneLight Light1;


        float rotation = 0;
        float scale = 0.1f;
        SceneSprite s1, s2;
        SceneSprite s4;
        SceneParticleSystem PS1;
        SceneParticle p1, p2;
        public override void InitApp()
        {
            //base.InitApp();
            Tex1 = new Texture2D("Data/tile1.png");
            Draw = new SmartDraw();
            Scene1 = new Scene2D();
            s1 = new SceneSprite();
            s2 = new SceneSprite();
            s1.Image = Tex1;
            s2.Image = new Texture2D("Data/sprite1.png");
            var s3 = new SceneSprite();
            s3.Image = new Texture2D("Data/sprite1.png");
            //   Scene1.AddNode(s1);

            s1.Normals = new Texture2D("Data/normal2.jpg");

            Scene1.Fill(s1, 16, 16);
            //s1.Size = new Vector2(1024, 1024);

            s4 = new SceneSprite();
            s4.Image = new Texture2D("Data/sprite2.png");
            s4.CastShadows = true;
            s4.Position = new Vector3(200, 250, 1.0f);
           


            //Scene1.AddNode(s1);
            Scene1.AddNode(s3);
        //    Scene1.AddNode(s2);
       //     Scene1.AddNode(s4);
            s3.CastShadows = true;
            s2.Position = new Vector3(250, 120, 1);
            s3.Position = new Vector3(500, 400, 1);

            s3.RecvShadows = false;
            s4.RecvShadows = false;
            s2.RecvShadows = false;
            s1.RecvShadows = true;

            s1.Position = new Vector3(StarApp.FrameWidth/2,StarApp.FrameHeight/2, 1.0f);
           // s2.Position = new Vector3(100, 100, 1.0f);
            Light1 = new SceneLight();
            Light1.Position = new Vector3(350.0f, 350.0f,1.0f);
            Scene1.AddLight(Light1);
            s2.CastShadows = true;
            s1.RecvShadows = true;
            s3.CastShadows = true;
            s4.CastShadows = true;
            //s2.RecvShadows = true;

            PS1 = new SceneParticleSystem();

            Scene1.AddNode(PS1);
            p1 = new SceneParticle();
            p1.Image = new Texture2D("data/fire1.png");
            PS1.AddBaseParticle(p1);
            PS1.Spawn(30);


        }
        Random rnd = new Random();


        public override void UpdateApp()
        {
            //base.UpdateApp();
            s4.Rotation = s4.Rotation + 1f;
            if (GameInput.MouseButton[1])
            {
                //Environment.Exit(0);
                //    Scene1.Camera.Position -= new Vector3(GameInput.MouseDelta.X, GameInput.MouseDelta.Y, 0.0f);
                Scene1.Camera.Move(-GameInput.MouseDelta);

            }

            if (GameInput.MouseButton[2])
            {
                Scene1.Camera.Rotate(GameInput.MouseDelta.X);

                //Scene1.Camera.Rotation += GameInput.MouseDelta.X;
            }

            //Scene1.Camera.AddZoom(GameInput.MouseWheel * 0.1f);

            var os = new Vector2(StarApp.FrameWidth / 2 - GameInput.MousePosition.X, StarApp.FrameHeight / 2 - GameInput.MousePosition.Y);

            Scene1.Camera.SmartZoom(GameInput.MouseWheel * 0.1f, -os);

        }
        public override void RenderApp()
        {
            //base.RenderApp();

         //   Scene1.Camera.Rotation = Scene1.Camera.Rotation + 1.0f;
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

       //     GC.Collect();

        }

    }

   

}
