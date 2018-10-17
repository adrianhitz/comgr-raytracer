﻿using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Numerics;

namespace Raytracing {

    /// <summary>
    /// Represents a raytracer. Uses a <see cref="Camera"/> to create an image of a <see cref="Scene"/>
    /// </summary>
    public class Raytracer {

        /// <summary>
        /// The camera used to take a picture of the scene
        /// </summary>
        private Camera Camera { get; }

        /// <summary>
        /// The scene that is drawn by the raytracer
        /// </summary>
        private Scene Scene { get; }

        /// <summary>
        /// How far the recursive calculation of reflection should go. 0 means no reflection,
        /// 1 just regular reflections, 2 reflections of reflections, etc.
        /// </summary>
        public int ReflectionLevel { get; set; }

        /// <summary>
        /// The standard deviation used for random sampling of pixels for anti-aliasing
        /// </summary>
        public float GaussSigma { get; set; } = 0.5f;

        /// <summary>
        /// The numbers of samples for anti-aliasing
        /// </summary>
        public int AASamples { get; set; }

        /// <summary>
        /// The numbers of samples to calculate shadows
        /// </summary>
        public int ShadowSamples { get; set; }

        /// <summary>
        /// Creates a new raytracer with a camera and a scene
        /// </summary>
        /// <param name="camera">A camera</param>
        /// <param name="scene">A scene</param>
        public Raytracer(Camera camera, Scene scene, int aaSamples = 1, int shadowSamples = 1, int reflectionLevel = 2) {
            this.Camera = camera;
            this.Scene = scene;
            this.AASamples = aaSamples;
            this.ShadowSamples = shadowSamples;
            this.ReflectionLevel = reflectionLevel;
        }

        /// <summary>
        /// Calculates all pixels for an image of a given resolution. Returns a two-dimensional array of vectors with (R, G, B) values.
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <returns>A two-dimensional array of vectors with (R, G, B) values.</returns>
        public Vector3[,] CalculatePixels(int width, int height) {
            Vector3[,] pixels = new Vector3[width, height];
            Random random = new Random();
            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    Vector2 pixel = new Vector2((x / (float)(width - 1)) * 2 - 1, (y / (float)(height - 1)) * 2 - 1);
                    Vector3 colour = Colour.Black;
                    if(AASamples > 1) {
                        Ray[] eyeRays = Camera.CreateEyeRays(pixel, random, AASamples, GaussSigma / width);
                        foreach(Ray eyeRay in eyeRays) {
                            colour += Scene.CalculateColour(eyeRay, ShadowSamples, ReflectionLevel);
                        }
                        colour /= AASamples;
                    } else {
                        Ray eyeRay = Camera.CreateEyeRay(pixel);
                        colour += Scene.CalculateColour(eyeRay, ShadowSamples, ReflectionLevel);
                    }
                    pixels[x, y] = colour;
                }
            }
            return pixels;
        }

        /// <summary>
        /// Calculates all pixels for an image of a given resolution. Converts them to SRGB colour space and returns a byte array with all the values.
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <param name="gamma">Gamma value for transformation to SRGB colour space</param>
        /// <returns></returns>
        public byte[] CalculatePixelsByteArray(int width, int height, float gamma = 2.2f) {
            // TODO This method should probably just return the WriteableBitmap instead
            Vector3[,] pixels = CalculatePixels(width, height);
            int stride = pixels.GetLength(0) * 3;
            byte[] bytes = new byte[stride * height];
            for(int y = 0; y < pixels.GetLength(0); y++) {
                for(int x = 0; x < pixels.GetLength(1); x++) {
                    int i = (y * pixels.GetLength(0) + x) * 3;
                    Vector3 colourSRGB = pixels[x, y].ToSRGB(gamma);
                    bytes[i] = (byte)(colourSRGB.Z);
                    bytes[i + 1] = (byte)(colourSRGB.Y);
                    bytes[i + 2] = (byte)(colourSRGB.X);
                }
            }
            return bytes;
        }
    }
}
