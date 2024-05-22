using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IWindow : IControl
    {

        public IWindowTitle Title
        {
            get;
            set;
        }

        public IPanel Contents
        {
            get;
            set;
        }

        public IWindow()
        {

            Title = new IWindowTitle();
            Contents = new IPanel();

            AddControl(Contents);
            AddControl(Title);

            Title.OnDragged = (x, y) =>
            {
                Position = Position + new OpenTK.Mathematics.Vector2(x, y);
            };

        }

        public override void AfterSet()
        {
            //base.AfterSet();
            Title.Set(new OpenTK.Mathematics.Vector2(0, 0), new OpenTK.Mathematics.Vector2(Size.X, 30), Text);
            Contents.Set(new OpenTK.Mathematics.Vector2(0, 0),new OpenTK.Mathematics.Vector2(Size.X, Size.Y), "");
        }

    }
}
