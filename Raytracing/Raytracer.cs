using System.Numerics;

namespace Raytracing {
    public class Raytracer {
        private Camera Camera { get; }
        private Scene Scene { get; }
        public int RecursionDepth { get; set; } = 3;

        public Raytracer(Camera camera, Scene scene) {
            this.Camera = camera;
            this.Scene = scene;
        }

        public Colour[,] CalculatePixels(int width, int height) {
            Colour[,] pixels = new Colour[width, height];

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    Vector2 pixel = new Vector2((x / (float)(width - 1)) * 2 - 1, (y / (float)(height - 1 )) * 2 - 1);
                    Ray eyeRay = Camera.CreateEyeRay(pixel);
                    pixels[x, y] = Scene.CalculateColour(eyeRay, RecursionDepth);
                }
            }
            return pixels;
        }

        public byte[] CalculatePixelsByteArray(int width, int height, float lambda = 2.2f) {
            Colour[,] pixels = CalculatePixels(width, height);
            int stride = pixels.GetLength(0) * 3;
            byte[] bytes = new byte[stride * height];
            for(int y = 0; y < pixels.GetLength(0); y++) {
                for(int x = 0; x < pixels.GetLength(1); x++) {
                    int i = (y * pixels.GetLength(0) + x) * 3;
                    Vector3 colourSRGB = pixels[x, y].ToSRGB(lambda);
                    bytes[i] = (byte)(colourSRGB.Z);
                    bytes[i + 1] = (byte)(colourSRGB.Y);
                    bytes[i + 2] = (byte)(colourSRGB.X);
                }
            }
            return bytes;
        }
    }
}
