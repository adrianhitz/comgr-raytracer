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
                if(solution1 < 0 && solution2 < 0) {
                    return null;
                } else {
                    float lambda;
                    if(solution1 >= 0 && solution2 >= 0) {
                        lambda = Math.Min(solution1, solution2);
                    } else {
                        lambda = Math.Max(solution1, solution2);
                    }
                    Vector3 location = ray.Origin + lambda * ray.Direction;
                    Vector3 normal = location - Centre;
                    return new HitPoint(lambda, location, normal, this);
                }
            }
        }

        private Vector3 CalculateNormal(Vector3 surfacePoint) {
            return Vector3.Normalize(surfacePoint - Centre);
        }
    }
}
