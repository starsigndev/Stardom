using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StardomEngine.Resonance.Controls
{
    public class IHBox : IControl
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

        public void Calculate()
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
                            iy = iy - (th + AlignSpace);
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
                        int tw = AlignSize;
                        int ix = 0;
                        int ch = (int)Size.Y;


                        int bx = 0;
                        if (Controls.Count == 1)
                        {
                            bx = 0;
                        }
                        else
                        {
                            bx = (int)Size.X / (Controls.Count - 1);
                        }
                        //by = by - 32;


                        //th = by - AlignSize / Controls.Count;
                        int mw = (int)Size.X / Controls.Count;
                        int cw = mw - AlignSpace;

                        int lx = AlignSpace / Controls.Count;

                        mw = mw + lx;
                        int al = AlignSize;

                        foreach (var cont in Controls)
                        {
                            if (cont.AlignSize != -1)
                            {
                                al = cont.AlignSize;
                            }
                            else
                            {
                                al = AlignSize;
                            }
                            cont.Set(new OpenTK.Mathematics.Vector2(Size.X-ix-al,Size.Y - AlignSize), new OpenTK.Mathematics.Vector2(al,AlignSize), cont.Text);
                            ix = ix + al ;
                          //  ix = ix + al + AlignSpace; ;

                        }
                    }
                    break;

                case FillAlignment.Left:

                    {
                        int tw = AlignSize;
                        int ix = 0;
                        int ch = (int)Size.Y;


                        int bx = 0;
                        if (Controls.Count == 1)
                        {
                            bx = 0;
                        }
                        else
                        {
                            bx = (int)Size.X / (Controls.Count - 1);
                        }
                        //by = by - 32;


                        //th = by - AlignSize / Controls.Count;
                        int mw = (int)Size.X / Controls.Count;

                        //mh = mh - AlignSpace;
                        int cw = mw - AlignSpace;

                        int lx = AlignSpace / Controls.Count;

                        mw = mw + lx;

                        int al = AlignSize;



                        foreach (var cont in Controls)
                        {
                            if (cont.AlignSize != -1)
                            {
                                al = cont.AlignSize;
                            }
                            else
                            {
                                al = AlignSize;
                            }
                            cont.Set(new OpenTK.Mathematics.Vector2(ix, 0), new OpenTK.Mathematics.Vector2(al, Size.Y), cont.Text);
                            ix = ix + al + AlignSpace; ;
                        }
                    }
                    break;

                    break;
                case FillAlignment.Fill:
                    {
                        int tw = (int)Size.X;
                        tw = (tw + AlignSpace) / Controls.Count;




                        int ix = 0;

                        int mx = tw;

                        int cc = 0;
                        foreach (var cont in Controls)
                        {
                            cc++;
                            if (cont.AlignSize != -1)
                            {
                                mx = cont.AlignSize;
                            }
                            else
                            {
                                mx = tw;
                            }

                            cont.Set(new OpenTK.Mathematics.Vector2(ix, 0), new OpenTK.Mathematics.Vector2(mx - AlignSpace, Size.Y), cont.Text);
                            ix = ix + mx;
                            if (cont.AlignSize != -1)
                            {

                                tw = tw - ((cont.AlignSize-tw) / (Controls.Count - (cc)));//
                              
                            }

                        }
                        int b = 5;
                    }
                    break;
            }

        }

        public IHBox()
        {
            AlignSpace = 15;
            AlignSize = 60;
            Alignment = FillAlignment.Fill;
            AlignLength = 45;

        }

        public override void Render()
        {
            //base.Render();
         //   GameUI.This.DrawRect(GameUI.Theme.ButtonSlice, 8, 8, RenderPosition, Size, Color);

            RenderChildren();
        }


    }
}
