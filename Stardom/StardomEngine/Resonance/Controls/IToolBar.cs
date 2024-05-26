using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IToolBar : IControl
    {

        private int NextX = 5;

        public IToolBar()
        {
            Size = new OpenTK.Mathematics.Vector2(App.StarApp.FrameWidth, 40);
        }
        public override void Render()
        {
            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.MenuBar, RenderPosition, Size, new OpenTK.Mathematics.Vector4(0.7f, 0.7f, 0.7f, 1.0f));

            RenderChildren();

        }

        public void AddControl(IControl control)
        {
            base.AddControl(control);
            control.SetPosition(new OpenTK.Mathematics.Vector2(NextX, 7));
            NextX =NextX + (int)control.Size.X + 5;

        }

    }
}
