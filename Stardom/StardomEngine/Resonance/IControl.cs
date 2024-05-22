using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Texture;

namespace StardomEngine.Resonance
{
    public delegate void Dragged(int x, int y);
    public class IControl
    {

        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 Size
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public IControl Root
        {
            get;
            set;
        }

        public List<IControl> Controls
        {
            get;
            set;
        }

        public Dragged OnDragged
        {
            get;
            set;
        }
        public Vector2 RenderPosition
        {
            get
            {
                Vector2 position = Vector2.Zero;
                if (Root != null)
                {
                    position = Root.RenderPosition;
                }

                return position + Position;

            }

        }

        public Vector4 Color
        {
            get;
            set;
        }

        public Texture2D Image
        {
            get;
            set;
        }

        public Texture2D Refracter
        {
            get;
            set;
        }

        public IControl()
        {

            Color = new Vector4(1, 1, 1, 1);
            Controls = new List<IControl>();
            Root = null;
            Image = null;

        }


        public void AddControl(IControl control)
        {

            control.Root = this;
            Controls.Add(control);

        }

        public void AddControls(params IControl[] controls)
        {
            foreach(var control in controls)
            {
                AddControl(control);
            }
        }

        public IControl Set(Vector2 position,Vector2 size,string text="")
        {
            Position = position;
            Size = size;
            Text = text;
            AfterSet();
            foreach(var control in Controls)
            {
                control.Set(control.Position, control.Size, control.Text);
            }
            return this;
        }

        public virtual void AfterSet()
        {



        }

        public bool InBounds(Vector2 position)
        {

            if (position.X >= RenderPosition.X && position.X < RenderPosition.X + Size.X)
            {
                if (position.Y >= RenderPosition.Y && position.Y < RenderPosition.Y + Size.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void OnMoved(Vector2 moved)
        {

        }
        public virtual void OnEnter()
        {

        }

        public virtual void OnLeave()
        {

        }

        public virtual void OnMouseDown(int button)
        {

        }

        public virtual void OnMouseUp(int button)
        {

        }

        public virtual void Update()
        {

        }
        public virtual void Render()
        {

            RenderChildren();
        }

        public void RenderChildren()
        {

            foreach(var control in Controls)
            {
                control.Render();
            }

        }


    }
}
