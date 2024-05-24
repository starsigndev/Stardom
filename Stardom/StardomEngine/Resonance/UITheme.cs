﻿using System;
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
            TextBox = new Texture2D(path + "\\textbox.png");
            WindowTitle = new Texture2D(path + "\\windowtitle.png");
            ListBox = new Texture2D(path + "\\listbox.png");
        }


        public Texture2D Frame { get; set; }
        public Texture2D Button { get; set; }

        public Texture2D TextBox { get; set; }
        public Texture2D WindowTitle { get; set; }

        public Texture2D ListBox { get; set; }


    }
}
