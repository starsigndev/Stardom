using OpenTK.Mathematics;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Draw
{
    public class DrawCall
    {

        public float[] X { get; set; }
        public float[] Y { get; set; }
        public float Z { get; set; }
        public Vector4 Color { get; set; }

        public Vector4 Ext { get; set; }

        public Texture2D Image { get; set; }

        public Texture2D Normals { get; set; }

        private static Texture2D BlankNormal = null;


        public DrawCall()
        {

            if (BlankNormal == null)
            {
                BlankNormal = new Texture2D("data/blankNormals.png");
            }
            X = new float[4];
            Y = new float[4];
            Z = 0.0f;
            Color = new Vector4(1, 1, 1, 1);
            Normals = BlankNormal;

        }


    }
}
