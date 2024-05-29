﻿using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Texture;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StardomEngine.App;

namespace StardomEngine.Resonance
{
    public delegate void Dragged(int x, int y);
    public delegate void ControlAdded(IControl root, IControl added);
    public delegate void TextChanged(IControl control, string text);
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

        public ControlAdded OnControlAdded
        {
            get;
            set;
        }
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                TextChanged();
            }
        }
        protected string _Text = "";

        public TextChanged OnTextChanged
        {
            get;set;
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

        public IControl TabNext
        {
            get;
            set;
        }
        public Dragged OnDragged
        {
            get;
            set;
        }

        public bool AnchorToRoot
        {
            get;
            set;
        }

        public Vector2 RenderOffset
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
                    position = Root.RenderPosition+Root.RenderOffset;
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

        public Vector2 ContentSize
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public bool ShiftOn
        {
            get;
            set;
        }

        public int AlignSize
        {
            get;
            set;
        }

        public Vector2 Offset
        {
            get;
            set;
        }

        public bool Built
        {
            get;
            set;
        }

        public IControl()
        {
            AlignSize = -1;
            Color = new Vector4(1, 1, 1, 1);
            Controls = new List<IControl>();
            Root = null;
            Image = null;
            RenderOffset = new Vector2(0, 0);

        }

        public virtual void Build()
        {

        }
        public void Center()
        {

            var position = new Vector2(StarApp.FrameWidth / 2 - Size.X / 2, StarApp.FrameHeight / 2 - Size.Y / 2);
            Set(position, Size, Text);


        }

        public void AddControl(IControl control)
        {

            control.Root = this;
            Controls.Add(control);
            UpdatedContent();
            UpdateContentSize();
            OnControlAdded?.Invoke(this, control);
            control.AfterSet();

        }

        public void RemoveControl(IControl control)
        {

            Controls.Remove(control);

        }


        public virtual void TextChanged()
        {

        }

        public void UpdatedContent()
        {
            int by = 0;
            foreach (var control in Controls)
            {
                if (control.RenderPosition.Y-control.Size.Y > by)
                {
                    by = (int)control.Position.Y - (int)control.Size.Y;
                }
            }
            ContentSize = new Vector2(0, by);
        }
        public virtual void UpdateContentSize()
        {

        }

        public void AddControls(params IControl[] controls)
        {
            foreach(var control in controls)
            {
                AddControl(control);
            }
        }

        public IControl SetPosition(Vector2 position)
        {
            Position = position;
            AfterSet();
            foreach (var control in Controls)
            {
                control.Set(control.Position, control.Size, control.Text);
            }
            UpdatedContent();
            UpdateContentSize();
            return this;
        }
        public IControl Set(Vector2 position,Vector2 size,string text="")
        {
            if (size.X < 2)
            {
                size.X = 2;
            }
            if (size.Y < 2)
            {
                size.Y = 2;
            }
            Position = position;
            Size = size;
            Text = text;
            AfterSet();
            foreach(var control in Controls)
            {
                control.Set(control.Position, control.Size, control.Text);
            }
            UpdatedContent();
            UpdateContentSize();
            return this;
        }

        public virtual void AfterSet()
        {



        }

        public virtual UILayoutBuilder GetLayoutBuilder()
        {
            UILayoutBuilder builder = new UILayoutBuilder(this);
            return builder;
        }

        public virtual UIBuilder GetBuilder()
        {
            return new UIBuilder(this);
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

        public virtual void OnMouseMove(Vector2 position,Vector2 delta)
        {

        }
        public virtual void OnKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys key)
        {

        }

        public virtual void OnKey(OpenTK.Windowing.GraphicsLibraryFramework.Keys key)
        {

        }

        public virtual void OnMoved(Vector2 moved)
        {

        }


        public virtual void OnActivate()
        {
            Console.WriteLine("Activated:");
        }

        public virtual void OnDeactivate()
        {
            Console.WriteLine("On deactivate");
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

        public void SetStencil(Texture2D stencil,int shrink=4)
        {

            GL.Enable(EnableCap.StencilTest);
            GL.Clear(ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit);

            GL.ClearStencil(0); // Reset stencil buffer to 0
            GL.StencilMask(0xFF);

            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);


            //GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            //GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            //GL.StencilMask(0xFF);


           // GL.ClearStencil(0);
            GL.ColorMask(false, false, false, false);

            GameUI.This.DrawRect(stencil, RenderPosition+new Vector2(shrink,shrink), Size+new Vector2(-shrink*2,-shrink*2), Color, 0, false);

            GL.ColorMask(true, true, true, true);

            GL.StencilFunc(StencilFunction.Equal, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep); // Keep stencil values
            GL.StencilMask(0x00); // Disable writing to the stencil buffer

        }

        public void SetStencil(TextureSlice stencil,int shrink=4)
        {

            GL.Enable(EnableCap.StencilTest);
            GL.Clear(ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit);

            GL.ClearStencil(0); // Reset stencil buffer to 0
            GL.StencilMask(0xFF);

            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);


            //GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            //GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            //GL.StencilMask(0xFF);


            // GL.ClearStencil(0);
            GL.ColorMask(false, false, false, false);

     GameUI.This.DrawRect(stencil,64,64, RenderPosition+new Vector2(shrink,shrink), Size+new Vector2(-shrink*2,-shrink*2), Color, 0, false);

            GL.ColorMask(true, true, true, true);

            GL.StencilFunc(StencilFunction.Equal, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep); // Keep stencil values
            GL.StencilMask(0x00); // Disable writing to the stencil buffer

        }

        public void ClearStencil()
        {
            GL.Disable(EnableCap.StencilTest);


            GL.StencilMask(0xFF); // Allow writing to the entire stencil buffer
            GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        }

        public virtual void PreRender()
        {

            PreRenderChildren();

        }

        public virtual void Render()
        {

            RenderChildren();
        }
        public void PreRenderChildren()
        {

            foreach(var control in Controls)
            {
                control.PreRender();
            }

        }
        public void RenderChildren()
        {

            foreach(var control in Controls)
            {
             //   control.Position = control.Position + Offset;
                control.Render();
           //     control.Position = control.Position - Offset;
            }

        }


    }
}
