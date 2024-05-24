using OpenTK.Mathematics;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Platform.Native.macOS.MacOSCursorComponent;

namespace StardomEngine.Resonance.Controls
{

    public delegate void ItemAction(ListItem item, int index, object data);

    public class ListItem
    {
        public int Index = 0;
        public event ItemAction Action;

        public event ItemAction ActionDoubleClick;

        public string Name
        {
            get; set;
        }

        public object Data
        {
            get;
            set;
        }

        public Texture2D Icon
        {
            get;
            set;
        }
    
        public Vector4 Color
        {
            get;
            set;
        }

        public void InvokeAction(ListItem item, int index, object data)
        {
            Action?.Invoke(item, index, data);
        }

        public void InvokeDoubleClickAction(ListItem item, int index, object data)
        {
            ActionDoubleClick?.Invoke(item, index, data);
        }

        public ListItem()
        {
            Index = 0;
            Name = "";
            Action = null;
            Color = new Vector4(0.7f, 0.7f, 0.7f, 0.95f);

        }
    }
    public class IList : IControl
    {
        private IVerticalScroller VerticalScroller
        {
            get;
            set;
        }
        public IPanel ListFrame
        {
            get;
            set;
        }

        public IPanel ListContentsFrame
        {
            get;
            set;
        }

        public List<ListItem> Items
        {
            get;
            set;
        }

        public ListItem OverItem
        {
            get;
            set;
        }
        int ScrollY = 0;
        public IList()
        {

            Items = new List<ListItem>();
            OverItem = null;
          //  DrawOutline = true;
         //   ScissorSelf = true;
            VerticalScroller = new IVerticalScroller();
            AddControl(VerticalScroller);
            //VerticalScroller.OnMove += (item, x, y) =>
            {
                //ScrollValue = new Maths.Position(0, y);

             //   ScrollY = y;
                //Console.WriteLine("Y:" + y);
            };
            /*
            ListFrame = new IFrame();
            ListContentsFrame = new IFrame();
            AddForms(ListFrame);
            ListFrame.AddForm(ListContentsFrame);
            ListContentsFrame.Color = new Maths.Color(0, 1, 1, 1);
            */

        }

        public override void AfterSet()
        {
            VerticalScroller.Set(new Vector2(Size.X - 12, 12), new Vector2(12, Size.Y - 24),"");
            //base.AfterSet();
            //ListFrame.Set(new Maths.Position(0, 0), new Maths.Size(Size.w, Size.h), "");
            //  ListContentsFrame.Set(new Maths.Position(10, 10), new Maths.Size(Size.w - 20, Size.h - 20),"");
        }
        public void CalculateHeight()
        {

            int mh = 0;
            mh = (Items.Count) * (int)(GameUI.This.TextHeight("") + 8);
            Size =  new OpenTK.Mathematics.Vector2(Size.X, mh);

        }
        public ListItem AddItem(string name)
        {
            ListItem item = new ListItem();
            item.Name = name;
            item.Index = Items.Count;
            Items.Add(item);

            return item;
        }



        
        public override void OnMouseMove(Vector2 position,Vector2 moved)
        {
            //base.OnMouseMove(position, delta);
            int ix, iy;

            ix = (int)RenderPosition.X + 5;
            iy =  ScrollY;
            OverItem = null;
            foreach (var item in Items)
            {
                if (position.Y > iy && position.Y < + iy+GameUI.This.TextHeight("") + 8)
                {
                    if (position.X > 0 && position.X < Size.X)
                    {
                        OverItem = item;
                    }
                }
                iy = iy + (int)GameUI.This.TextHeight("") + 8;
            }
        }
        

        
        public override void Update()
        {


            //base.OnUpdate();
            foreach (var item in Items)
            {
                if (item == OverItem)
                {
                    item.Color += (new Vector4(0.2f, 1.2f, 1.2f, 0.97f) - item.Color) * 0.1f;
                }
                else
                {
                    item.Color += (new Vector4(0f,0f, 0f, 0.97f) - item.Color) * 0.1f;
                }
            }
        }
        

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if (OverItem != null)
            {
                OverItem.InvokeAction(OverItem, 0, OverItem.Data);  //Action?.Invoke(OverItem, 0, OverItem.Data);
            }
        }

      //  public override void OnDoubleClick(int button)
        //{
            //base.OnDoubleClick(button);
            //if (OverItem != null)
          //  {
             //   OverItem.InvokeDoubleClickAction(OverItem, 0, OverItem.Data);  //Action?.Invoke(OverItem, 0, OverItem.Data);
           // }
//        }

        public override void Render()
        {
            //base.OnRender()
            //;
           // Draw(UI.Theme.Pure, -1, -1, -1, -1, new Maths.Color(0.5f, 0.5f, 0.5f, 0.95f));
            // Draw(UI.Theme.Frame, 10, 10, Size.w - 20, Size.h - 20, new Maths.Color(3, 3, 3, 1));
            GameUI.This.DrawRect(GameUI.Theme.ListBox, RenderPosition, Size, new Vector4(1, 1, 1, 1));
            int ix, iy;

            ix = (int)RenderPosition.X + 5;
            iy = (int)RenderPosition.Y + 5 - ScrollY;//(int)(VerticalScroller.Value * (float)VerticalScroller.MaxValue);
                                                //  return;
            foreach (var item in Items)
            {
                if (item == OverItem)
                {
                    GameUI.This.DrawRect(GameUI.Theme.TextBox,new Vector2(ix-5,iy-5) ,new Vector2(Size.X,GameUI.This.TextHeight("")+7), new Vector4(1, 1, 1, 1));
                    //Draw(UI.Theme.Frame, ix - 5, iy - 2, Size.w, UI.SystemFont.StringHeight() + 6, new Maths.Color(1f, 1f, 1f, 0.8f));
                }
                if (item.Icon != null)
                {
                    //Draw(item.Icon, ix, iy, 16, 16, new Maths.Color(1, 1, 1, 1));
                    //UI.DrawString(item.Name, ix + 20, iy, item.Color);

                    GameUI.This.DrawText(item.Name, new Vector2(ix + 20, iy), item.Color);
                }
                else
                {
                    //UI.DrawString(item.Name, ix, iy, item.Color);

                    GameUI.This.DrawText(item.Name, new Vector2(ix, iy),item.Color);
                }
                iy = iy + (int)GameUI.This.TextHeight("") + 8;

            }

            VerticalScroller.MaxValue = (iy + ScrollY) - (int)(Size.Y / 4);
            if (VerticalScroller.MaxValue < 64)
            {
                VerticalScroller.MaxValue = 64;
            }


            // ScrollValue = new Maths.Position(0, (int)cy);


        }



    }

}
