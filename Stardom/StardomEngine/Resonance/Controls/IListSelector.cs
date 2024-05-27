using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public class IListSelector : IControl
    {

        public ListItemSelected OnItemSelected
        {
            get;
            set;
        }

        private string CurrentSelection="";
        public ListItem CurrentItem
        {
            get;
            set;
        }

        public IList List
        {
            get;
            set;
        }

        private bool Open
        {
            get;
            set;
        }

        public IListSelector()
        {

            List = new IList();

        }
        public override void OnMouseDown(int button)
        {
            //base.OnMouseDown(button);
            if (Open)
            {
                //RemoveControl(List);
                GameUI.This.Overlay.Remove(List);
                Open = false;
            }
            else
            {
                GameUI.This.Overlay.Add(List);
                Open = true;
                List.Set(new OpenTK.Mathematics.Vector2(RenderPosition.X, RenderPosition.Y+Size.Y), new OpenTK.Mathematics.Vector2(Size.X, 180), "");
                List.OnItemSelected = (item) =>
                {

                    GameUI.This.Overlay.Remove(List);
                    //RemoveControl(List);
                    Open = false;
                    OnItemSelected?.Invoke(item);
                    CurrentItem = item;
                };
                //AddControl(List);
                // var item = .AddItem(v);
               


            }
        }

        public override void Render()
        {
            GameUI.This.DrawRect(GameUI.Theme.TextBoxSlice, 10, 10, RenderPosition, Size, Color);

            if (CurrentItem == null)
            {

                CurrentItem = List.Items[0];

            }

            int th = (int)GameUI.This.TextHeight("") / 2;
            int tw = (int)GameUI.This.TextWidth(CurrentItem.Name) / 2;

            if (CurrentItem != null)
            {
                GameUI.This.DrawText(CurrentItem.Name, new OpenTK.Mathematics.Vector2(RenderPosition.X + Size.X / 2 - tw, RenderPosition.Y + Size.Y / 2 - th), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            }
        

            //if (ShowLabel)
            {
            //    GameUI.This.DrawText(EType.Name, new Vector2(RenderPosition.X + Size.X + 8, RenderPosition.Y + 3), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

            }

            RenderChildren();

            //base.Render();

        }

    }
}
