using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Texture
{
    public class TextureSlice
    {
        public Texture2D LeftTopCorner, RightTopCorner, LeftBotCorner, RightBotCorner;
        public Texture2D Left, Right, Top, Botttom, Center;
        public TextureSlice(Texture2D tex,int cornerWidth=16,int cornerHeight=16)
        {
            LeftTopCorner = new Texture2D(tex, 0, 0, cornerWidth, cornerHeight);
            RightTopCorner = new Texture2D(tex, tex.Width - cornerWidth,0,cornerWidth, cornerHeight);
            LeftBotCorner = new Texture2D(tex, 0, tex.Height - cornerHeight, cornerWidth, cornerHeight);
            RightBotCorner = new Texture2D(tex,tex.Width-cornerWidth,tex.Height-cornerHeight,cornerWidth,cornerHeight);
            Left = new Texture2D(tex, 0, cornerHeight, cornerWidth, tex.Height - (cornerHeight * 2));
            Right = new Texture2D(tex, tex.Width - cornerWidth, cornerHeight, cornerWidth, tex.Height - (cornerHeight * 2));
            Top = new Texture2D(tex, cornerWidth, 0, tex.Width - (cornerWidth * 2), cornerHeight);
            Botttom = new Texture2D(tex,cornerWidth,tex.Height- cornerHeight,tex.Width-(cornerWidth*2), cornerHeight);
            Center = new Texture2D(tex, cornerWidth, cornerHeight, tex.Width - (cornerWidth * 2), tex.Height - (cornerHeight * 2));
        }

    }
}
