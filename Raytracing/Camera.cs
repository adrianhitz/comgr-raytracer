﻿using System;
using System.Numerics;

namespace Raytracing {
    public class Camera {
        public Vector3 Position { get; }
        public Vector3 Up { get; }
        public Vector3 LookAt { get; }
        public float FOV { get; }
        public Vector3 F { get; }
        public Vector3 R { get; }
        public Vector3 U { get; }

        public Camera(Vector3 position, Vector3 lookAt, float fov) : this(position, lookAt, fov, new Vector3(0, 1, 0)) { }

        public Camera(Vector3 position, Vector3 lookAt, float fov, Vector3 up) {
            this.Position = position;
            this.LookAt = lookAt;
            this.Up = up;
            this.FOV = fov;

            this.F = LookAt - Position;
            this.R = Vector3.Cross(F, Up);
            this.U = Vector3.Cross(R, F);
        }

        Ray CreateEyeRay(Vector2 pixel) {
            Vector3 direction = F + (pixel.X * R + pixel.Y * U) * (float)Math.Tan(FOV / 2);
            return new Ray(Position, direction);
        }
    }
}
