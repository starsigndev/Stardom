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

        public IWindowContent Contents
        {
            get;
            set;
        }

        public IButton Resizer
        {
            get;
            set;
        }

        public IVerticalScroller VScroller
        {
            get;
            set;
        }

        public IWindow(bool include_scrollers = false)
        {

            Title = new IWindowTitle();
            Contents = new IWindowContent();
            VScroller = new IVerticalScroller();
            Resizer = new IButton();
            Resizer.Text = "*";
            VScroller.OnValueChange = (val) =>
            {
                //Console.WriteLine("Value:" + val);
                Contents.RenderOffset = new OpenTK.Mathematics.Vector2(0, -val * VScroller.MaxValue);
            };

            AddControl(Contents);
            AddControl(Title);
            
            if (include_scrollers)
            {
                AddControl(VScroller);
            }
            AddControl(Resizer);

            Title.OnDragged = (x, y) =>
            {
                Position = Position + new OpenTK.Mathematics.Vector2(x, y);
            };

            Contents.OnControlAdded = (root, control) =>
            {
                VScroller.MaxValue = (int)Contents.ContentSize.Y;
            };

            Resizer.OnDragged = (x,y) =>
            {
                Set(Position, new OpenTK.Mathematics.Vector2(Size.X + x, Size.Y + y),Text);
            };

        }

        public override void AfterSet()
        {
            //base.AfterSet();
            Title.Set(new OpenTK.Mathematics.Vector2(0, 0), new OpenTK.Mathematics.Vector2(Size.X, 20), Text);
            Contents.Set(new OpenTK.Mathematics.Vector2(0, 10),new OpenTK.Mathematics.Vector2(Size.X, Size.Y), "");
            VScroller.Set(new OpenTK.Mathematics.Vector2(Size.X - 15, 22), new OpenTK.Mathematics.Vector2(13, Size.Y-30), "");
            Resizer.Set(new OpenTK.Mathematics.Vector2(Size.X - 16, Size.Y-6), new OpenTK.Mathematics.Vector2(16, 16), "*");
        }

        public override void UpdateContentSize()
        {
            //base.UpdateContentSize();

          
        }

    }
}
