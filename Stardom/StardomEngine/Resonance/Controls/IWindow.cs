﻿using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IWindow : IControl
    {

        public UIBuilder Builder
        {
            get;
            set;
        }
        public static TextureSlice ShadowSlice
        {
            get;
            set;
        }

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

        public bool CastShadow
        {
            get;
            set;
        }

        public bool RectDesign
        {
            get;
            set;
        }

        

        public IWindow(bool include_scrollers = false,bool rect_design=true)
        {
            if (ShadowSlice == null)
            {
                ShadowSlice = new TextureSlice(new Texture2D("data/ui/shadow.png"), 32, 32);
            }
            Title = new IWindowTitle();
            Contents = new IWindowContent();
            VScroller = new IVerticalScroller();
            Resizer = new IButton();
            Resizer.Text = "*";
            RectDesign = true;
            Contents.Rect = rect_design;
            CastShadow = true;
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
                Vector2 new_size = Size + new OpenTK.Mathematics.Vector2(x, y);
                if(new_size.X<200)
                {
                    new_size.X = 200;
                }
                if (new_size.Y < 200)
                {
                    new_size.Y = 200;
                }
                Set(Position,new_size,Text);
            };
            Resizer.Icon = GameUI.Theme.Resizer;
           
        }

        public override void AfterSet()
        {
            //base.AfterSet();
            Title.Set(new OpenTK.Mathematics.Vector2(1, 0), new OpenTK.Mathematics.Vector2(Size.X, 20), Text);
            Contents.Set(new OpenTK.Mathematics.Vector2(0, 20),new OpenTK.Mathematics.Vector2(Size.X, Size.Y-20), "");
            VScroller.Set(new OpenTK.Mathematics.Vector2(Size.X - 15, 22), new OpenTK.Mathematics.Vector2(13, Size.Y-30), "");
            Resizer.Set(new OpenTK.Mathematics.Vector2(Size.X - 16, Size.Y-16), new OpenTK.Mathematics.Vector2(16, 16), "*");
            
            if (!Built)
            {
                Builder = new UIBuilder(Contents);
                Build();
                Built = true;
            }
            else
            {
                if (Contents.Layout != null)
                {
                    Contents.Layout.Set(new Vector2(10,150),Contents.Size,Contents.Text);
                }
            }
        }

        public override void UpdateContentSize()
        {
            //base.UpdateContentSize();

          
        }

        public override void Render()
        {
            //base.Render();

            if (CastShadow)
            {
                // GameUI.This.DrawRect(Shadow, RenderPosition + new OpenTK.Mathematics.Vector2(32, 32), Title.Size, new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 0.7f));
                Position = Position + new Vector2(32, 32);
                SetStencil(GameUI.Theme.FrameSlice, 0);
                Position = Position + new Vector2(-32, -32);
                ClearStencil();
                GameUI.This.DrawRect(ShadowSlice,16,16, RenderPosition + new OpenTK.Mathematics.Vector2(32, 32), Size, new OpenTK.Mathematics.Vector4(0.8f, 0.8f, 0.8f, 0.65f));
            }

            RenderChildren();

        }

    }
}
