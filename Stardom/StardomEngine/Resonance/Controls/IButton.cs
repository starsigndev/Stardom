using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Texture;

namespace StardomEngine.Resonance.Controls
{
    public delegate void Click(IButton button, int mouse);

    public class IButton : IControl
    {

        public Click OnClick
        {
            get;
            set;
        }

        public Texture2D Icon
        {
            get;
            set;
        }

        public IButton()
        {
            Color = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);
        }


        public override void OnMouseDown(int button)
        {
            Color = new Vector4(1, 1.5f, 1.5f, 1.0f);
            //base.OnMouseDown(button);
            OnClick?.Invoke(this,button);
        }

        public override void OnMouseUp(int button)
        {

            Color = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);
            //base.OnMouseUp(button);

        }

        public override void OnEnter()
        {
            //Environment.Exit(1);
            Color = new Vector4(1.1f, 1.1f, 1.1f, 1.0f);
        }

        public override void OnLeave()
        {
            //base.OnLeave();
            Color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        }

        public override void Render()
        {
            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition, Size,Color);

            float tx = RenderPosition.X + Size.X / 2;
            float ty = RenderPosition.Y + Size.Y / 2;

            tx = tx - GameUI.This.TextWidth(Text) / 2;
            ty = ty - GameUI.This.TextHeight(Text) / 2;

            if (Icon != null)
            {
                GameUI.This.DrawRect(Icon, new Vector2(RenderPosition.X + 3, RenderPosition.Y + 3), new Vector2(Size.X - 6, Size.Y - 6), new Vector4(2,2,2,2));
            }
            else
            {
                GameUI.This.DrawText(Text, new Vector2(tx + 2, ty + 2), new Vector4(0, 0, 0, 1));
                GameUI.This.DrawText(Text, new Vector2(tx, ty), Vector4.One);
            }

            RenderChildren();
        }

    }
}
