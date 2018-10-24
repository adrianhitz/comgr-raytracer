using Raytracing.Shapes;
using System.Numerics;

namespace Raytracing.Acceleration.BVH {

    /// <summary>
    /// Represents a node in a bounding volume hierarchy.
    /// </summary>
    internal class BVHNode : Sphere {

        /// <summary>
        /// A child object of this node.
        /// </summary>
        public Sphere Left { get; }

        /// <summary>
        /// A child object of this node.
        /// </summary>
        public Sphere Right { get; }

        /// <summary>
        /// Creates a new BVH node from two spheres. Calculates the minimum sphere that encompasses these two spheres.s
        /// </summary>
        /// <param name="left">Child object for this node</param>
        /// <param name="right">Child object for this node</param>
        public BVHNode(Sphere left, Sphere right) : base() {
            Left = left;
            Right = right;
            R = ((Right.Position - Left.Position).Length() + Left.R + Right.R) / 2;
            Position = Left.Position + Vector3.Normalize(Right.Position - Left.Position) * (R - Left.R);
        }
    }
}
