using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Represents a ray. A ray, or a half-line, is a line with an initial point.
    /// Has an origin and a direction.
    /// </summary>
    public class Ray {

        /// <summary>
        /// The ray's origin.
        /// </summary>
        public Vector3 Origin { get; }

        /// <summary>
        /// The ray's direction, a normalised vector.
        /// </summary>
        public Vector3 Direction { get; }

        /// <summary>
        /// Creates a new ray with given origin and direction.
        /// </summary>
        /// <param name="origin">The ray's origin.</param>
        /// <param name="direction">The ray's direction. Will be normalised by the constructor.</param>
        public Ray(Vector3 origin, Vector3 direction) {
            this.Origin = origin;
            this.Direction = Vector3.Normalize(direction);
        }
    }
}
