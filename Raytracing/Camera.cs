using Raytracing.Shapes;
using System;
using System.Numerics;

namespace Raytracing {

    /// <summary>
    /// Represents a camera in a scene.
    /// </summary>
    public class Camera {

        /// <summary>
        /// The camera's position in three-dimensional space (also called "eye").
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// The camera's up vector.
        /// </summary>
        public Vector3 Up { get; }

        /// <summary>
        /// Where the camera is looking.
        /// </summary>
        public Vector3 LookAt { get; }

        /// <summary>
        /// The camera's field of view, in radians.
        /// </summary>
        public float FOV { get; }

        /// <summary>
        /// The camera's f vector. (Normalised vector from the camera position to the look at position.)
        /// </summary>
        public Vector3 F { get; }

        /// <summary>
        /// The camera's r vector. (Normalised vector pointing to the right on the picture plane.)
        /// </summary>
        public Vector3 R { get; }

        /// <summary>
        /// The camera's u vector. (Normalised vector pointing up on the picture plane.)
        /// </summary>
        public Vector3 U { get; }

        /// <summary>
        /// Creates a new camera with a default up vector of (0, 1, 0).
        /// </summary>
        /// <param name="position">The camera position (also called "eye")</param>
        /// <param name="lookAt">Point at which the camera is looking</param>
        /// <param name="fov">The camera's field of view, in radians</param>
        public Camera(Vector3 position, Vector3 lookAt, float fov) : this(position, lookAt, fov, new Vector3(0, 1, 0)) { }


        /// <summary>
        /// Creates a new camera
        /// </summary>
        /// <param name="position">The camera position (also called "eye")</param>
        /// <param name="lookAt">Point at which the camera is looking</param>
        /// <param name="fov">The camera's field of view, in radians</param>
        /// <param name="up">The camera's up vector</param>
        public Camera(Vector3 position, Vector3 lookAt, float fov, Vector3 up) {
            this.Position = position;
            this.LookAt = lookAt;
            this.Up = up;
            this.FOV = fov;

            this.F = Vector3.Normalize(LookAt - Position);
            this.R = Vector3.Normalize(Vector3.Cross(F, Up));
            this.U = Vector3.Normalize(Vector3.Cross(R, F));
        }

        /// <summary>
        /// Creates an eye ray for a specified pixel
        /// </summary>
        /// <param name="pixel">Pixel for which the eye ray is created. Component values should be in the range [-1,1].</param>
        /// <returns>An eye ray for the specified pixel</returns>
        public Ray CreateEyeRay(Vector2 pixel) {
            Vector3 direction = F + (pixel.X * R + pixel.Y * U) * (float)Math.Tan(FOV / 2);
            return new Ray(Position, direction);
        }

        /// <summary>
        /// Premade camera for the Cornell Box scene.
        /// </summary>
        /// <returns>A premade camera for the Cornell Box scene</returns>
        public static Camera CornellBoxCamera() {
            Vector3 position = new Vector3(0, 0, -4);
            Vector3 lookAt = new Vector3(0, 0, 6);
            float fov = (float)Math.PI / 5;
            return new Camera(position, lookAt, fov);
        }
    }
}
