using System;
using System.Numerics;

namespace Raytracing {
    public class Sphere : ISceneObject {
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
            float c = (float)(Math.Pow(CE.Length(), 2) - Math.Pow(R, 2));
            double discriminant = b * b - 4 * a * c;
            if(discriminant < 0) {
                return null;
            } else {
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
                    Vector3 hitPointPosition = ray.Origin + lambda * ray.Direction;
                    Vector3 normal = Vector3.Normalize(hitPointPosition - Centre);
                    return new HitPoint(lambda, hitPointPosition, normal, this);
                }
            }
        }
    }
}
