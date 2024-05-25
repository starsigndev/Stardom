using OpenTK.Mathematics;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class MenuItem
    {
        public IVerticalMenu NextMenu
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }

        public Texture2D Icon
        {
            get;
            set;
        }

        public List<MenuItem> Items
        {
            get;
            set;
        }

        public int DrawX
        {
            get;
            set;
        }

        public int DrawY
        {
            get;
            set;
        }

        public MenuItem(string text)
        {
            Text = text;
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

    }
    public class IMenu : IControl
    {

        public List<MenuItem> Items
        {
            get;
            set;
        }

        public MenuItem OverItem
        {
            get;
            set;
        }

        public IMenu()
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
            //base.OnMouseMove(position, delta);
            int menu_x = 15;

            int th = (int)GameUI.This.TextHeight("") / 2;


            foreach (MenuItem item in Items)
            {

                int tw = (int)GameUI.This.TextWidth(item.Text);

             //   GameUI.This.DrawText(item.Text, new OpenTK.Mathematics.Vector2(RenderPosition.X + menu_x, RenderPosition.Y + Size.Y / 2 - th + 2), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
                if(position.X>menu_x-15 && position.X < menu_x-15+tw+15)
                {
                    OverItem = item;
                    break;
                }

                menu_x += tw + 15;

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
                    
                    foreach(var item in Items)
                    {
                        if (item.NextMenu != null)
                        {
                            this.RemoveControl(item.NextMenu);
                            item.NextMenu = null;
                        }
                    }
                    if (OverItem.Items.Count == 0) return;
                    OverItem.NextMenu = new IVerticalMenu();
                    int w = 0;
                    int h = (OverItem.Items.Count) * (int)(10 + GameUI.This.TextHeight(""));
                    foreach(MenuItem item in OverItem.Items)
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
                    OverItem.NextMenu.Set(new Vector2(OverItem.DrawX, 30), new Vector2(w+40, h),"");
                    this.AddControl(OverItem.NextMenu);


                }
            }
        }
        public override void OnDeactivate()
        {
            //base.OnDeactivate();
            foreach(var item in Items)
            {
                if (item.NextMenu!=null)
                {
                 //   RemoveControl(item.NextMenu);
                }
            }
        }
        public override void Render()
        {
            //base.Render();

            GameUI.This.DrawRect(GameUI.Theme.MenuBar, RenderPosition+new OpenTK.Mathematics.Vector2(-10,1), Size+new OpenTK.Mathematics.Vector2(20,0), Color);

            int menu_x = 15;

            int th = (int)GameUI.This.TextHeight("") / 2;

        
            foreach (MenuItem item in Items)
            {

                int tw = (int)GameUI.This.TextWidth(item.Text);

                if(OverItem == item)
                {
                    GameUI.This.DrawRect(GameUI.Theme.TextBox, new Vector2(RenderPosition.X + menu_x-15, RenderPosition.Y), new Vector2(tw + 30, Size.Y+3), Color);
                }
                GameUI.This.DrawText(item.Text, new OpenTK.Mathematics.Vector2(RenderPosition.X + menu_x, RenderPosition.Y + Size.Y / 2 - th+2), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
                item.DrawX = menu_x - 15;

                menu_x += tw + 15;

            }

            RenderChildren();

        }


    }
}
