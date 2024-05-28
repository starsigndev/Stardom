using OpenTK.Windowing.Desktop;

namespace StardomEditor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stardom Editor. Pre-Alpha");

            GameWindowSettings settings = new GameWindowSettings();


            settings.UpdateFrequency = 0;

            NativeWindowSettings native = new NativeWindowSettings();

            native.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            native.APIVersion = new Version(4, 2);
            native.AutoLoadBindings = true;
            native.ClientSize = new OpenTK.Mathematics.Vector2i(1300, 768);
            native.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            native.Title = "Stardom Editor (c)Star Signal 2024";
            native.Vsync = OpenTK.Windowing.Common.VSyncMode.Off;
            native.IsEventDriven = false;


            var app = new StardomEditor(settings, native);


            app.Run();


        }
    }
}
