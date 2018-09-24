using System.Collections.Generic;

namespace Raytracing {
    class Scene {
        private List<ISceneObject> spheres;
        private Camera Camera { get; }

        public Scene(Camera camera) {
            this.spheres = new List<ISceneObject>();
        }

        public void AddObject(ISceneObject sceneObject) {
            spheres.Add(sceneObject);
        }

        public void AddObjects(IEnumerable<ISceneObject> sceneObjects) {
            spheres.AddRange(sceneObjects);
        }
    }
}
