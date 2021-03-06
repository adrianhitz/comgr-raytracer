﻿using System;
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

        /// <summary>
        /// Generates a shadow feeler for a given point to this light source.
        /// </summary>
        /// <param name="origin">The point where the shadow feeler originates</param>
        /// <param name="random">An instance of <see cref="Random"/></param>
        /// <returns>A <see cref="Ray"/> representing the shadow feeler</returns>
        private Ray GenerateShadowFeeler(Vector3 origin, Random random) {
            Vector3 L = Vector3.Normalize(Position - origin);
            Vector3 Nx = Vector3.Normalize(Vector3.Cross(L, new Vector3(0, 1, 0)));
            if(Nx == Vector3.Zero) Nx = Vector3.Normalize(Vector3.Cross(L, new Vector3(0, 0, 1)));
            Vector3 Ny = Vector3.Normalize(Vector3.Cross(L, Nx));
            float theta = (float)(2 * Math.PI * random.NextDouble());
            float x = (float)(Math.Sqrt(random.NextDouble()) * Math.Sin(theta));
            float y = (float)(Math.Sqrt(random.NextDouble()) * Math.Cos(theta));
            Vector3 p = Position + Nx * x * Radius + Ny * y * Radius;
            Vector3 Lprime = p - origin;
            Ray shadowFeeler = new Ray(origin, Lprime);
            return shadowFeeler;
        }

        /// <summary>
        /// Generates a number of uniformly distributed shadow feelers originating from a certain point and pointing to this <see cref="LightSource"/>
        /// </summary>
        /// <param name="origin">The point where the shadow feelers originate</param>
        /// <param name="random">An instance of <see cref="Random"/></param>
        /// <param name="n">The number of shadow feelers</param>
        /// <returns>An array containing rays that represent shadow feelers</returns>
        public Ray[] GenerateShadowFeelers(Vector3 origin, Random random, int n = 1) {
            n = Math.Max(n, 1);
            Ray[] rays = new Ray[n];
            if(n > 1) {
                for(int i = 0; i < n; i++) {
                    rays[i] = GenerateShadowFeeler(origin, random);
                }
            } else {
                rays[0] = new Ray(origin, Position - origin);
            }
            return rays;
        }
    }
}
