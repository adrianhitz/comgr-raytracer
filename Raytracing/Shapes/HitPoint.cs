using Raytracing.Helpers;
using System;
using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Reperesents a hit point, where a <see cref="Ray"/> intersected a <see cref="ISceneObject"/>.
    /// This unfortunately contains a bunch of different things and is generally a bit of a mess.
    /// </summary>
    public class HitPoint {

        /// <summary>
        /// The hit point's distance from the ray origin.
        /// </summary>
        public float Lambda { get; }

        /// <summary>
        /// The hit point's position in three-dimensional space.
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// The surface normal at the hit point's position.
        /// </summary>
        public Vector3 Normal { get; }

        /// <summary>
        /// The surface's material
        /// </summary>
        public Material Material { get; }

        /// <summary>
        /// Creates a new hit point
        /// </summary>
        /// <param name="lambda">The hit point's distance from the ray origin</param>
        /// <param name="position">The hit point's position in three-dimensional space</param>
        /// <param name="normal">The surface normal at the hit point's position</param>
        /// <param name="hitObject">The object that was hit</param>
        public HitPoint(float lambda, Vector3 position, Vector3 normal, ISceneObject hitObject) {
            this.Lambda = lambda;
            this.Position = position;
            this.Normal = normal;
            this.Material = hitObject.Material;
        }

        /// <summary>
        /// Calculates this point's diffuse reflection colour.
        /// </summary>
        /// <param name="lightSource">The light source illuminating the point</param>
        /// <returns>Diffuse reflection colour</returns>
        internal Vector3 Diffuse(LightSource lightSource) {
            Vector3 L = Vector3.Normalize(lightSource.Position - this.Position);
            float nL = Vector3.Dot(this.Normal, L);
            Vector3 diffuse = Material?.Texture != null ? GetTextureColour() : (Material != null ? Material.Diffuse : Colour.Black);
            return nL >= 0 ? Vector3.Multiply(lightSource.Colour, diffuse) * nL : Colour.Black;
        }

        /// <summary>
        /// Calculates this point's phong/specular reflection colour.
        /// </summary>
        /// <param name="lightSource">The light source illuminating the point</param>
        /// <param name="cameraPosition">The position of the camera</param>
        /// <param name="k">Phong reflection k value</param>
        /// <returns>Phong reflection colour</returns>
        internal Vector3 Specular(LightSource lightSource, Vector3 cameraPosition, int k = 40) {
            if(Material != null) {
                Vector3 L = Vector3.Normalize(lightSource.Position - this.Position);
                float nL = Vector3.Dot(this.Normal, L);
                Vector3 eh = Vector3.Normalize(this.Position - cameraPosition);
                Vector3 r = Vector3.Normalize(2 * nL * this.Normal - L);
                if(nL >= 0) return lightSource.Colour * (float)Math.Pow(Vector3.Dot(r, eh), k) * Material.Specular;
            }
            return Colour.Black;
        }

        /// <summary>
        /// Calculates the Fresnel value for a given ray
        /// </summary>
        /// <param name="ray">The ray</param>
        /// <returns>Fresnel colour value</returns>
        internal Vector3 Fresnel(Ray ray) {
            if(Material != null) {
                return Material.Specular + (Vector3.One - Material.Specular) * (float)Math.Pow(1 - Vector3.Dot(Normal, Vector3.Reflect(ray.Direction, Normal)), 5);
            }
            return Colour.Black;
        }

        /// <summary>
        /// Returns the colour of the texture projected onto the hit object at the hit point.
        /// </summary>
        /// <returns>The texture colour value</returns>
        private Vector3 GetTextureColour() {
            if(Material?.Texture != null) {
                float s = (float)Math.Atan2(this.Normal.X, this.Normal.Z) / (2 * (float)Math.PI) + 0.5f;
                float t = ((float)Math.Acos(this.Normal.Y)) / (float)Math.PI;
                return Material.Texture.GetPixel(s, t);
            }
            return Colour.Black;
        }
    }
}
