using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {
    public class Scene {
        private List<ISceneObject> SceneObjects { get; }
        public LightSource LightSource { get; set; } // TODO multiple light sources

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

        public HitPoint? FindClosestHitPoint(Ray ray) {
            HitPoint? closestHitPoint = null;
            foreach(ISceneObject sceneObject in SceneObjects) {
                HitPoint? hitPoint = sceneObject.CalculateHitPoint(ray);
                if(hitPoint != null && (closestHitPoint == null || hitPoint?.Lambda < closestHitPoint?.Lambda)) {
                    closestHitPoint = hitPoint;
                }
            }
            return closestHitPoint;
        }

        public Colour CalculateColour(Ray ray) {
            HitPoint? potentialHitPoint = FindClosestHitPoint(ray);
            if(potentialHitPoint.HasValue) {
                HitPoint hitPoint = potentialHitPoint.Value;
                Vector3 L = Vector3.Normalize(LightSource.Position - hitPoint.Position);
                Vector3 n = hitPoint.Normal;
                Vector3 m = hitPoint.HitObject.Colour; // TODO this should probably be in Sphere
                float nL = Vector3.Dot(n, L);
                if(nL >= 0) {
                    return new Colour(Vector3.Multiply(LightSource.Colour.ToVector3(), m) * nL);
                }
            }
            return Colour.Black;
        } 

        public static Scene CornellBox() {
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
            cornellBox.LightSource = new LightSource(new Vector3(0, -0.9f, -0.5f), new Colour(1, 1, 1));
            return cornellBox;
        }
    }
}
