using StardomEngine.App;
using StardomEngine.Draw;
using StardomEngine.Helper;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Scene.Nodes
{
    public class SceneSprite : SceneNode
    {

        public Texture2D Image
        {
            get;
            set;
        }

        public Texture2D Normals
        {
            get; set;
        }

        public override void Render(SceneCam camera,SmartDraw draw)
        {
            //base.Render();


            float rot = camera.Rotation;
            float scale = camera.Zoom;

            float iw = StarApp.FrameWidth;
            float ih = StarApp.FrameHeight;

            float rx = Position.X - camera.Position.X;
            float ry = Position.Y - camera.Position.Y;

            float px = (iw / 2.0f) - rx;
            float py = (ih / 2.0f) - ry;

            var rp = GameMaths.RotateAndScale(new OpenTK.Mathematics.Vector2(px, py), rot, scale);

            rp.X = (iw / 2.0f) - rp.X;
            rp.Y = (ih / 2.0f) - rp.Y;


           draw.DrawSprite(Image, new OpenTK.Mathematics.Vector2(rp.X,rp.Y),Size, rot,scale, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));


            RenderSubNodes(camera, draw);   
        }

        public SceneSprite CloneSprite()
        {

            SceneSprite clone = new SceneSprite();
            clone.Image = Image;
            clone.Normals = Normals;
            clone.Size = Size;
            clone.Position = Position;

            foreach (var sub in SubNodes)
            {
                if (sub is SceneSprite) {
                    var sub_spr = sub as SceneSprite;
                    clone.AddNode(sub_spr.CloneSprite());
                }
            }

            return clone;

        }

    }
}
