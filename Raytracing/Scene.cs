using System.Collections.Generic;

namespace Raytracing {
    class Scene {
        private List<Sphere> spheres;
        private Camera Camera { get; }

        public Scene(Camera camera) {
            this.spheres = new List<Sphere>();
        }

        public void AddSphere(Sphere sphere) {
            spheres.Add(sphere);
        }

        public void AddSpheres(IEnumerable<Sphere> s) {
            spheres.AddRange(s);
        }
    }
}
