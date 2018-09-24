using System.Numerics;

namespace Raytracing {
    class Sphere : ISceneObject {
        public Vector3 Centre { get; }
        public float R { get; }
        public Colour Colour { get; }

        public Sphere(Vector3 centre, float r, Colour colour) {
            this.Centre = centre;
            this.R = r;
            this.Colour = colour;
        }
    }
}
