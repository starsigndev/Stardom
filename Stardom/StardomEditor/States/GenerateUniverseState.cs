using StardomEditor.Windows.GenerateUniverse;
using StardomEngine.App;
using StardomEngine.Resonance;
using StardomEngine.Resonance.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEditor.States
{
    public class GenerateUniverseState : AppState
    {

        public override void InitState()
        {
            base.InitState();


           


            var uni = UI.MainMenu.AddItem("Universe");
            var ex = uni.AddItem("Exit");

            ex.OnItemSelected = (item) =>
            {
                StarApp.PopState();
            };

            var main = new IWindow(false, true).Set(new(0, 0), new(StarApp.FrameWidth, StarApp.FrameHeight - 30), "Generate Universe") as IWindow;

            UI.RootControl.AddControl(main);

            var build = main.Contents.GetBuilder();// new UIBuilder(main.Contents);

            var panel = build.Panel(new(50, 50), new(300, 250));


            var pan_b = panel.GetBuilder();


            //var uni_pan = new IPanel().Set(new(50, 50), new(300, 250), "");

            //UI.RootControl.AddControl(uni_pan);

            var gen_random = pan_b.Button(new(10, 10), new(180, 30), "Generate Random");
                      

            gen_random.OnClick += (s, e) =>
            {
                GRandomUniverse rand_win = new();
                rand_win.Text = "Generate Galaxies";
                rand_win.Size = new OpenTK.Mathematics.Vector2(470, 450);
                rand_win.Center();
                //rand_win.Text = "New Module";

                UI.AddWindow(rand_win);
            };

        }

        public override void UpdateState()
        {
            base.UpdateState();
            UI.Update();
        }

        public override void RenderState()
        {
            base.RenderState();
            UI.Render();
        }

    }
}
