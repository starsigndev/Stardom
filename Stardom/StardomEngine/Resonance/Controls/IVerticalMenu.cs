using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IVerticalMenu : IControl
    {

        public MenuItem OverItem
        {
            get;
            set;
        }

        public List<MenuItem> Items
        {
            get;
            set;
        }
    
        public IVerticalMenu()
        {
            Items = new List<MenuItem>();
        }

        public MenuItem AddItem(MenuItem item)
        {
            Items.Add(item);
            return item;
        }

        public MenuItem AddItem(string item)
        {
            return AddItem(new MenuItem(item));
        }

        public override void OnMouseMove(Vector2 position, Vector2 delta)
        {
            int item_y = 10;

            foreach (var item in Items)
            {

                //  GameUI.This.DrawText(item.Text, new OpenTK.Mathematics.Vector2(RenderPosition.X + 15, RenderPosition.Y + item_y), Vector4.One);
                if (position.Y > item_y-5 && position.Y < item_y + GameUI.This.TextHeight(""))
                {
                    OverItem = item;
                }

                item_y += (int)GameUI.This.TextHeight("") + 10;
            }
        }



        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if (OverItem != null)
            {
                if (OverItem.NextMenu != null)
                {
                    this.RemoveControl(OverItem.NextMenu);
                    OverItem.NextMenu = null;
                }
                else
                {
                    foreach (var item in Items)
                    {
                        if (item.NextMenu != null)
                        {
                            this.RemoveControl(item.NextMenu);
                            item.NextMenu = null;
                        }
                    }
                    OverItem.NextMenu = new IVerticalMenu();
                    int w = 0;
                    int h = (OverItem.Items.Count) * (int)(10 + GameUI.This.TextHeight(""));
                    foreach (MenuItem item in OverItem.Items)
                    {
                        int nw = (int)GameUI.This.TextWidth(item.Text);
                        nw = nw + 30;
                        if (nw > w)
                        {
                            w = nw;
                        }
                        OverItem.NextMenu.AddItem(item);
                    }
                    h = h + 10;
                    OverItem.NextMenu.Set(new Vector2(OverItem.DrawX+Size.X,OverItem.DrawY), new Vector2(w, h), "");
                    this.AddControl(OverItem.NextMenu);


                }
            }
        }

        public override void Render()
        {
            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.ListBox, RenderPosition, Size, Color);

            int item_y = 10;

            foreach(var item in Items)
            {

                if (item == OverItem)
                {
                    GameUI.This.DrawRect(GameUI.Theme.TextBox, RenderPosition + new Vector2(0, item_y-5), new Vector2(Size.X, GameUI.This.TextHeight("")+10),Vector4.One);
                }
                GameUI.This.DrawText(item.Text, new OpenTK.Mathematics.Vector2(RenderPosition.X+15, RenderPosition.Y+item_y), Vector4.One);
                item.DrawX = (int)0;
                item.DrawY = (int)item_y - 10;

                item_y += (int)GameUI.This.TextHeight("") + 10;

            }

            RenderChildren();

        }

    }
}
