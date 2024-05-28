using StardomEditor.Windows;
using StardomEngine.App;
using StardomEngine.Resonance.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardomEditor.States
{
    public class MainEditorState : AppState
    {
        public static ILabel ModInfo;

        public override void InitState()
        {
            base.InitState();

            var project = UI.MainMenu.AddItem("Project");

            var new_proj = project.AddItem("New Module");
            project.AddSeperator();
            var load_proj = project.AddItem("Load Module");
            var save_proj = project.AddItem("Save Module");


            var edit = UI.MainMenu.AddItem("Edit");

            var tools = UI.MainMenu.AddItem("Tools");

            var gen_uni = tools.AddItem("Generate Universe");

            gen_uni.OnItemSelected = (item) =>
            {
                if (ProjectState.EditModule != null)
                {
                    StarApp.PushState(new GenerateUniverseState());
                };
            };


            //Project Events
            new_proj.OnItemSelected = (item) =>
            {
                //Environment.Exit(0);
                GNewModule new_mod = new GNewModule();
                new_mod.Text = "New Module";
                new_mod.Size = new OpenTK.Mathematics.Vector2(420, 450);
                new_mod.Center();

                new_mod.Text = "New Module";
                UI.AddWindow(new_mod);

            };

            var bg = new IImage(new StardomEngine.Texture.Texture2D("edit/mainbg1.jpg")).Set(new(0, 0), new(StarApp.FrameWidth, StarApp.FrameHeight - 30), "");

            UI.RootControl.AddControl(bg);
            ModInfo = new ILabel("Mod:").SetPosition(new(130, 30)) as ILabel;
            ModInfo.Scale = 3;
            var info_panel = new IPanel().Set(new OpenTK.Mathematics.Vector2(50, 100), new OpenTK.Mathematics.Vector2(StarApp.FrameWidth - 100, StarApp.FrameHeight - 170), "");
            UI.RootControl.AddControls(ModInfo, info_panel);

            ProjectState.LoadIf();
            if (ProjectState.EditModule != null)
            {
                ModInfo.Text = "Mod:" + ProjectState.EditModule.ModName;
            }
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
