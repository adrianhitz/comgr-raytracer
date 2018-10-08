using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Represents a point light source. Illuminates a <see cref="Scene"/>.
    /// </summary>
    public class LightSource {

        /// <summary>
        /// The light source's position
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// The light source's colour/intensity
        /// </summary>
        public Vector3 Colour { get; }

        /// <summary>
        /// Creates a new light source
        /// </summary>
        /// <param name="position">The light source's position</param>
        /// <param name="colour">The light source's colour/intensity</param>
        public LightSource(Vector3 position, Vector3 colour) {
            this.Position = position;
            this.Colour = colour;
        }
    }
}
