using System.Numerics;

namespace Raytracing {
    class Ray {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction) {
            this.Origin = origin;
            this.Direction = Vector3.Normalize(direction);
        }
    }
}
