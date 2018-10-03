using System;
using System.Numerics;

namespace Raytracing {
    public class Sphere : ISceneObject {
        public Vector3 Centre { get; }
        public float R { get; }
        public Vector3 Diffuse { get; }
        public Vector3 Specular { get; }
        public Vector3 Reflective { get; }
        public Vector3 Emissive { get; }

        public Sphere(Vector3 centre, float r, Vector3 colour, Vector3 specular, Vector3 reflective, Vector3 emissive) {
            this.Centre = centre;
            this.R = r;
            this.Diffuse = colour;
            this.Specular = specular;
            this.Reflective = reflective;
            this.Emissive = emissive;
        }

        public Sphere(Vector3 centre, float r, Vector3 colour) : this(centre, r, colour, Colour.Black, Colour.Black, Colour.Black) { }
        public Sphere(Vector3 centre, float r, Vector3 colour, Vector3 specular, Vector3 reflective) : this(centre, r, colour, specular, reflective, Raytracing.Colour.Black) { }

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
