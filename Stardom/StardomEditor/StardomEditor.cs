using StardomEngine.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using StardomEngine.Resonance;
using StardomEditor.States;

namespace StardomEditor
{
    public class StardomEditor : StarApp
    {

        


        public StardomEditor(GameWindowSettings settings,NativeWindowSettings native) : base(settings, native)
        {

        }

        public override void InitApp()
        {
            base.InitApp();
            PushState(new MainEditorState());
        }

        public override void UpdateApp()
        {
            base.UpdateApp();
        }

        public override void RenderApp()
        {
            base.RenderApp();
        }

    }

}
