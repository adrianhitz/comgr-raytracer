using System.Numerics;

namespace Raytracing {
    public class Camera {
        public Vector3 Position { get; }
        public Vector3 Up { get; }
        public Vector3 LookAt { get; }
        public float FOV { get; }

        public Camera(Vector3 position, Vector3 lookAt, float fov) : this(position, lookAt, fov, new Vector3(0, 1, 0)) { }

        public Camera(Vector3 position, Vector3 lookAt, float fov, Vector3 up) {
            this.Position = position;
            this.LookAt = lookAt;
            this.Up = up;
            this.FOV = fov;
        }
    }
}
