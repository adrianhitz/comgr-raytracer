using System.Numerics;

namespace Raytracing {
    class Colour {
        public float R { get; }
        public float G { get; }
        public float B { get; }

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
