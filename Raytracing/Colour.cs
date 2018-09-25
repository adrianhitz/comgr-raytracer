using System.Numerics;

namespace Raytracing {
    public class Colour {
        public float R { get; }
        public float G { get; }
        public float B { get; }

        public static readonly Colour Black = new Colour(0, 0, 0);
        public static readonly Colour Red = new Colour(1, 0, 0);
        public static readonly Colour Green = new Colour(0, 1, 0);
        public static readonly Colour Blue = new Colour(0, 0, 1);
        public static readonly Colour Yellow = new Colour(1, 1, 0);
        public static readonly Colour Magenta = new Colour(1, 0, 1);
        public static readonly Colour Cyan = new Colour(0, 1, 1);
        public static readonly Colour White = new Colour(1, 1, 1);
        public static readonly Colour LightCyan = new Colour(0.8f, 1, 1);

        public Colour(float r, float g, float b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public Colour(Vector3 colour) : this(colour.X, colour.Y, colour.Z) {
        }

        public static implicit operator Vector3(Colour colour) {
            return new Vector3(colour.R, colour.G, colour.B);
        }

        public Vector3 ToVector3() {
            return new Vector3(R, G, B);
        }
    }
}
