using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StardomEngine.Helper
{
    public class GameMaths
    {

        private static readonly Random random = new Random();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomVec3(Vector3 min,Vector3 max)
        {
            float x = (float)(random.NextDouble() * (max.X - min.X) + min.X);
            float y = (float)(random.NextDouble() * (max.Y - min.Y) + min.Y);
            float z = (float)(random.NextDouble() * (max.Z - min.Z) + min.Z);

            return new Vector3(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RotateAndScale(Vector2 vector, float rotate, float scale)
        {
            // Convert the rotation angle from degrees to radians
            float radians = MathHelper.DegreesToRadians(rotate);

            // Calculate the sine and cosine of the rotation angle
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            // Apply the rotation
            float xNew = vector.X * cos - vector.Y * sin;
            float yNew = vector.X * sin + vector.Y * cos;

            // Apply the scaling
            xNew *= scale;
            yNew *= scale;

            // Return the transformed vector
            return new Vector2(xNew, yNew);
        }

    }
}
