using System.Numerics;

namespace Raytracing {
    public class LightSource {
        public Vector3 Position { get; }
        public Colour Colour { get; }

        public LightSource(Vector3 position, Colour colour) {
            this.Position = position;
            this.Colour = colour;
        }
    }
}
