using StardomEngine.Resonance;
using StardomEngine.Resonance.Controls;
using StardomEngine.Universe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEditor.Windows.GenerateUniverse
{
    public class GRandomUniverse : IWindow
    {

        IVBox vb;
        IHBox galax_h;
        IHBox planets_h;

        public GRandomUniverse() : base(false, false)
        {

            

            return;
            vb = new IVBox();
            galax_h = new IHBox();
            planets_h = new IHBox();
            Contents.AddControl(vb);
            vb.AddControls(galax_h,planets_h);

            vb.Alignment = FillAlignment.Top;
            vb.AlignSize = 30;
            vb.AlignSpace = 10;

            ILabel gal_lab = new("Galaxies");
            ILabel gal_min = new("Min");
            ITextBox min_box = new();
            min_box.Numeric = true;
            ILabel gal_max = new("Max");
            ITextBox max_box = new();
            max_box.Numeric = true;
            galax_h.AddControls(gal_lab, gal_min, min_box, gal_max, max_box);
            gal_lab.AlignSize = 120;
            gal_min.AlignSize = 50;
            gal_max.AlignSize = 50;
            min_box.AlignSize = 120;
            max_box.AlignSize = 120;

            ILabel plan_lab = new("Planets");
            ILabel pmin_lab = new("Min");
            ITextBox pmin_box = new();
            ILabel pmax_lab = new("Max");
            ITextBox pmax_box = new();

            plan_lab.AlignSize = 120;
            pmin_lab.AlignSize = 50;
            pmax_lab.AlignSize = 50;
            pmin_box.AlignSize = 120;
            pmax_box.AlignSize = 120;



            planets_h.AddControls(plan_lab, pmin_lab, pmin_box, pmax_lab, pmax_box);
            pmin_box.Text = "1";
            pmax_box.Text = "7";
            min_box.Text = "1";
            max_box.Text = "700";



        }

        public override void Build()
        {
            //base.Build();
            Builder.BeginVertical(FillAlignment.Top,10, 35);
            Builder.BeginHorizontal(FillAlignment.Fill,10);
            Builder.Label("Planets",120);
            var i1 = Builder.TextBox("Min",120,50,"1");
            var i2 = Builder.TextBox("Max",120,50,"7");
            Builder.End();
            Builder.BeginHorizontal(FillAlignment.Fill, 10);
            Builder.Label("Galaxies", 120);
            var i3 = Builder.TextBox("Min", 120, 50,"1");
            var i4 = Builder.TextBox("Max", 120, 50,"100");
            Builder.End();
            Builder.BeginHorizontal(FillAlignment.Fill,10);
            var create = Builder.Button("Create");
            Builder.Button("Cancel");
            Builder.End();
            Builder.End();
            i1.TabNext = i2;
            i2.TabNext = i3;
            i3.TabNext = i4;
            i4.TabNext = i1;
            create.OnClick = (b, mb) =>
            {
            
                var uni = UniGen.This.CreateUniverse(int.Parse(i3.Text), int.Parse(i4.Text),int.Parse(i1.Text),int.Parse(i2.Text));
            
            };
        }

        public override void AfterSet()
        {
            //base.AfterSet();
    
            base.AfterSet();
    //        vb.Set(new OpenTK.Mathematics.Vector2(10, 10), Contents.Size, Text);
     //       vb.Calculate();
      //      galax_h.Calculate();
       //     planets_h.Calculate();

           // name_h.Calculate();
           // author_h.Calculate();
           // starsys_h.Calculate();
           // accept_h.Calculate();

         //   Contents.Offset = new OpenTK.Mathematics.Vector2(6, 0);
        }

    }
}
