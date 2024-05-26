using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IToggle : IControl
    {

        public bool Toggle
        {
            get;
            set;
        }

        public IToggle()
        {
            Size = new OpenTK.Mathematics.Vector2(16, 16);
        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            Toggle = Toggle ? false : true;
        }

        public override void Render()
        {
            //base.Render();

            GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition, new OpenTK.Mathematics.Vector2(16, 16), Color);
            GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition+new OpenTK.Mathematics.Vector2(2,2), new OpenTK.Mathematics.Vector2(12, 12), new OpenTK.Mathematics.Vector4(0.4f,0.4f,0.4f,1.0f));
            if (Toggle)
            {
                GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition + new OpenTK.Mathematics.Vector2(3, 3), new OpenTK.Mathematics.Vector2(10, 10), Color);
            }
            GameUI.This.DrawText(Text, RenderPosition + new OpenTK.Mathematics.Vector2(Size.X + 2, 2), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

        }

    }
}
