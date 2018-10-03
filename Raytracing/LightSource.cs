using System.Numerics;

namespace Raytracing {
    public class LightSource {
        public Vector3 Position { get; }
        public Vector3 Colour { get; }

        public LightSource(Vector3 position, Vector3 colour) {
            this.Position = position;
            this.Colour = colour;
        }
    }
}
