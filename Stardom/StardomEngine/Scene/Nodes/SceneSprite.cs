using StardomEngine.App;
using StardomEngine.Draw;
using StardomEngine.Helper;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.Serialization;

namespace StardomEngine.Scene.Nodes
{
    public class SceneSprite : SceneNode
    {

        public Texture2D Image
        {
            get
            {
                return _Img;
            }
            set
            {

                _Img = value;
                _Img.Resize((int)Size.X,(int)Size.Y);
            }
        }
        Texture2D _Img;

        public Texture2D Normals
        {
            get; set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public bool CastShadows
        {
            get;
            set;
        }

        public bool RecvShadows
        {
            get;
            set;
        }

     

        public SceneSprite()
        {

            CastShadows = false;
            RecvShadows = true;

        }

        public bool InBounds(int x,int y)
        {
            if(x>Position.X-Size.X/2)
            {
                if(x<Position.X+Size.X/2)
                {
                    if (y > Position.Y - Size.Y / 2)
                    {
                        if(y<Position.Y+Size.Y/2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        public Vector4 GetPixel(int x,int y)
        {

            float mx = Image.Width / 2;
            float my = Image.Height / 2;

            float nx = x - mx;
            float ny = y - my;


            var rp = GameMaths.RotateAndScale(new Vector2(nx, ny), -Rotation, 1.0f);

            nx = mx + rp.X;
            ny = my + rp.Y;

            return Image.GetPixel((int)nx, (int)ny);

            return new Vector4(1, 1, 1, 1);

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

            var ext = new Vector4();

            if (RecvShadows)
            {
                ext.X = 1.0f;
            }
            else
            {
                ext.X = 0.0f;
            }

            var call = draw.DrawSprite(Image, new OpenTK.Mathematics.Vector2(rp.X, rp.Y), Size, rot + Rotation, scale, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), ext);
            if (Normals != null)
            {
                call.Normals = Normals;
            }

            RenderSubNodes(camera, draw);   
        }

        public SceneSprite CloneSprite()
        {

            SceneSprite clone = new SceneSprite();
            clone.Image = Image;
            clone.Normals = Normals;
            clone.Size = Size;
            clone.Position = Position;
            clone.RecvShadows = RecvShadows;

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
