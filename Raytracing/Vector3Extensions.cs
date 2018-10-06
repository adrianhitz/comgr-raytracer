using System;
using System.Numerics;

namespace Raytracing {

    /// <summary>
    ///     Contains extension methods for <see cref="System.Numerics.Vector3" />.
    /// </summary>
    public static class Vector3Extensions {

        /// <summary>
        /// Converts this vector from linear to SRGB colour space using a given <paramref name="gamma"/>. Component values are clamped to the range [0,1].
        /// </summary>
        /// <param name="colour">The vector to be converted, with colour values inside the range [0,1].</param>
        /// <param name="gamma">Gamma value used for conversion.</param>
        /// <returns>Vector in SRGB colour space with component values in the range [0,255].</returns>
        public static Vector3 ToSRGB(this Vector3 colour, float gamma) {
            float f(float component) => (float)Math.Pow(Math.Max(Math.Min(component, 1), 0), 1 / gamma);
            return 255 * new Vector3(f(colour.X), f(colour.Y), f(colour.Z));
        }

        /// <summary>
        /// Converts this vector from SRGB to linear colour space using a given <paramref name="gamma"/>. Component values are clamped to the range [0,255].
        /// </summary>
        /// <param name="colour">The vector to be converted, with colour values inside the range [0,255].</param>
        /// <param name="gamma">Gamma value used for conversion.</param>
        /// <returns>Vector in linear colour space with component values in the range [0,1].</returns>
        public static Vector3 FromSRGB(this Vector3 colour, float gamma) {
            float f(float component) => (float)Math.Pow(Math.Max(Math.Min(component, 255), 0) / 255.0, gamma);
            return new Vector3(f(colour.X), f(colour.Y), f(colour.Z));
        }
    }
}

