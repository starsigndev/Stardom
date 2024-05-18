using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Texture;

namespace StardomEngine.Draw
{
    public class SmartDraw
    {
        public List<DrawList> Lists { get; set; }
        public float CurrentZ = 0.0f;

        public SmartDraw()
        {

            Lists = new List<DrawList>();

        }
        public void Begin()
        {

            CurrentZ = 0.1f;
            Lists.Clear();

        }

        public void DrawQuad(Texture2D image,Vector2 position,Vector2 size,Vector4 color)
        {

        }

        public void End()
        {

        }

    }
}
