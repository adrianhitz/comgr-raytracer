using System;
using System.Numerics;

namespace Raytracing {
    public static class Vector3Extensions {
        public static Vector3 ToSRGB(this Vector3 vector3, float lambda) {
            float f(float component) => (float)Math.Pow(Math.Max(Math.Min(component, 1), 0), 1 / lambda);
            return 255 * new Vector3(f(vector3.X), f(vector3.Y), f(vector3.Z));
        }
    }
}
