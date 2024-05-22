using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Texture;

namespace StardomEngine.Resonance
{
    public class UITheme
    {
        public UITheme(string path)
        {
            path = "data/ui/themes/" + path;
            Frame = new Texture2D(path + "\\frame.png");
            Button = new Texture2D(path + "\\button.png");
        }

        public Texture2D Frame { get; set; }
        public Texture2D Button { get; set; }



    }
}
