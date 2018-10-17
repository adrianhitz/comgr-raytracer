using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// Represents a point light source. Illuminates a <see cref="Scene"/>.
    /// </summary>
    public class LightSource {

        /// <summary>
        /// The light source's position
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// The light source's colour/intensity
        /// </summary>
        public Vector3 Colour { get; }

        /// <summary>
        /// The light source's radius. Can be zero.
        /// </summary>
        public float Radius { get; }

        /// <summary>
        /// Creates a new light source
        /// </summary>
        /// <param name="position">The light source's position</param>
        /// <param name="colour">The light source's colour/intensity</param>
        /// <param name="radius">The light source's radius</param>
        public LightSource(Vector3 position, Vector3 colour, float radius = 0) {
            this.Position = position;
            this.Colour = colour;
            this.Radius = radius;
        }

        private Ray GenerateShadowFeeler(Vector3 origin, Random random) {
            Vector3 L = Vector3.Normalize(Position - origin);
            Vector3 Nx = Vector3.Normalize(Vector3.Cross(L, new Vector3(0, 1, 0)));
            if(Nx == Vector3.Zero) Nx = Vector3.Normalize(Vector3.Cross(L, new Vector3(0, 0, 1)));
            Vector3 Ny = Vector3.Normalize(Vector3.Cross(L, Nx));
            // Generate a random point
            // TODO Random as parameter?
            float r = (float)random.NextDouble();
            float theta = (float)(2 * Math.PI * random.NextDouble());
            float x = (float)(Math.Sqrt(r) * Math.Sin(theta));
            float y = (float)(Math.Sqrt(r) * Math.Cos(theta));
            Vector3 p = Position + Nx * x * Radius + Ny * y * Radius;
            Vector3 Lprime = p - origin;
            Ray shadowFeeler = new Ray(Position, Lprime);
            return shadowFeeler;
        }

        public Ray[] GenerateShadowFeelers(Vector3 origin, int n = 1) {
            n = Math.Max(n, 1);
            Ray[] rays = new Ray[n];
            Random random = new Random();
            for(int i = 0; i < n; i++) {
                rays[i] = GenerateShadowFeeler(origin, random);
            }
            return rays;
        }
    }
}
