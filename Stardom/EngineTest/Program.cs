using OpenTK.Windowing.Desktop;

namespace EngineTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Stardom. Pre-Alpha.");

            GameWindowSettings settings = new GameWindowSettings();


            settings.UpdateFrequency = 0;

            NativeWindowSettings native = new NativeWindowSettings();

            native.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            native.APIVersion = new Version(4, 2);
            native.AutoLoadBindings = true;
            native.ClientSize = new OpenTK.Mathematics.Vector2i(1300, 768);
            native.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            native.Title = "StardomEngine Test 1.";
            native.Vsync = OpenTK.Windowing.Common.VSyncMode.Off;
            native.IsEventDriven = false;
           

            var app = new EngineTest1(settings, native);


            app.Run();




        }
    }
}
