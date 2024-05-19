using OpenTK.Mathematics;
using StardomEngine.App;
using StardomEngine.Draw;
using StardomEngine.Helper;
using StardomEngine.Scene.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Scene
{
    public class Scene2D
    {

        public SceneNode RootNode
        {
            get;
            set;
        }

        public List<SceneLight> Lights
        {
            get;
            set;
        }

        public SceneCam Camera
        {
            get;
            set;
        }

        private SmartDraw Draw
        {
            get;
            set;
        }

        public Scene2D()
        {

            RootNode = new SceneNode();
            Camera = new SceneCam();
            Draw = new SmartDraw();
            Lights = new List<SceneLight>();

        }

        public void AddNode(SceneNode node)
        {

            RootNode.AddNode(node);

        }

        public void AddLight(SceneLight light)
        {

            Lights.Add(light);

        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

            Draw.Begin();

            //foreach(var light in Lights) { 


            RootNode.Render(Camera,Draw);

            foreach (var light in Lights)
            {

                float lx = light.Position.X - Camera.Position.X;
                float ly = light.Position.Y - Camera.Position.Y;
                
                float px = (StarApp.FrameWidth/2.0f)-lx;
                float py = (StarApp.FrameHeight / 2.0f) - ly;

                Vector2 lp = GameMaths.RotateAndScale(new Vector2(px, py), Camera.Rotation, Camera.Zoom);

                float fx = (StarApp.FrameWidth / 2.0f) - lp.X;
                float fy = (StarApp.FrameHeight / 2.0f) - lp.Y;

                float lr = light.Range * Camera.Zoom;


                Draw.DrawNormal.Bind();
                Draw.DrawNormal.SetVec3("se_LightDiffuse", light.Diffuse);
                Draw.DrawNormal.SetVec2("se_LightPosition", new OpenTK.Mathematics.Vector2(fx,fy));
                Draw.DrawNormal.SetVec2("se_RenderOffset", new OpenTK.Mathematics.Vector2(0, 0));
                Draw.DrawNormal.SetVec2("se_RenderSize", new OpenTK.Mathematics.Vector2(StardomEngine.App.StarApp.FrameWidth, StardomEngine.App.StarApp.FrameHeight));
                Draw.DrawNormal.SetFloat("se_LightRange",lr);
                Draw.End();
                Draw.DrawNormal.Release();

            }

        }

        public void Fill(SceneSprite spr,int tiles_x,int tiles_y)
        {

            for(int y = 0; y < tiles_y; y++)
            {
                for(int x = 0; x < tiles_x; x++)
                {
                    var tile_spr = spr.CloneSprite();
                    float dx = x * spr.Size.X;
                    float dy = y * spr.Size.Y;
                    tile_spr.Position = new OpenTK.Mathematics.Vector3(dx, dy, 1.0f);
                    AddNode(tile_spr);
                }
            }

        }

    }
}
