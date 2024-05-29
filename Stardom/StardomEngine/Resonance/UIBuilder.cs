using StardomEngine.Resonance.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance
{
    public class UIBuilder
    {
        private IControl Root
        {
            get;
            set;
        }
        public UIBuilder(IControl root)
        {

            Root = root;

        }

        public IPanel Panel(Vector2 position,Vector2 size)
        {
            var panel = new IPanel().Set(position, size, "") as IPanel;
            Root.AddControl(panel);
            return panel;
        }

        public IButton Button(Vector2 position,Vector2 size,string text)
        {

            var button = new IButton().Set(position, size, text) as IButton;

            Root.AddControl(button);
            return button;

        }

    }
}
