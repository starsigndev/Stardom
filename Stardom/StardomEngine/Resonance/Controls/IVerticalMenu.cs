using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using StardomEngine.Resonance.SmartImages;

namespace StardomEngine.Resonance.Controls
{

    public class IVerticalMenu : IControl
    {
        public static SIListBox ListBoxImage;

     

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
            if (ListBoxImage == null)
            {
                ListBoxImage = new SIListBox();
            }
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
                if (OverItem.OnItemSelected != null)
                {
                    OverItem.OnItemSelected.Invoke(OverItem);
                    Root.RemoveControl(this);

                    var pmen = Root as IVerticalMenu;
                    if (pmen != null)
                    {
                        //pmen

                    }
                    else
                    {
                        var mm = Root as IMenu;
                        foreach(var item in mm.Items)
                        {
                            if (item.NextMenu == this) 
                            {
                                item.NextMenu = null;
                            }
                        }
                       // var mm = Root as IMenu;
                    }

                    return;
                }
                if (OverItem.NextMenu != null)
                {
                    this.RemoveControl(OverItem.NextMenu);
                    OverItem.NextMenu = null;
                }
                else
                {
                    if (OverItem.Items.Count == 0) return;
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
                    OverItem.NextMenu.Set(new Vector2(OverItem.DrawX+Size.X+2,OverItem.DrawY), new Vector2(w+40, h), "");
                    this.AddControl(OverItem.NextMenu);


                }
            }
        }

        public override void Render()
        {
            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.WindowTitle, RenderPosition, Size, Color);
            //ListBoxImage.Draw(RenderPosition, Size, new Vector4(1, 1, 1, 1));


            int item_y = 10;

            foreach(var item in Items)
            {

                if (item == OverItem && item.Seperator==false)
                {
                    GameUI.This.DrawRect(GameUI.Theme.TextBoxSlice,10,10, RenderPosition + new Vector2(0, item_y-5), new Vector2(Size.X, GameUI.This.TextHeight("")+10),Vector4.One);
                }
                if(item.Items.Count>0)
                {
                    GameUI.This.DrawRect(GameUI.Theme.ArrowH, new Vector2(RenderPosition.X + Size.X - 25, RenderPosition.Y + item_y), new Vector2(15, 15), Color);
                }
                if (item.Icon != null)
                {
                    GameUI.This.DrawRect(item.Icon, new Vector2(RenderPosition.X + 3, RenderPosition.Y + item_y), new Vector2(16, 16), new Vector4(1, 1, 1, 1));
                }

                if (item.Seperator)
                {

                    GameUI.This.DrawRect(GameUI.Theme.Frame, new Vector2(RenderPosition.X + 15, RenderPosition.Y + item_y + 8), new Vector2(Size.X - 30, 2), new Vector4(1, 1, 1, 1));

                }
                else
                {
                    GameUI.This.DrawText(item.Text, new OpenTK.Mathematics.Vector2(RenderPosition.X + 30, RenderPosition.Y + item_y + 2), Vector4.One);
                }

                item.DrawX = (int)0;
                item.DrawY = (int)item_y - 10;

                item_y += (int)GameUI.This.TextHeight("") + 10;

            }

            RenderChildren();

        }

    }
}
