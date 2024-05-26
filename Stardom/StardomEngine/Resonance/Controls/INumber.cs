using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public delegate void NumberChanged(INumber control, float value);
    public class INumber : IControl
    {
        IButton Up, Down;
        ITextBox Number;

        public NumberChanged OnNumberChanged { get; set; }

        public bool FloatingPoint
        {
            get;
            set;
        }

        public float Increment
        {
            get;
            set;
        }

        public INumber(bool floating_point = false)
        {
            Increment = 1;
            Up = new IButton();
            Up.Icon = GameUI.Theme.ArrowUp;
            
            Down = new IButton();
            Down.Icon = GameUI.Theme.ArrowDown;
            Number = new ITextBox();
            FloatingPoint = floating_point;
            Number.Numeric = true;

            Number.OnTextChanged = (c, text) =>
            {
                try
                {
                    OnNumberChanged?.Invoke(this, float.Parse(text));
                }
                catch
                {
                    OnNumberChanged?.Invoke(this, 0.0f);
                }
            };

            if (floating_point)
            {
                Number.Text = "0.0";
            }
            else
            {
                Number.Text = "0";
            }
            Up.OnClick = (b, mb) =>
            {
                if (FloatingPoint)
                {
                    var fv = float.Parse(Number.Text);
                    fv = fv + Increment;
                    Number.Text = fv.ToString();
                }
                else
                {
                    int iv = int.Parse(Number.Text);
                    iv = iv + (int)Increment;
                    Number.Text = iv.ToString();
                }
                try
                {
                    OnNumberChanged?.Invoke(this, float.Parse(Number.Text));
                }
                catch
                {
                    OnNumberChanged.Invoke(this, 0.0f);
                }
            };
            Down.OnClick = (b, mb) =>
            {
                if (FloatingPoint)
                {
                    var fv = float.Parse(Number.Text);
                    fv = fv - Increment;
                    Number.Text = fv.ToString();//.Substring(0, 3);
                }
                else
                {
                    int iv = int.Parse(Number.Text);
                    iv = iv - (int)Increment;
                    Number.Text = iv.ToString();
                }
                try
                {
                    OnNumberChanged?.Invoke(this, float.Parse(Number.Text));
                }
                catch
                {
                    OnNumberChanged.Invoke(this, 0.0f);
                }
            };
            AddControls(Up, Down, Number);

        }
    

        public override void AfterSet()
        {
            Up.Set(new OpenTK.Mathematics.Vector2(0, 0),new OpenTK.Mathematics.Vector2(28,28),"/\\");
            Down.Set(new OpenTK.Mathematics.Vector2(108, 0), new OpenTK.Mathematics.Vector2(28,28), "\\/");
            Number.Set(new OpenTK.Mathematics.Vector2(28,0),new OpenTK.Mathematics.Vector2(80,28),Number.Text);
            //base.AfterSet();/


        }

    }
}
