using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IPanel : IControl
    {

        public IPanel()
        {

        }

        public override void Render()
        {
            //base.Render();

            var bg = GameUI.This.GrabBG(RenderPosition, Size);

           // Color = new OpenTK.Mathematics.Vector4(Color.X, Color.Y, Color.Z, 1.0f);
            GameUI.This.DrawRect(bg, RenderPosition, Size,new OpenTK.Mathematics.Vector4(1,1,1,1), 0.007f, true, GameUI.Theme.Frame);
           // Color = new OpenTK.Mathematics.Vector4(Color.X, Color.Y, Color.Z,0.2f);
           GameUI.This.DrawRect(GameUI.Theme.Frame, RenderPosition, Size,Color, 0, false);



            RenderChildren();

        }

    }
}
