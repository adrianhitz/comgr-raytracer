using Raytracing.Helpers;
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
        /// The camera's aperture radius
        /// </summary>
        public float ApertureRadius { get; }

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
        /// <param name="apertureRadius">The radius of the camera's aperture</param>
        public Camera(Vector3 position, Vector3 lookAt, float fov, float apertureRadius = 0)
            : this(position, lookAt, fov, apertureRadius, new Vector3(0, 1, 0)) { }


        /// <summary>
        /// Creates a new camera
        /// </summary>
        /// <param name="position">The camera position (also called "eye")</param>
        /// <param name="lookAt">Point at which the camera is looking</param>
        /// <param name="fov">The camera's field of view, in radians</param>
        /// <param name="apertureRadius">The radius of the camera's aperture</param>
        /// <param name="up">The camera's up vector</param>
        public Camera(Vector3 position, Vector3 lookAt, float fov, float apertureRadius, Vector3 up) {
            this.Position = position;
            this.LookAt = lookAt;
            this.FOV = fov;
            this.ApertureRadius = apertureRadius;
            this.Up = up;

            this.F = Vector3.Normalize(LookAt - Position);
            this.R = Vector3.Normalize(Vector3.Cross(F, Up));
            this.U = Vector3.Normalize(Vector3.Cross(R, F));
        }

        /// <summary>
        /// Creates an eye ray for a specified pixel. Needs an instance of <see cref="Random"/> if the camera's aperture radius is greater
        /// than 0. If the aperture radius is greater than 0 but no <see cref="Random"/> instance is passed, a radius of 0 is used.
        /// </summary>
        /// <param name="pixel">Pixel for which the eye ray is created. Component values should be in the range [-1,1].</param>
        /// <param name="random">An instance of <see cref="Random"/>. Necessary if the camera's aperture radius is greater than 0.</param>
        /// <returns>An eye ray for the specified pixel</returns>
        public Ray CreateEyeRay(Vector2 pixel, Random random = null) {
            Vector3 fPrime = F;
            Vector3 posPrime = Position;
            if(ApertureRadius > 0 && random != null) {
                float r = (float)Math.Sqrt(random.NextDouble());
                float theta = (float)(random.NextDouble() * 2 * Math.PI);
                Vector3 a = new Vector3((float)(r * Math.Sin(theta)), (float)(r * Math.Cos(theta)), 0) * ApertureRadius;
                posPrime += a;
                fPrime -= a / (LookAt - posPrime).Length();
            }
            Vector3 direction = fPrime + (pixel.X * R + pixel.Y * U) * (float)Math.Tan(FOV / 2);
            return new Ray(posPrime, direction);
        }

        /// <summary>
        /// Creates multiple randomly distributed eye rays for a specified pixel
        //x/ </summary>
        /// <param name="pixel">Pixel for which the eye ray is created. Component values should be in the range [-1,1].</param>
        /// <param name="random">Instance of <see cref="Random"/></param>
        /// <param name="sigma">Standard deviation</param>
        /// <param name="n">The number of rays</param>
        /// <returns>An array containing randomly distributed eye rays for the specified pixel</returns>
        public Ray[] CreateEyeRays(Vector2 pixel, Random random, float sigma, int n = 10) {
            Ray[] rays = new Ray[n];
            for(int i = 0; i < n; i++) {
                float x = (float)random.NextGaussian(pixel.X, sigma);
                float y = (float)random.NextGaussian(pixel.Y, sigma);
                rays[i] = CreateEyeRay(new Vector2(x, y), random);
            }
            return rays;
        }
    }
}
