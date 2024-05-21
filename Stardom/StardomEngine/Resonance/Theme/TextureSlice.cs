using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Theme
{
    public class TextureSlice
    {

        public Texture2D Left, Right, Top, Bottom, Edge, Central;

        public TextureSlice(string path)
        {

            Left = new Texture2D(path + "_left.png");
            Right = new Texture2D(path + "_right.png");
            Top = new Texture2D(path + "_top.png");
            Bottom = new Texture2D(path + "_bottom.png");
            Edge = new Texture2D(path + "_edge.png");
            Central = new Texture2D(path + "_central");

        }

    }
}
