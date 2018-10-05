using System;
using System.Drawing;
using System.Numerics;

namespace Raytracing {
    public class Texture {
        private readonly Bitmap bitmap;
        public readonly float Gamma;
        public readonly float RotationOffset;

        public int Width { get => bitmap.Width; }
        public int Height { get => bitmap.Height; }

        public Texture(string path, float gamma = 2.2f, float rotationOffset = 0) {
            bitmap = new Bitmap(path);
            this.Gamma = gamma;
            this.RotationOffset = rotationOffset;
        }

        public Vector3 GetPixel(float s, float t) {
            float clamp(float v) => Math.Min(Math.Max(v, 0), 1);
            int x = (int)(((clamp(s) + RotationOffset / (2 * Math.PI)) % 1.0) * (Width - 1));
            int y = (int)((1 - clamp(t)) * (Height - 1));
            Color c = bitmap.GetPixel(x, y);
            return new Vector3(c.R, c.G, c.B).FromSRGB(Gamma);
        }
    }
}

