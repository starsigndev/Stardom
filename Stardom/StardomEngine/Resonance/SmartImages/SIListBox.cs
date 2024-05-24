using StardomEngine.Draw;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.SmartImages
{
    public class SIListBox : SmartImage
    {

        Texture2D White = null;

        public SIListBox()
        {
            White = new Texture2D("data/ui/white.png");
        }

        public override void Draw(Vector2 pos, Vector2 size, Vector4 color)
        {
            //base.Draw(draw, pos, size, color);
            GameUI.This.DrawRect(White,pos,new Vector2(3,size.Y),new Vector4(0.4f,0.4f,0.4f,1.0f));
            GameUI.This.DrawRect(White, new Vector2(pos.X+size.X - 3, pos.Y), new Vector2(3, size.Y), new Vector4(0.4f, 0.4f, 0.4f, 1.0f));
            GameUI.This.DrawRect(White, new Vector2(pos.X +3, pos.Y), new Vector2(size.X - 6, size.Y), new Vector4(0.7f, 0.7f, 0.7f, 1.0f));


        }

    }
}
