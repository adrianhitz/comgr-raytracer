using System.Collections.Generic;

namespace Raytracing {
    public class Raytracer {
        private Camera Camera { get; }
        private Scene Scene { get; }

        public Raytracer(Camera camera, Scene scene) {
            this.Camera = camera;
            this.Scene = scene;
        }
    }
}
