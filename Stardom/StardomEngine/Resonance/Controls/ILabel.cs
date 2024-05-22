using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class ILabel : IControl
    {

        public float Scale = 1.0f;

        public ILabel(string text)
        {

            Text = text;

        }

        public override void Render()
        {
            //base.Render();
            
            GameUI.This.DrawText(Text, RenderPosition, Color,Scale);
        }

    }
}
