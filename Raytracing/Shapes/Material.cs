using Raytracing.Helpers;
using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Represents a surface material for <see cref="ISceneObject" />s. Defines how the surface interacts with light (e.g. reflections).
    /// </summary>
    public class Material {

        /// <summary>
        /// Diffuse reflection component for this material. A vector with components in the range [0,1].
        /// </summary>
        public Vector3 Diffuse { get; }

        /// <summary>
        /// Specular reflection component for this material. A vector with components in the range [0,1].
        /// </summary>
        public Vector3 Specular { get; }

        /// <summary>
        /// Regular reflection component for this material. A vector with components in the range [0,1].
        /// </summary>
        public Vector3 Reflective { get; }

        /// <summary>
        /// Emissive component for this material (how much the material glows). A vector with components in the range [0,1].
        /// </summary>
        public Vector3 Emissive { get; }

        /// <summary>
        /// This material's texture. Null if there is none.
        /// </summary>
        public Texture Texture { get; }

        /// <summary>
        /// Creates a new material with the specified properties. Components outside the range [0,1] will be clamped.
        /// </summary>
        /// <param name="diffuse">Diffuse reflection component</param>
        /// <param name="specular">Specular reflection component</param>
        /// <param name="reflective">Regular reflection component</param>
        /// <param name="emissive">Emissive component</param>
        /// <param name="texture">Texture</param>
        public Material(Vector3 diffuse, Vector3 specular, Vector3 reflective, Vector3 emissive, Texture texture = null) {
            this.Diffuse = diffuse.Clamp();
            this.Specular = specular.Clamp();
            this.Reflective = reflective.Clamp();
            this.Emissive = emissive.Clamp();
            this.Texture = texture;
        }

        /// <summary>
        /// Creates a new component with the specified properties and the default emission value (0, 0, 0).
        /// </summary>
        /// <param name="diffuse">Diffuse reflection component</param>
        /// <param name="specular">Specular reflection component</param>
        /// <param name="reflective">Regular reflection component</param>
        /// <param name="texture">Texture</param>
        public Material(Vector3 diffuse, Vector3 specular, Vector3 reflective, Texture texture = null)
            : this(diffuse, specular, reflective, Colour.Black, texture) { }
    }
}
