using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public delegate void ValueChanged(float new_value);
    public class IVerticalScroller : IControl
    {

        public ValueChanged OnValueChange
        {
            get;
            set;
        }
        public int MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
            }
        }

        public IVerticalScroller()
        {
            _MaxValue = 200;

        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            _Dragging = true;
        }

        public override void OnMouseUp(int button)
        {
            //base.OnMouseUp(button);
            _Dragging = false;
        }

        public override void OnMoved(Vector2 moved)
        {
            //base.OnMoved(moved);
            if (dh > Size.Y - 1)
            {
                return;
            }
            int cy = _CurrentValue;
            _CurrentValue += (int)moved.Y;
            if (_CurrentValue < 0)
            {
                _CurrentValue = 0;
            }
            if (_CurrentValue >= _MaxValue)
            {
                _CurrentValue = _MaxValue - 1;
            }
            OnValueChange?.Invoke(GetValue());
        }

        public float GetValue()
        {

            // Constants for minimum and maximum handle sizes
             float minHandleSize = 20.0f;  // Minimum handle size
             float maxHandleSize = Size.Y;  // Maximum handle size is the height of the scroller


            // Ensure _maxValue is not zero to avoid division by zero
            if (_MaxValue == 0)
            {
                dh = (int)maxHandleSize;
                return 0.0f;
            }

            // Calculate the proportion of the content visible in the scroll area
            float contentRatio = Size.Y / _MaxValue;

            // Cap content ratio if needed
            const float maxContentRatio = 0.92f;
            contentRatio = MathF.Min(contentRatio, maxContentRatio);


            if (contentRatio < 0.2)
            {
                contentRatio = 0.2f;
            }

            // Calculate the handle size based on content ratio
            float handleSize = contentRatio * Size.Y;



            if (handleSize == 0) return 0.0f;
            // Clamp the handle size to be within minimum and maximum bounds
            //dh = clamp(handleSize, minHandleSize, maxHandleSize);
            dh = Math.Clamp((int)handleSize, (int)minHandleSize,(int)maxHandleSize);


            // Calculate and return the current value based on the clamped handle size
            float maxScrollableHeight = Size.Y - dh;

            // Ensure avoiding division by zero
            if (maxScrollableHeight == 0)
            {
                return 0.0f;
            }

            // Adjust current value if needed
            if (_CurrentValue + dh > Size.Y)
            {
                _CurrentValue -= ((_CurrentValue + dh) - (int)Size.Y);
            }

            return Math.Clamp(_CurrentValue / maxScrollableHeight, 0.0f, 1.0f);
            //
            //Calculate and return the normalized current value
            //return clamp(static_cast<float>(_CurrentValue) / maxScrollableHeight, 0.0f, 1.0f);



        }

        void SetValue(int value)
        {
            _CurrentValue += -value;
            if (_CurrentValue < 0) _CurrentValue = 0;
            if(_CurrentValue+dh>Size.Y)
            {
                _CurrentValue -= ((int)Size.Y - (_CurrentValue + dh));
            }
        }

        public override void Render()
        {
            if (_MaxValue < 0)
            {
                _CurrentValue = 0;
            }



            float v = GetValue();



            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition, Size, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition + new OpenTK.Mathematics.Vector2(0, _CurrentValue), new OpenTK.Mathematics.Vector2(Size.X, dh), new OpenTK.Mathematics.Vector4(0.4f, 0.4f, 0.4f, 1.0f));

        }

        int _MaxValue = 0;
        int _CurrentValue = 0;
        float _Value = 0;
        float av2 = 0;
        int dh = 0;
        bool _Dragging = false;
        bool over_drag = false;

    }
}
