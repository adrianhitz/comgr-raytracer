using System;
using System.Numerics;

namespace Raytracing {
    public class Sphere : ISceneObject {
        public Vector3 Centre { get; }
        public float R { get; }
        public Colour Colour { get; }
        public Colour Specular { get; }
        public Colour Reflective { get; }
        public Colour Emissive { get; }

        public Sphere(Vector3 centre, float r, Colour colour, Colour specular, Colour reflective, Colour emissive) {
            this.Centre = centre;
            this.R = r;
            this.Colour = colour;
            this.Specular = specular;
            this.Reflective = reflective;
            this.Emissive = emissive;
        }

        public Sphere(Vector3 centre, float r, Colour colour) : this(centre, r, colour, new Colour(0, 0, 0), new Colour(0, 0, 0), new Colour(0, 0, 0)) { }
        public Sphere(Vector3 centre, float r, Colour colour, Colour specular, Colour reflective) : this(centre, r, colour, specular, reflective, new Colour(0, 0, 0)) { }

        public HitPoint CalculateHitPoint(Ray ray) {
            Vector3 CE = ray.Origin - Centre;
            float a = 1;
            float b = 2 * Vector3.Dot(CE, ray.Direction);
            float c = (float)(Math.Pow(CE.Length(), 2) - Math.Pow(R, 2));
            double discriminant = b * b - 4 * a * c;
            if(discriminant >= 0) {
                float solution1 = (float)((-b + Math.Sqrt(discriminant)) / (2 * a));
                float solution2 = (float)((-b - Math.Sqrt(discriminant)) / (2 * a));
                if(solution1 > 0 || solution2 > 0) {
                    float lambda = solution1 >= 0 && solution2 >= 0 ? Math.Min(solution1, solution2) : Math.Max(solution1, solution2);
                    Vector3 hitPointPosition = ray.Origin + lambda * ray.Direction;
                    Vector3 normal = Vector3.Normalize(hitPointPosition - Centre);
                    return new HitPoint(lambda, hitPointPosition, normal, this);
                }
            }
            return null;
        }
    }
}
