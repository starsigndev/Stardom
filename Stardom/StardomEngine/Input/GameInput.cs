using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Input
{
    public class GameInput
    {

        public static Vector2 MousePosition
        {
            get;
            set;
        }

        public static Vector2 MouseDelta
        {
            get;
            set;
        }

        public static float MouseWheel
        {
            get;
            set;
        }

        public static bool[] MouseButton
        {
            get;
            set;
        }

        public static bool[] KeyButton
        {
            get;
            set;
        }

        public static void InitInput()
        {

            MousePosition = new Vector2(0, 0);
            MouseDelta = new Vector2(0, 0);
            MouseButton = new bool[16];
            KeyButton = new bool[512];
            MouseWheel = 0.0f;


        }


    }
}
