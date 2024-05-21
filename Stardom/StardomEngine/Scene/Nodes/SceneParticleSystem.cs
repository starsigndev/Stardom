using StardomEngine.App;
using StardomEngine.Draw;
using StardomEngine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.Buffers;

namespace StardomEngine.Scene.Nodes
{
    public class SceneParticleSystem : SceneNode
    {

        public List<SceneParticle> BaseParticles
        {
            get;
            set;
        }

        public List<SceneParticle> ActiveParticles
        {
            get;
            set;
        }

        private Random Rnd = new Random(Environment.TickCount);

        public SceneParticleSystem()
        {

            BaseParticles = new List<SceneParticle>();
            ActiveParticles = new List<SceneParticle>();

        }

        public void AddBaseParticle(SceneParticle particle)
        {

            BaseParticles.Add(particle);

        }

        public void Spawn(int count)
        {

            for (int i = 0; i < count; i++)
            {
                int idx = Rnd.Next(0, BaseParticles.Count);
                var spawn = BaseParticles[idx].Clone();
                spawn.Owner = this;
                spawn.Position = Position;
                ActiveParticles.Add(spawn);

            }

        }

        List<SceneParticle> GetSprites()
        {

            List<SceneParticle> Sprites = new List<SceneParticle>();

            foreach (var p in ActiveParticles)
            {
                if (p.Type == ParticleType.Sprite)
                {
                    Sprites.Add(p);
                }
            }

            return Sprites;

        }

        public void RenderSystem(SceneCam cam, SmartDraw draw)
        {
            var sprites = GetSprites();

            foreach (var sprite in sprites)
            {
                RenderSprite(cam, draw, sprite);
            }

            int b = 5;

        }

        void RenderSprite(SceneCam camera, SmartDraw draw, SceneParticle sprite)
        {


            float rot = camera.Rotation;
            float scale = camera.Zoom;

            float iw = StarApp.FrameWidth;
            float ih = StarApp.FrameHeight;

            float rx = sprite.Position.X - camera.Position.X;
            float ry = sprite.Position.Y - camera.Position.Y;

            float px = (iw / 2.0f) - rx;
            float py = (ih / 2.0f) - ry;

            var rp = GameMaths.RotateAndScale(new OpenTK.Mathematics.Vector2(px, py), rot, scale);

            rp.X = (iw / 2.0f) - rp.X;
            rp.Y = (ih / 2.0f) - rp.Y;

            var ext = new Vector4();

            if (sprite.RecvShadows)
            {
                ext.X = 1.0f;
            }
            else
            {
                ext.X = 0.0f;
            }

            var call = draw.DrawSprite(sprite.Image, new OpenTK.Mathematics.Vector2(rp.X, rp.Y), Size, rot + sprite.Rotation, scale, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), ext);
            //if (Normals != null)
            {
                //  call.Normals = Normals;

            }


        }
    }
}
