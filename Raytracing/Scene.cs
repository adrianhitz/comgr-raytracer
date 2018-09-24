using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {
    public class Scene {
        private List<ISceneObject> SceneObjects { get; }

        public Scene() {
            this.SceneObjects = new List<ISceneObject>();
        }

        public Scene(ISceneObject sceneObject) : this() {
            AddObject(sceneObject);
        }

        public Scene(IEnumerable<ISceneObject> sceneObjects) : this() {
            AddObjects(sceneObjects);
        }

        public void AddObject(ISceneObject sceneObject) {
            SceneObjects.Add(sceneObject);
        }

        public void AddObjects(IEnumerable<ISceneObject> sceneObjects) {
            SceneObjects.AddRange(sceneObjects);
        }

        public HitPoint FindClosestHitPoint(Ray ray) {
            HitPoint closestHitPoint = HitPoint.None;
            foreach(ISceneObject sceneObject in SceneObjects) {
                HitPoint hitPoint = sceneObject.CalculateHitPoint(ray);
                if(hitPoint.Exists) {
                    if(!closestHitPoint.Exists || hitPoint.Lambda < closestHitPoint.Lambda) {
                        closestHitPoint = hitPoint;
                    }
                }
            }
            return closestHitPoint;
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
