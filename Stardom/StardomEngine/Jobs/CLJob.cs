using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloo;
using OpenTK.Compute.OpenCL;

namespace StardomEngine.Jobs
{
    public class CLJob
    {
        protected static ComputePlatform platform = null;
        public static ComputeContextPropertyList properties = null;
        public static ComputeContext context = null;
        public static ComputeCommandQueue queue = null;
        public ComputeProgram Program;


        private static bool CL_Init = false;
        public CLJob(string file)
        {

            if (!CL_Init)
            {
                InitCL();
            }

            SetProgram(file);


        }
        public virtual void Init()
        {



        }
        public virtual void Bind()
        {

        }

        public virtual void Release()
        {

        }

        public virtual void Execute()
        {

        }

        public void SetProgram(string path)
        {

            Program = new ComputeProgram(context, File.ReadAllText(path));
            try
            {
                // Attempt to build the OpenCL program
                Program.Build(null, null, null, IntPtr.Zero);
            }
            catch (BuildProgramFailureComputeException ex)
            {
                // Get the build log for each device
                foreach (ComputeDevice device in context.Devices)
                {
                    string buildLog = Program.GetBuildLog(device);
                    Console.WriteLine($"Build Log for device {device.Name}:");
                    Console.WriteLine(buildLog);
                }
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                Console.WriteLine("An error occurred while building the OpenCL program:");
                Console.WriteLine(ex.Message);
            }
        }

        public ComputeKernel GetKernel(string name)
        {

            return Program.CreateKernel(name);

        }

        public static void InitCL()
        {
            foreach (var platform in ComputePlatform.Platforms)
            {
                Console.WriteLine($"Platform: {platform.Name}");
                Console.WriteLine($"Vendor: {platform.Vendor}");
                Console.WriteLine($"Version: {platform.Version}");
                Console.WriteLine($"Profile: {platform.Profile}");
                Console.WriteLine($"Extensions: {platform.Extensions}");

                // Get available devices for each platform
                foreach (var device in platform.Devices)
                {
                    Console.WriteLine($"\nDevice: {device.Name}");
                    Console.WriteLine($"Type: {device.Type}");
                    Console.WriteLine($"Vendor: {device.Vendor}");
                    Console.WriteLine($"Version: {device.Version}");
                    Console.WriteLine($"Driver Version: {device.DriverVersion}");
                    Console.WriteLine($"Profile: {device.Profile}");
                    Console.WriteLine($"Max Compute Units: {device.MaxComputeUnits}");
                    Console.WriteLine($"Max Clock Frequency: {device.MaxClockFrequency} MHz");
                    Console.WriteLine($"Global Memory Size: {device.GlobalMemorySize / (1024 * 1024)} MB");
                    Console.WriteLine($"Local Memory Size: {device.LocalMemorySize / 1024} KB");
                    Console.WriteLine($"Max Work Group Size: {device.MaxWorkGroupSize}");
                    Console.WriteLine($"Extensions: {device.Extensions}");
                }
            }
            platform = ComputePlatform.Platforms[0];
            Console.WriteLine($"Using Platform: {platform.Name}");
            ComputeDevice device1 = platform.Devices[0]; // Selecting the first device
            Console.WriteLine($"\nUsing Device: {device1.Name}");

            // Create a context for the selected device
            context = new ComputeContext(new[] { device1 }, new ComputeContextPropertyList(platform), null, IntPtr.Zero);

            queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);
            int b = 5;
            PrintDeviceInfo(device1);
        }
        static void PrintDeviceInfo(ComputeDevice device)
        {
            Console.WriteLine($"Device Name: {device.Name}");
            Console.WriteLine($"Device Type: {device.Type}");
            Console.WriteLine($"Vendor: {device.Vendor}");
            Console.WriteLine($"Version: {device.Version}");
            Console.WriteLine($"Driver Version: {device.DriverVersion}");
            Console.WriteLine($"Profile: {device.Profile}");
            Console.WriteLine($"Max Compute Units: {device.MaxComputeUnits}");
            Console.WriteLine($"Max Clock Frequency: {device.MaxClockFrequency} MHz");
            Console.WriteLine($"Global Memory Size: {device.GlobalMemorySize / (1024 * 1024)} MB");
            Console.WriteLine($"Local Memory Size: {device.LocalMemorySize / 1024} KB");
            Console.WriteLine($"Max Work Group Size: {device.MaxWorkGroupSize}");
            Console.WriteLine($"Extensions: {device.Extensions}");
        }
    }
}
