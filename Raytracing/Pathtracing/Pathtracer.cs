using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Raytracing.Pathtracing {
    public class Pathtracer {
        /// <summary>
        /// The camera used to take a picture of the scene
        /// </summary>
        private Camera Camera { get; }

        /// <summary>
        /// The scene that is drawn by the raytracer
        /// </summary>
        private PathtracingScene Scene { get; }

        /// <summary>
        /// Recursion depth for path tracing
        /// </summary>
        public int RecursionDepth { get; set; }


        /// <summary>
        /// The numbers of samples taken for each pixel
        /// </summary>
        public int Samples { get; set; }


        /// <summary>
        /// Creates a new raytracer with a camera and a scene
        /// </summary>
        /// <param name="camera">A camera</param>
        /// <param name="scene">A scene</param>
        /// <param name="samples">Number of samples for each pixel</param>
        /// <param name="recursionDepth">Recursion depth for path tracing</param>
        public Pathtracer(Camera camera, PathtracingScene scene, int samples = 1, int recursionDepth = 4) {
            this.Camera = camera;
            this.Scene = scene;
            this.Samples = samples;
            this.RecursionDepth = recursionDepth;
        }

        /// <summary>
        /// Calculates all pixels for an image of a given resolution. Returns a two-dimensional array of vectors with (R, G, B) values.
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <returns>A two-dimensional array of vectors with (R, G, B) values.</returns>
        public Vector3[,] CalculatePixels(int width, int height) {
            Vector3[,] pixels = new Vector3[width, height];
            Parallel.For(0, width, x => {
                Random random = new Random();
                for(int y = 0; y < height; y++) {
                    Vector2 pixel = new Vector2((x / (float)(width - 1)) * 2 - 1, (y / (float)(height - 1)) * 2 - 1);
                    Vector3 colour = Colour.Black;
                    Ray eyeRay = Camera.CreateEyeRay(pixel);
                    for(int i = 0; i < Samples; i++) {
                        colour += Scene.CalculateColour(eyeRay, random, RecursionDepth);
                    }
                    pixels[x, y] = colour / Samples;
                }
            });
            return pixels;
        }

        /// <summary>
        /// Calculates all pixels for an image of a given resolution. Converts them to SRGB colour space and returns a byte array with all the values.
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <param name="gamma">Gamma value for transformation to SRGB colour space</param>
        /// <returns>A byte array containing the pixels of the calculated image</returns>
        public byte[] CalculatePixelsByteArray(int width, int height, float gamma = 2.2f) {
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
