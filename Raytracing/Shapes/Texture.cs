using Raytracing.Helpers;
using System;
using System.Drawing;
using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Handles Textures that can be projected onto <see cref="ISceneObject"/>s like <see cref="Sphere"/>s.
    /// </summary>
    public class Texture {
        private readonly Bitmap bitmap;
        private readonly object bitmapLock = new object();

        /// <summary>
        /// The gamma value that is used when converting the image from SRGB to linear colour space.
        /// </summary>
        public readonly float Gamma;

        /// <summary>
        /// The amount by which the texture is rotated horizontally, in radians.
        /// </summary>
        public readonly float RotationOffset;

        /// <summary>
        /// The textures width, in pixels.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// The textures height, in pixels.
        /// </summary>
        public readonly int Height;

        /// <summary>
        /// Creates a new <see cref="Texture"/> with the image at the given path. Supports BMP, GIF, EXIF, JPG, PNG and TIFF image files.
        /// </summary>
        /// <param name="path">Path to an image file. Supports BMP, GIF, EXIF, JPG, PNG and TIFF.</param>
        /// <param name="gamma">Gamma value that is used to transform the image from SRGB to linear colour space.</param>
        /// <param name="rotationOffset">How much the texture is rotated horizontally before being projected onto the object, in radians.</param>
        public Texture(string path, float gamma = 2.2f, float rotationOffset = 0) {
            bitmap = new Bitmap(path);
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            this.Gamma = gamma;
            this.RotationOffset = rotationOffset;
        }

        /// <summary>
        /// Returns a specific pixel on the texture. Values outside the range [0,1] will be wrapped.
        /// </summary>
        /// <param name="s">Horizontal position of the desired pixel</param>
        /// <param name="t">Vertical position of the desired pixel</param>
        /// <returns>A specific pixel on the texture</returns>
        public Vector3 GetPixel(float s, float t) {
            int x = (int)(((s + RotationOffset / (2 * Math.PI)) % 1.0) * (Width - 1));
            int y = (int)((1 - (t % 1.0)) * (Height - 1));
            Color c;
            lock(bitmapLock) {
                c = bitmap.GetPixel(x, y);
            }
            return new Vector3(c.R, c.G, c.B).FromSRGB(Gamma);
        }
    }
}
