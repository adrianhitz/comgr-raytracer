using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {
    public class Scene {
        private List<ISceneObject> SceneObjects;

        public void AddObject(ISceneObject sceneObject) {
            SceneObjects.Add(sceneObject);
        }

        public void AddObjects(IEnumerable<ISceneObject> sceneObjects) {
            SceneObjects.AddRange(sceneObjects);
        }

        private static Scene CornellBox() {
            Scene cornellBox = new Scene();

            cornellBox.AddObjects(new Sphere[] {
                new Sphere(new Vector3(-1001, 0, 0), 1000, Colour.Red),
                new Sphere(new Vector3(1001, 0, 0), 1000, Colour.Blue),
                new Sphere(new Vector3(0, 0, 1001), 1000, Colour.White),
                new Sphere(new Vector3(0, -1001, 0), 1000, Colour.White),
                new Sphere(new Vector3(0, 1001, 0), 1000, Colour.White),
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, Colour.Yellow),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, Colour.LightCyan)
            });

            return cornellBox;

        }
    }
}
