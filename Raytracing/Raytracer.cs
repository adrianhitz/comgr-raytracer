using System.Numerics;

namespace Raytracing {
    public class Raytracer {
        private Camera Camera { get; }
        private Scene Scene { get; }
        public int Width;
        public int Height;

        public Raytracer(Camera camera, Scene scene, int width, int height) {
            this.Camera = camera;
            this.Scene = scene;
            this.Width = width; // TODO remove width and height as properties and instead pass them directly as arguments to CalculatePixels
            this.Height = height;
        }

        public Colour[,] CalculatePixels() {
            Colour[,] pixels = new Colour[Width, Height];

            for(int x = 0; x < Width; x++) {
                for(int y = 0; y < Height; y++) {
                    Vector2 pixel = new Vector2((x / (float)(Width - 1)) * 2 - 1, (y / (float)(Height - 1 )) * 2 - 1);
                    Ray eyeRay = Camera.CreateEyeRay(pixel);
                    pixels[x, y] = Scene.CalculateColour(eyeRay);
                }
            }
            return pixels;
        }

        public byte[] CalculatePixelsByteArray() {
            Colour[,] pixels = CalculatePixels();
            int stride = pixels.GetLength(0) * 4;
            byte[] bytes = new byte[stride * Height];
            for(int y = 0; y < pixels.GetLength(0); y++) {
                for(int x = 0; x < pixels.GetLength(1); x++) {
                    int i = (y * pixels.GetLength(0) + x) * 4;
                    bytes[i] = (byte)(pixels[x, y].B * 255); // TODO move this calculation to class colour, also implement lambda correction
                    bytes[i + 1] = (byte)(pixels[x, y].G * 255);
                    bytes[i + 2] = (byte)(pixels[x, y].R * 255);
                }
            }
            return bytes;
        }
    }
}
