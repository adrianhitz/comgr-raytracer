using System.Numerics;

namespace Raytracing {
    public class Raytracer {
        private Camera Camera { get; }
        private Scene Scene { get; }

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
                    pixels[x, y] = Scene.CalculateColour(eyeRay);
                }
            }
            return pixels;
        }

        public byte[] CalculatePixelsByteArray(int width, int height) {
            Colour[,] pixels = CalculatePixels(width, height);
            int stride = pixels.GetLength(0) * 3;
            byte[] bytes = new byte[stride * height];
            for(int y = 0; y < pixels.GetLength(0); y++) {
                for(int x = 0; x < pixels.GetLength(1); x++) {
                    int i = (y * pixels.GetLength(0) + x) * 3;
                    bytes[i] = (byte)(pixels[x, y].B * 255); // TODO move this calculation to class colour, also implement lambda correction
                    bytes[i + 1] = (byte)(pixels[x, y].G * 255);
                    bytes[i + 2] = (byte)(pixels[x, y].R * 255);
                }
            }
            return bytes;
        }
    }
}
