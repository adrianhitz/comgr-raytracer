using System;
using System.Numerics;

namespace Raytracing {
    public static class Vector3Extensions {
        public static Vector3 ToSRGB(this Vector3 vector3, float gamma) {
            float f(float component) => (float)Math.Pow(Math.Max(Math.Min(component, 1), 0), 1 / gamma);
            return 255 * new Vector3(f(vector3.X), f(vector3.Y), f(vector3.Z));
        }

        public static Vector3 FromSRGB(this Vector3 vector3, float gamma) {
            float f(float component) => (float)Math.Pow(Math.Max(Math.Min(component, 255), 0) / 255.0, gamma);
            return new Vector3(f(vector3.X), f(vector3.Y), f(vector3.Z));
        }
    }
}
