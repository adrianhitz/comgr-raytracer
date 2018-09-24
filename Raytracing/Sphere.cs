using System;
using System.Numerics;

namespace Raytracing {
    class Sphere : ISceneObject {
        public Vector3 Centre { get; }
        public float R { get; }
        public Colour Colour { get; }

        public Sphere(Vector3 centre, float r, Colour colour) {
            this.Centre = centre;
            this.R = r;
            this.Colour = colour;
        }

        public HitPoint? CalculateHitPoint(Ray ray) {
            Vector3 CE = ray.Origin - Centre;
            float a = 1;
            float b = 2 * Vector3.Dot(CE, ray.Direction);
            float c = (float)Math.Pow(CE.Length(), 2);
            if(Math.Pow(b, 2) < 4 * a * c) {
                return null;
            } else {
                double discriminant = Math.Pow(b, 2) - 4 * a * c;
                float solution1 = (float)((-b + Math.Sqrt(discriminant)) / (2 * a));
                float solution2 = (float)((-b - Math.Sqrt(discriminant)) / (2 * a));
                if(solution1 >= 0 && solution2 >= 0) {
                    return new HitPoint(Math.Min(solution1, solution2), this);
                } else if(solution1 < 0 && solution2 < 0) {
                    return null;
                } else {
                    return new HitPoint(Math.Max(solution1, solution2), this);
                }
            }
        }
    }
}
}
