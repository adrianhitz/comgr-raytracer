using System.Numerics;

namespace Raytracing {
    public class Colour {
        public float R { get; }
        public float G { get; }
        public float B { get; }

        public readonly Colour Black = new Colour(0, 0, 0);
        public readonly Colour Red = new Colour(1, 0, 0);
        public readonly Colour Green = new Colour(0, 1, 0);
        public readonly Colour Blue = new Colour(0, 0, 1);
        public readonly Colour Yellow = new Colour(1, 1, 0);
        public readonly Colour Magenta = new Colour(1, 0, 1);
        public readonly Colour Cyan = new Colour(0, 1, 1);
        public readonly Colour White = new Colour(1, 1, 1);
        public readonly Colour LightCyan = new Colour(0.8f, 1, 1);

        public Colour(float r, float g, float b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public Vector3 ToVector() {
            return new Vector3(R, G, B);
        }



    }
}
