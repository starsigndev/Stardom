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
using OpenTK.Graphics.OpenGL;
using StardomEngine.Scene.Jobs;

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

        private JobGenerateShadowMap GenerateShadows
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
           GenerateShadows = new JobGenerateShadowMap();
            GenerateShadows.Scene = this;
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
            UpdateParticleSystems();
        }

        

        public List<SceneSprite> GetShadowCasters()
        {

            List<SceneSprite> list = new List<SceneSprite>();

            AddCasters(list, RootNode);

            return list;

        }

        public void AddCasters(List<SceneSprite> list,SceneNode spr)
        {


            if (spr is SceneSprite)
            {
                var sprn = spr as SceneSprite;
                if (sprn.CastShadows)
                {
                    list.Add(sprn);
                }
            }

            foreach(var sub in spr.SubNodes)
            {

                AddCasters(list, sub);

            }

        }

        public void RenderShadows(SceneLight light)
        {

            
            GenerateShadows.Bind();
            GenerateShadows.Execute();
            GenerateShadows.Release();


        }

        public void RenderShadows2(SceneLight light)
        {

            var casters = GetShadowCasters();

            for (int i = 0; i < 360; i++)
            {


                //Vector4 col = new Vector4(0.5f, 0.5f, 0.5f,0.0f);

                float close = 1.0f;

                float sx, sy;

                sx = light.Position.X;
                sy = light.Position.Y;

                float dx, dy;

                float xi = (float)Math.Cos(MathHelper.DegreesToRadians(i+180));
                float yi = (float)Math.Sin(MathHelper.DegreesToRadians(i+180));

                dx = sx + xi * light.Range;
                dy = sy + yi * light.Range;

                float lxd = dx - sx;
                float lyd = dy - sy;

                float steps = 0.0f;

                if (MathHelper.Abs(lxd) > MathHelper.Abs(lyd))
                {
                    steps = MathHelper.Abs(lxd);
                }
                else
                {
                    steps = MathHelper.Abs(lyd);
                }

                float mxi = lxd / steps;
                float myi = lyd / steps;

                float cx, cy;

                cx = sx;
                cy = sy;


                bool ls = false;
                for(int li = 0; li < steps; li++)
                {
                    
                    foreach(var sc in casters)
                    {
                        if ((cx >= sc.Position.X - sc.Size.X / 2) && (cx<=sc.Position.X+sc.Size.X/2))
                        {
                            if((cy>=sc.Position.Y-sc.Size.Y/2) && (cy<=sc.Position.Y+sc.Size.Y/2))
                            {

                                var rx = cx - (sc.Position.X - sc.Size.X/2.0f);
                                var ry = cy - (sc.Position.Y - sc.Size.Y/2.0f);

                                float lx = rx / sc.Size.X;
                                float ly = ry / sc.Size.Y;

                                rx = sc.Image.Width * lx;
                                ry = sc.Image.Height * ly;

                                //   var pix = sc.Image.GetPixel((int)rx, (int)ry);
                                var pix = sc.GetPixel((int)rx, (int)ry);

                                if (pix.W > 0.1)
                                {
                                    //       ls = true;
                                    float cdx = cx - sx;
                                    float cdy = cy - sy;
                                    float ad = MathF.Sqrt(cdx * cdx + cdy * cdy);
                                    ad = ad / light.Range;
                                    if (ad < close)
                                    {
                                        close = ad;
                                    }
                                }
                           
                            }
                        }
                    }

                    cx += mxi;
                    cy += myi;
                }


                light.ShadowMap.SetPixelFloat(i, 0,new Vector4(close,close,close,1.0f));


            }

            light.ShadowMap.Upload();



        }

        public virtual void Render()
        {

            GL.Disable(EnableCap.ScissorTest);

            Draw.Begin();

            //foreach(var light in Lights) { 


            RootNode.Render(Camera, Draw);

            foreach (var light in Lights)
            {
                RenderShadows(light);
            }


            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Disable(EnableCap.DepthTest);

            FinalDraw();

            Draw.Begin();

            RenderParticleSystems(Camera, Draw);


            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);

            FinalDraw();

//            Draw.End();

        }

        private void FinalDraw()
        {
            foreach (var light in Lights)
            {


             

                float lx = light.Position.X - Camera.Position.X;
                float ly = light.Position.Y - Camera.Position.Y;

                float px = (StarApp.FrameWidth / 2.0f) - lx;
                float py = (StarApp.FrameHeight / 2.0f) - ly;

                Vector2 lp = GameMaths.RotateAndScale(new Vector2(px, py), Camera.Rotation, Camera.Zoom);

                float fx = (StarApp.FrameWidth / 2.0f) - lp.X;
                float fy = (StarApp.FrameHeight / 2.0f) - lp.Y;

                float lr = light.Range * Camera.Zoom;



                Draw.DrawNormal.Bind();
                light.ShadowMap.Bind(2);
                Draw.DrawNormal.SetInt("se_ShadowMap", 2);
                Draw.DrawNormal.SetVec3("se_LightDiffuse", light.Diffuse);
                Draw.DrawNormal.SetVec2("se_LightPosition", new OpenTK.Mathematics.Vector2(fx, fy));
                Draw.DrawNormal.SetVec2("se_RenderOffset", new OpenTK.Mathematics.Vector2(0, 0));
                Draw.DrawNormal.SetVec2("se_RenderSize", new OpenTK.Mathematics.Vector2(StardomEngine.App.StarApp.FrameWidth, StardomEngine.App.StarApp.FrameHeight));
                Draw.DrawNormal.SetFloat("se_LightRange", lr);
                Draw.DrawNormal.SetFloat("se_Rotate", -Camera.Rotation);
                Draw.DrawNormal.SetFloat("se_Zoom", Camera.Zoom);
                Draw.End();
                Draw.DrawNormal.Release();
                light.ShadowMap.Release(2);

            }
        }

        public void UpdateParticleSystems()
        {

            UpdateParticleSystemNode(RootNode);

        }

        void UpdateParticleSystemNode(SceneNode node)
        {

            if(node is SceneParticleSystem)
            {

                node.Update();

            }

            foreach(var sub in node.SubNodes)
            {
                UpdateParticleSystemNode(sub);
            }

        }

        public void RenderParticleSystems(SceneCam camera,SmartDraw draw)
        {


            RenderParticleSystemNode(RootNode,camera,draw);

        }

        public void RenderParticleSystemNode(SceneNode node,SceneCam camera,SmartDraw draw)
        {

            if(node is SceneParticleSystem)
            {
                var ps = (SceneParticleSystem)node;
                ps.RenderSystem(camera,draw);
            }

            foreach(var sub in node.SubNodes)
            {
                RenderParticleSystemNode(sub,camera,draw);
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
