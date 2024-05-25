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

        public IVerticalScroller VScroller
        {
            get;
            set;
        }

        public IWindow(bool include_scrollers = false)
        {

            Title = new IWindowTitle();
            Contents = new IPanel();
            VScroller = new IVerticalScroller();
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

            Title.OnDragged = (x, y) =>
            {
                Position = Position + new OpenTK.Mathematics.Vector2(x, y);
            };

            Contents.OnControlAdded = (root, control) =>
            {
                VScroller.MaxValue = (int)Contents.ContentSize.Y;
            };

        }

        public override void AfterSet()
        {
            //base.AfterSet();
            Title.Set(new OpenTK.Mathematics.Vector2(0, 0), new OpenTK.Mathematics.Vector2(Size.X, 20), Text);
            Contents.Set(new OpenTK.Mathematics.Vector2(0, 10),new OpenTK.Mathematics.Vector2(Size.X, Size.Y), "");
            VScroller.Set(new OpenTK.Mathematics.Vector2(Size.X - 15, 22), new OpenTK.Mathematics.Vector2(13, Size.Y-30), ""); 
        }

        public override void UpdateContentSize()
        {
            //base.UpdateContentSize();

          
        }

    }
}
