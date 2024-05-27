using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Resonance.Controls
{
    public enum FillAlignment
    {
        Horizontal,Top,Bottom,Fill,Left,Right
    }
    public class IVBox : IControl
    {
        public FillAlignment Alignment
        {
            get;
            set;
        }

        public int AlignSize
        {
            get;
            set;
        }

        public int AlignSpace
        {
            get;
            set;
        }

        public int AlignLength
        {
            get;
            set;
        }

        public IVBox()
        {
            AlignSpace = 15;
            AlignSize = 60;
            Alignment = FillAlignment.Fill;
            AlignLength = 45;

            OnControlAdded = (root, control) =>
            {

                switch (Alignment)
                {
                   
                    case FillAlignment.Bottom:

                        {
                            int th = (int)Size.Y;
                            th = th / Controls.Count;
                            th = AlignSize;
                            int iy = (int)Size.Y - th;

                            foreach (var cont in Controls)
                            {
                                cont.Set(new OpenTK.Mathematics.Vector2(0, iy), new OpenTK.Mathematics.Vector2(Size.X, th), cont.Text);
                                iy = iy - (th+AlignSpace);
                            }
                            int b = 5;
                        }

                        break;
                    case FillAlignment.Top:

                        {
                            int th = (int)Size.Y;
                            th = AlignSize;
                            int iy = 0;

                            foreach (var cont in Controls)
                            {
                                cont.Set(new OpenTK.Mathematics.Vector2(0, iy), new OpenTK.Mathematics.Vector2(Size.X, th), cont.Text);
                                iy = iy + th + AlignSpace;
                            }
                            int b = 5;
                        }
                        break;
                    case FillAlignment.Horizontal:
                        {
                            int th = AlignSize;
                            int iy = 0;
                            int cw = (int)Size.X;


                            foreach (var cont in Controls)
                            {
                                cont.Set(new OpenTK.Mathematics.Vector2(0, iy), new OpenTK.Mathematics.Vector2(cw, th), cont.Text);
                                iy = iy + th + AlignSpace;
                            }
                        }
                        break;
                    case FillAlignment.Right:

                        {
                            int th = AlignSize;
                            int iy = 0;
                            int cw = (int)Size.X;


                            int by = 0;
                            if (Controls.Count == 1)
                            {
                                by = 0;
                            }
                            else
                            {
                                by = (int)Size.Y / (Controls.Count - 1);
                            }
                            //by = by - 32;


                            //th = by - AlignSize / Controls.Count;
                            int mh = (int)Size.Y / Controls.Count;
                            int ch = mh - AlignSpace;

                            int ly = AlignSpace / Controls.Count;

                            mh = mh + ly;

                            foreach (var cont in Controls)
                            {
                                cont.Set(new OpenTK.Mathematics.Vector2(Size.X-AlignSize, iy), new OpenTK.Mathematics.Vector2(AlignSize, ch), cont.Text);
                                iy = iy + mh; ;
                            }
                        }
                        break;

                    case FillAlignment.Left:
      
                        {
                            int th = AlignSize;
                            int iy = 0;
                            int cw = (int)Size.X;
                            

                            int by = 0;
                            if (Controls.Count == 1)
                            {
                                by = 0;
                            }
                            else
                            {
                                by = (int)Size.Y / (Controls.Count - 1);
                            }
                            //by = by - 32;


                            //th = by - AlignSize / Controls.Count;
                            int mh = (int)Size.Y / Controls.Count;

                            //mh = mh - AlignSpace;
                            int ch = mh - AlignSpace;

                            int ly = AlignSpace / Controls.Count;

                            mh = mh + ly;


                            foreach (var cont in Controls)
                            {
                                cont.Set(new OpenTK.Mathematics.Vector2(0, iy), new OpenTK.Mathematics.Vector2(AlignSize, ch), cont.Text);
                                iy = iy + mh; ;
                            }
                        }
                        break;

                        break;
                    case FillAlignment.Fill:
                        {
                            int th = (int)Size.Y;
                            th = (th+AlignSpace) / Controls.Count;

                            
                            int iy = 0;

                            foreach (var cont in Controls)
                            {
                                cont.Set(new OpenTK.Mathematics.Vector2(0, iy), new OpenTK.Mathematics.Vector2(Size.X, th-AlignSpace), cont.Text);
                                iy = iy + th;
                            }
                            int b = 5;
                        }
                        break;
                }

            };

        }

        

        public override void Render()
        {
            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.ButtonSlice,8,8, RenderPosition, Size, Color);

            RenderChildren();
        }

    }
}
