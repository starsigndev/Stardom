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
using StardomEngine.Resonance;
using StardomEngine.Resonance.Controls;
using OpenTK.Mathematics;
using StardomEngine.RenderTarget;

namespace EngineTest
{
    public enum TestEnum
    {
        DirectX,OpenGL,Vulkan,DirectX12,Metal,Glide,Other,Try,No,Why,Way,How
    }
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
        GameUI UI;
        RenderTarget2D RT1;
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
            s3.RecvShadows = false;
            s3.Image = new Texture2D("Data/sprite1.png");
            //   Scene1.AddNode(s1);
            RT1 = new RenderTarget2D(StarApp.FrameWidth, StarApp.FrameHeight);
            s1.Normals = new Texture2D("Data/normal2.jpg");
            s1.RecvShadows = true;
            Scene1.Fill(s1, 16, 16);
            s1.Size = new Vector2(1024, 1024);

            s4 = new SceneSprite();
            s4.Image = new Texture2D("Data/sprite2.png");
            s4.CastShadows = true;
            s4.Position = new Vector3(200, 250, 1.0f);
           


         //   Scene1.AddNode(s1);
            Scene1.AddNode(s3);
        //    Scene1.AddNode(s2);
       //     Scene1.AddNode(s4);
            s3.CastShadows = true;
            s2.Position = new Vector3(250, 120, 1);
            s3.Position = new Vector3(500, 400, 1);

            s3.RecvShadows = false;
            s4.RecvShadows = false;
            s2.RecvShadows = false;
          //  s1.RecvShadows = true;

            s1.Position = new Vector3(StarApp.FrameWidth/2,StarApp.FrameHeight/2, 1.0f);
           // s2.Position = new Vector3(100, 100, 1.0f);
            Light1 = new SceneLight();
            Light1.Position = new Vector3(350.0f, 350.0f,1.0f);
            Scene1.AddLight(Light1);
            s2.CastShadows = true;
            s1.RecvShadows = false;
            s3.CastShadows = true;
            s4.CastShadows = true;
            //s2.RecvShadows = true;

            PS1 = new SceneParticleSystem();

            Scene1.AddNode(PS1);
            p1 = new SceneParticle();
            p1.Image = new Texture2D("data/fire1.png");
            PS1.AddBaseParticle(p1);
            PS1.Spawn(5000);

            UI = new GameUI();

            var img = new IImage(new Texture2D("data/bg1.png")).Set(new Vector2(0, 0), new Vector2(StarApp.FrameWidth, StarApp.FrameHeight), "") as IImage;

            UI.RootControl.AddControl(img);

            b1 = new IButton().Set(new Vector2(140, 140), new Vector2(120, 30), "Test") as IButton;
           
      
            UI.RootControl.AddControl(b1);
            b1.OnClick = (but, mbut) =>
            {
                Console.WriteLine("Clicked!");
            };
            var l1 = new ILabel("This is a test label.").Set(new Vector2(20, 20), Vector2.Zero, "This is a test label.") as ILabel;
            UI.RootControl.AddControl(l1);
            l1.Scale = 3.0f;

            var image1 = new IPanel().Set(new Vector2(50, 30), new Vector2(400, 280), "") as IPanel;
            //UI.RootControl.AddControl(image1);


          //  var b2 = new IButton().Set(new Vector2(30, 20), new Vector2(180, 30), "Load Game") as IButton;
           // image1.AddControl(b2);
            image1.Color = new Vector4(1, 1, 1, 0.4f);
            image1.Refracter = new Texture2D("data/refract2.jpg");
            pp1 = image1;

            var win1 = new IWindow().Set(new Vector2(202, 20), new Vector2(400, 300), "Test Window") as IWindow;
            var b2 = new IButton().Set(new Vector2(-40, 455), new Vector2(180, 30), "Load Game") as IButton;
            //win1.Contents.AddControl(b2);
            //--- STENCIL eden stealing resorces cloaked
            //UI.RootControl.AddControl(win1);
            UI.AddWindow(win1);
            var frt = new IRenderTarget(800, 600);
            //win1.Contents.AddControl(frt);

            var vid1 = new IVideo();
            vid1.PlayVideo("data/intronew.mp4");
            vid1.Set(new Vector2(0, 0), win1.Contents.Size, "");

