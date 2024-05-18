using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;
using StardomEngine.App;
using StardomEngine.Texture;

namespace EngineTest
{
    public class EngineTest1 : StarApp
    {

        public EngineTest1(GameWindowSettings settings,NativeWindowSettings native) : base(settings,native) { }

        Texture2D Tex1;

        public override void InitApp()
        {
            //base.InitApp();
            Tex1 = new Texture2D("Data/test1.png");


        }

    }

   

}
