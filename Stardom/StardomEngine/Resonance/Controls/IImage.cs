using OpenTK.Windowing.Common.Input;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IImage : IControl
    {

        public IImage(Texture2D image)
        {

            Image = image;

        }

        public override void Render()
        {
            //base.Render();

            GameUI.This.DrawRect(Image, RenderPosition, Size, Color);

        }

    }
}
