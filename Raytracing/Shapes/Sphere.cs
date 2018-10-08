using System;
using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Represents an drawable sphere in a <see cref="Scene"/>.
    /// </summary>
    public class Sphere : ISceneObject {

        public Vector3 Position { get; protected set; }
        public float R { get; protected set; }
        public Material Material { get; }

        /// <summary>
        /// Creates a new sphere
        /// </summary>
        /// <param name="centre">The sphere's centre in thee-dimensional space</param>
        /// <param name="r">The sphere's radius</param>
        /// <param name="material">The sphere's material</param>
        /// <param name="texture">The texture that should be projected onto the sphere, if any</param>
        public Sphere(Vector3 position, float r, Material material = null) {
            this.Position = position;
            this.R = r;
            this.Material = material;
        }

        /// <summary>
        /// Creates a sphere without parameters. Mostly used to be able to calculate position and radius of spheres in subclasses (like BVHNode)
        /// before assigning them. Yes it's ugly. I'm sorry.
        /// </summary>
        protected Sphere() { }

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

        public BoundingSphere GetBoundingSphere() {
            throw new NotImplementedException(); // TODO implement Sphere.GetBoundingSphere
        }
    }
}
