﻿using StardomEngine.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel.Design;
using System.Net.Http.Headers;

namespace StardomEngine.Resonance.Controls
{
    public class IWindowContent : IControl
    {

        public bool Rect
        {
            get;
            set;
        }

        public IWindowContent()
        {

        }

        public override void Render()
        {
            //base.Render();

            var bg = GameUI.This.GrabBG(RenderPosition, Size);

            // Color = new OpenTK.Mathematics.Vector4(Color.X, Color.Y, Color.Z, 1.0f);
            if (Rect)
            {
                GameUI.This.DrawRect(bg, RenderPosition, Size, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), 0.003f, true, GameUI.Theme.WindowContentsRect, 0, Refracter);
                // Color = new OpenTK.Mathematics.Vector4(Color.X, Color.Y, Color.Z,0.2f);
                GameUI.This.DrawRect(GameUI.Theme.WindowContentsSlice, 64, 64, RenderPosition, Size, Color, 0, false);
            }
            else
            {
                GameUI.This.DrawRect(bg, RenderPosition, Size, new OpenTK.Mathematics.Vector4(1, 1, 1, 1), 0.003f, true, GameUI.Theme.FrameGuide, 0, Refracter);
                // Color = new OpenTK.Mathematics.Vector4(Color.X, Color.Y, Color.Z,0.2f);
                GameUI.This.DrawRect(GameUI.Theme.FrameSlice, 64, 64, RenderPosition, Size, Color, 0, false);
            }
          


            int windowHeight = (int)StarApp.FrameHeight;
            int bottomLeftY = windowHeight - (int)RenderPosition.Y - (int)Size.Y;

            // Enable scissor testing
            // GL.Enable(EnableCap.ScissorTest);

            // Set the scissor box
            // GL.Scissor((int)RenderPosition.X, bottomLeftY, (int)Size.X, (int)Size.Y);


            if (Rect)
            {
                SetStencil(GameUI.Theme.WindowContentsRect, 4);
            }
            else
            {
                SetStencil(GameUI.Theme.FrameSlice, 0);
            }
            GameUI.This.DrawRect(GameUI.Theme.UIGradient, RenderPosition, Size, new OpenTK.Mathematics.Vector4(0.6f, 0.6f, 0.6f, 0.6f));

            GL.Disable(EnableCap.StencilTest);


            GL.StencilMask(0xFF); // Allow writing to the entire stencil buffer
            GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);

            if (Rect)
            {
                SetStencil(GameUI.Theme.WindowContentsSlice, 2);
            }
            else
            {
                SetStencil(GameUI.Theme.FrameSlice, 4);
            }

            RenderChildren();

            GL.Disable(EnableCap.StencilTest);


            GL.StencilMask(0xFF); // Allow writing to the entire stencil buffer
            GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);


            //GL.Scissor(0, 0, StarApp.FrameWidth, StarApp.FrameHeight);
            // GL.Disable(EnableCap.ScissorTest);
            return;
            foreach (var control in Controls)
            {
                if (control.RenderPosition.Y > RenderPosition.Y)
                {
                    if (control.RenderPosition.Y < RenderPosition.Y + Size.Y)
                    {
                        control.Render();
                    }
                }
            }
        }

    }
}
