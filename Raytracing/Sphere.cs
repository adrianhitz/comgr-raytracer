using System;
using System.Numerics;

namespace Raytracing {

    /// <summary>
    /// Represents an drawable sphere in a <see cref="Scene"/>.
    /// </summary>
    public class Sphere : ISceneObject {

        /// <summary>
        /// The position (centre) of the sphere in three-dimensional space.
        /// </summary>
        public Vector3 Position { get; }
        public float R { get; }
        public Material Material { get; }
        public Texture Texture { get; }

        /// <summary>
        /// Creates a new sphere
        /// </summary>
        /// <param name="centre">The sphere's centre in thee-dimensional space</param>
        /// <param name="r">The sphere's radius</param>
        /// <param name="material">The sphere's material</param>
        /// <param name="texture">The texture that should be projected onto the sphere, if any</param>
        public Sphere(Vector3 centre, float r, Material material, Texture texture = null) {
            this.Position = centre;
            this.R = r;
            this.Material = material;
            this.Texture = texture;
        }

        /// <summary>
        /// Calculates the closest <see cref="HitPoint"/> of a <see cref="Ray"/> if the <see cref="Ray"/> intersects it. Null otherwise.
        /// Returns a <see cref="HitPoint"/> containing information, most importantly the position in three-dimensional space where the hit occured.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> used to calculate the <see cref="HitPoint"/></param>
        /// <returns>A <see cref="HitPoint"/> containing information, most importantly the position in three-dimensional space where the hit occured.</returns>
        public HitPoint CalculateHitPoint(Ray ray) {
            Vector3 CE = ray.Origin - Position;
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
                    Vector3 normal = Vector3.Normalize(hitPointPosition - Position);
                    return new HitPoint(lambda, hitPointPosition, normal, this);
                }
            }
            return null;
        }
    }
}
