using StardomEngine.Resonance.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance
{
    public class UILayoutBuilder
    {
        public IControl Root
        {
            get;
            set;
        }

        public Stack<IControl> ControlStack
        {
            get;
            set;
        }

        public UILayoutBuilder(IControl root)
        {
            ControlStack = new Stack<IControl>();
            Root = root;
        }

        public void BeginVertical(FillAlignment allignment,int space=0,int size=0)
        {

            
            IVBox box = new IVBox();
            box.Alignment = allignment;
            box.AlignSize = size;
            box.AlignSpace = space;
            if (ControlStack.Count == 0)
            {
                Root.AddControl(box);
                box.AfterSet();
                if(Root is IWindowContent)
                {
                    var rw = Root as IWindowContent;
                    rw.Layout = box;
                }
            }

            if (ControlStack.Count > 0)
            {
                ControlStack.Peek().AddControl(box);
                box.AfterSet();
            }
            ControlStack.Push(box);


        }

        public void BeginHorizontal(FillAlignment alignment,int space=0,int size=0)
        {
            IHBox box = new IHBox();
            box.AlignSpace = space;
            box.AlignSize = size;
            box.Alignment = alignment;

            if (ControlStack.Count == 0)
            {
                Root.AddControl(box);
                box.AfterSet();

            }

            if (ControlStack.Count > 0)
            {
                ControlStack.Peek().AddControl(box);
                box.AfterSet();
            }
            ControlStack.Push(box);
        }

        public ITextBox TextBox(string text, int size = -1, int label_size = -1,string value="") { 
            ILabel lab = new ILabel(text);
            ITextBox box = new ITextBox();
            box.Text = value;
            if (label_size == -1)
            {
               // lab.AlignSize = (int)GameUI.This.TextWidth(text, lab.Scale) + 5;
            }
            else
            {
                lab.AlignSize = label_size;
            }
            box.AlignSize = size;

            var root = Peek();
            root.AddControls(lab, box);
            return box;
        }
        public IPanel Panel(Vector2 position,Vector2 size)
        {
            var panel = new IPanel();
            panel.Set(position, size,"");

            if (ControlStack.Count == 0)
            {
                Root.AddControl(panel);
            }
            else
            {
                Peek().AddControl(panel);
            }
            
            ControlStack.Push(panel);
            
            return panel;

        }
        public ILabel Label(string text,int size = -1)
        {
            ILabel label = new ILabel(text);
            label.AlignSize = size;
            var root = Peek();
            root.AddControl(label);
            return label;
        }
        public IButton Button(string text,int size=-1,Click click = null)
        {
            IButton new_button = new IButton();
            new_button.OnClick = click;
            new_button.Text = text;
            new_button.AlignSize = size;
            var root = Peek();
            root.AddControl(new_button);
            return new_button;
        }

        public void End()
        {
            dynamic con = ControlStack.Peek();

            con.Calculate();

            ControlStack.Pop();
        }

        public dynamic Peek()
        {
            return ControlStack.Peek();
        }

    }
}