            win1.Contents.AddControl(vid1);

            frt.Set(new Vector2(0,0), new Vector2(win1.Contents.Size.X, win1.Contents.Size.Y));

            frt.OnRenderContents = (control) =>
            {
                Scene1.Render();
            };

            win1.Contents.Color = new Vector4(1, 1, 1, 0.7f);
            win1.Contents.Refracter = new Texture2D("data/refract1.png");

            var tb1 = new ITextBox();
            UI.RootControl.AddControl(tb1);

            tb1.Set(new Vector2(20, 300), new Vector2(180, 30));
            //tb1.Text = "This is";

            var es1 = new IEnumSelector(typeof(TestEnum)).Set(new Vector2(20, 360), new Vector2(130, 30), "") as IEnumSelector;
            UI.RootControl.AddControl(es1);
            es1.OnSelected = (name,et) =>
            {
                if(et == TestEnum.Glide)
                {
                    Environment.Exit(1);
                }
                Console.WriteLine("Selected:" + name);
            };


            var file = UI.MainMenu.AddItem("File");
            var edit = UI.MainMenu.AddItem("Edit");
            var options = UI.MainMenu.AddItem("Options");

            var load = file.AddItem("Load Game");
            var save = file.AddItem("Save Game");
            file.AddSeperator();
            var ex = file.AddItem("Exit Game");

            save.OnItemSelected = (item) =>
            {
                Console.WriteLine("Loaded!");
            };

            ex.OnItemSelected = (item) =>
            {
                Environment.Exit(1);
            };

            load.Icon = new Texture2D("data/testicon.png");

            edit.AddItem("Cut");
            edit.AddItem("Copy");
            edit.AddItem("Paste");

            load.AddItem("New Game");
            load.AddItem("Old Game");
            load.AddItem("Exit Game");

            Draw.DrawNormal = new StardomEngine.Shader.ShaderModule("data/shader/drawUIVS.glsl", "data/shader/drawUIFS.glsl");

            var num1 = new INumber(true).Set(new Vector2(40, 500), new Vector2(0, 0), "") as INumber;
            UI.RootControl.AddControl(num1);
            num1.OnNumberChanged = (con, v) =>
            {
                Console.WriteLine("Num:" + v.ToString());
            };

            IToggle tog1 = new IToggle().Set(new Vector2(30, 600), new Vector2(16, 16), "") as IToggle;
            UI.RootControl.AddControl(tog1);
            tog1.Text = "Raytracing";

        }
        IButton b1;
        IPanel pp1;

        Random rnd = new Random();


        public override void UpdateApp()
        {
            //return;

            //base.UpdateApp();
           // Scene1.Update();
            
            UI.Update();
            s4.Rotation = s4.Rotation + 1f;
            if (GameInput.MouseButton[1])
            {
                //Environment.Exit(0);
                //    Scene1.Camera.Position -= new Vector3(GameInput.MouseDelta.X, GameInput.MouseDelta.Y, 0.0f);
                //  Scene1.Camera.Move(-GameInput.MouseDelta);

                pp1.Position += GameInput.MouseDelta;

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
            //base.RenderApp(); PS1.Spawn(30);
            // return;
          //  RT1.Bind();
            UI.Render();

         //   RT1.Release();
         //   Draw.Begin();
         //   Draw.DrawQuad(RT1.BB, new Vector2(0, StarApp.FrameHeight), new Vector2(StarApp.FrameWidth,-StarApp.FrameHeight), new Vector4(1, 1, 1, 1), new Vector4(0, 1, 0, 0));
         //   Draw.EndComplete();

            //   Scene1.Camera.Rotation = Scene1.Camera.Rotation + 1.0f;


            //Scene1.Render();
          //  UI.Render();
            //      Draw.Begin();

            // for (int i = 0; i < 1; i++)
            // {



            //    Draw.DrawQuad(Tex1, new Vector2(20,20),new Vector2(256, 256), new Vector4(1, 1, 1, 1));
            //        Draw.DrawSprite(Tex1, new Vector2(500, 400), new Vector2(256, 256), rotation, scale, new Vector4(1, 1, 1, 1));
            // }
            //          rotation += 0.3f;
            //            scale += 0.01f;

            //Draw.End();

         //   GC.Collect();

        }

    }

   

}
