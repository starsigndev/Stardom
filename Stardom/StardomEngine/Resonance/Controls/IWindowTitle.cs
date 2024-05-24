using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IWindowTitle : IControl
    {

        public IWindowTitle()
        {
            Color = new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 1.0f);
        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);

            GameUI.This.Windows.Remove(this.Root as IWindow);
            GameUI.This.Windows.Add(this.Root as IWindow);
            //Root.Root.RemoveControl(Root);
            //Root.Root.AddControl(Root);

        }
        public override void OnEnter()
        {
            //base.OnEnter();
            Color = new OpenTK.Mathematics.Vector4(1, 1, 1, 1);

        }

        public override void OnLeave()
        {
            //base.OnLeave();
            Color = new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 1.0f);
        }

//        public override void OnDragged(Vector2 moved)
  //      {

            //base.OnDragged(moved);
            //Position = Position + moved;
    //    }

        public override void Render()
        {

            int tw = (int)GameUI.This.TextWidth(Text, 0.77f) / 2;
            int th = (int)GameUI.This.TextHeight(Text, 0.77f) / 2;

            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.WindowTitle, RenderPosition, Size, Color);
            GameUI.This.DrawText(Text, RenderPosition+new OpenTK.Mathematics.Vector2(Size.X/2-tw,Size.Y/2-th), new OpenTK.Mathematics.Vector4(1, 1, 1, 1), 0.77f);
        }

    }
}
