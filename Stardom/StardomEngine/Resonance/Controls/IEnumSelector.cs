using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public delegate void EnumSelected(string value, dynamic evalue);
    public class IEnumSelector : IControl
    {
        public System.Type EType
        {
            get;
            set;
        }

        public List<string> Values = new List<string>();

        public int CurrentSelection
        {
            get;
            set;
        }

        private bool Open
        {
            get;
            set;
        }

        public bool ShowLabel
        {
            get;
            set;
        }

        public EnumSelected OnSelected
        {
            get;
            set;
        }

        private IList Selector;


        public IEnumSelector(Type etype)
        {
            ShowLabel = false;

            System.Type enumUnderlyingType = System.Enum.GetUnderlyingType(etype);
            System.Array enumValues = System.Enum.GetValues(etype);

            foreach (object enumValue in enumValues)
            {
                Values.Add(enumValue.ToString());

                //  Console.WriteLine("V:" + Values[Values.Count - 1]);
            }

            EType = etype;

            CurrentSelection = 0;
            Open = false;
            //   DrawOutline = true;

        }

        public override void Render()
        {
            //           base.RenderForm();
            //DrawFrameRounded();
            //Draw(UI.Theme.Pure);
            GameUI.This.DrawRect(GameUI.Theme.TextBox, RenderPosition, Size, Color);


            int th = (int)GameUI.This.TextHeight("") / 2;
            int tw = (int)GameUI.This.TextWidth(Values[CurrentSelection]) / 2;

            GameUI.This.DrawText(Values[CurrentSelection], new OpenTK.Mathematics.Vector2(RenderPosition.X + Size.X/2-tw, RenderPosition.Y + Size.Y/2-th+2), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));


            if (ShowLabel)
            {
                GameUI.This.DrawText(EType.Name, new Vector2(RenderPosition.X + Size.X + 8, RenderPosition.Y + 3), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

            }
        }

        public override void OnMouseDown(int button)
        {

            //base.OnMouseDown(button);
            if (button == 0)
            {

                Open = Open ? false : true;

                if (Open)
                {
                    
                    Selector = new IList();
                    foreach (var v in Values)
                    {
                        var item = Selector.AddItem(v);
                        item.Action += (item, index, data) =>
                        {
                            int i = 0;
                            foreach (var sel in Values)
                            {
                                if (sel == item.Name)
                                {
                                    CurrentSelection = i;
                                    //Child.Remove(Selector);
                                    //Forms.Remove(Selector);
                                    GameUI.This.Overlay.Remove(Selector);


                                    Open = false;
                                    OnSelected?.Invoke(Values[i], Enum.Parse(EType,Values[i]));
                                    break;
                                }
                                i++;
                            }
                        };
                    }
                    Selector.CalculateHeight();
                    //AddForm(Selector);
                    GameUI.This.Overlay.Add(Selector);
                    Selector.Set(new Vector2(RenderPosition.X + 0, RenderPosition.Y + 35), new Vector2(Size.X, Selector.Size.Y));
                    
                }
                else
                {
                    //UI.This.Overlay.Forms.Remove(Selector);
                    //C//hild.Remove(Selector);
                }

            }
        }

        public override void OnDeactivate()
        {
            //base.OnDeactivate();
            if (Selector != null)
            {
                GameUI.This.Overlay.Remove(Selector);
                Selector = null;
                Open = false;
            }
        }

        //public dynamic GetEnum()
        // {

        //}

        /*
        private void Selector_OnSelected(ListItem item)
        {
            int i = 0;
            foreach (var sel in Values)
            {
                if (sel == item.Name)
                {
                    CurrentSelection = i;
                 //   Forms.Remove(Selector);

                    //Child.Remove(Selector);
                    Open = false;
                    OnSelected?.Invoke(Values[i]);
                    break;
                }
                i++;
            }
        }
        */
    }


}
