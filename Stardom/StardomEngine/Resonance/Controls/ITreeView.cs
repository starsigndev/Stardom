using OpenTK.Mathematics;
using StardomEngine.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class TreeItem
    {
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

        public List<TreeItem> Items
        {
            get;
            set;
        }

        public bool Open
        {
            get;
            set;
        }

        public TreeItem(string text)
        {
            Text = text;
            Items = new List<TreeItem>();
        }
    
         public TreeItem AddItem(string text)
        {

            TreeItem item = new TreeItem(text);
            Items.Add(item);
            return item;

        }

    }
    public class ITreeView : IControl
    {

        public TreeItem RootItem
        {
            get;
            set;
        }

        public TreeItem OverItem
        {
            get;
            set;
        }

        public ITreeView()
        {

            RootItem = new TreeItem("Root");
            RootItem.Open = true;

        }

        public int CheckItem(TreeItem item,int cx,int cy,Vector2 position)
        {

            if (position.X < Size.X)
            {
                if(position.Y>cy && position.Y<cy+GameUI.This.TextHeight("")+8)
                {
                    OverItem = item;
                }
            }

            if (item.Open)
            {
                foreach (var sitem in item.Items)
                {

                    cy = CheckItem(sitem, cx + 10, cy  + (int)GameUI.This.TextHeight("") + 8,position);

                }
            }
            return cy;

        }

        public override void OnMouseMove(Vector2 position, Vector2 delta)
        {
            //base.OnMouseMove(position, delta);
            CheckItem(RootItem, 5, 5, position);
        }

        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if (OverItem != null)
            {
                OverItem.Open = OverItem.Open ? false : true;
            }
        }

        public int RenderItem(TreeItem item,int dx,int dy)
        {

            if (item == OverItem)
            {

                GameUI.This.DrawRect(GameUI.Theme.ListBoxSlice, 6, 6, RenderPosition + new Vector2(0, dy-3), new Vector2(Size.X, GameUI.This.TextHeight("") + 6), new Vector4(1, 1, 1, 1));

            }

            if (item.Items.Count > 0)
            {
                GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition + new OpenTK.Mathematics.Vector2(dx, dy + 2), new OpenTK.Mathematics.Vector2(10, 10), Color);
                GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition + new OpenTK.Mathematics.Vector2(dx + 1, dy + 3), new OpenTK.Mathematics.Vector2(8, 8), new OpenTK.Mathematics.Vector4(0.4f, 0.4f, 0.4f, 1.0f));
                if (item.Open)
                {
                    GameUI.This.DrawRect(GameUI.Theme.Button, RenderPosition + new OpenTK.Mathematics.Vector2(dx + 2, dy + 4), new OpenTK.Mathematics.Vector2(6, 6), Color);
                }
            }

            GameUI.This.DrawText(item.Text, RenderPosition + new OpenTK.Mathematics.Vector2(dx+14, dy+2), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

            if (item.Open)
            {
                foreach (var sitem in item.Items)
                {

                    dy = RenderItem(sitem, dx + 10, dy + (int)GameUI.This.TextHeight("") + 8);

                }
            }

            return dy;
        }

        public override void Render()
        {

            GameUI.This.DrawRect(GameUI.Theme.White, RenderPosition, Size,new OpenTK.Mathematics.Vector4(0.5f,0.5f,0.5f,1.0f));



            RenderItem(RootItem, 5, 5);


        }

    }
}
