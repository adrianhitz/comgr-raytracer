using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {
    public class Scene {
        private List<ISceneObject> SceneObjects { get; }
        private List<LightSource> LightSources { get; }

        public Scene() {
            this.SceneObjects = new List<ISceneObject>();
            this.LightSources = new List<LightSource>();
        }

        public Scene(ISceneObject sceneObject, LightSource lightSource) : this() {
            AddObject(sceneObject);
        }

        public Scene(IEnumerable<ISceneObject> sceneObjects, LightSource lightSource) : this() {
            AddObjects(sceneObjects);
        }

        public Scene(IEnumerable<ISceneObject> sceneObjects, IEnumerable<LightSource> lightSources) {
            AddObjects(sceneObjects);
            AddLightSources(lightSources);
        }

        public void AddObject(ISceneObject sceneObject) {
            SceneObjects.Add(sceneObject);
        }

        public void AddObjects(IEnumerable<ISceneObject> sceneObjects) {
            SceneObjects.AddRange(sceneObjects);
        }

        public void AddLightSource(LightSource lightSource) {
            LightSources.Add(lightSource);
        }

        public void AddLightSources(IEnumerable<LightSource> lightSources) {
            LightSources.AddRange(lightSources);
        }

        public HitPoint FindClosestHitPoint(Ray ray) {
            HitPoint closestHitPoint = null;
            foreach(ISceneObject sceneObject in SceneObjects) {
                HitPoint hitPoint = sceneObject.CalculateHitPoint(ray);
                if(hitPoint != null && (closestHitPoint == null || hitPoint?.Lambda < closestHitPoint?.Lambda)) {
                    closestHitPoint = hitPoint;
                }
            }
            return closestHitPoint;
        }

        // TODO clean this mess up
        public Colour CalculateColour(Ray ray, int recursionDepth = 1) {
            HitPoint hitPoint = FindClosestHitPoint(ray);
            Colour colour = new Colour();
            if(hitPoint != null) {
                Vector3 n = hitPoint.Normal;
                foreach(LightSource lightSource in LightSources) {
                    Vector3 L = Vector3.Normalize(lightSource.Position - hitPoint.Position);
                    Vector3 m = hitPoint.HitObject.Colour;
                    float nL = Vector3.Dot(n, L);
                    if(nL >= 0) {
                        // Diffuse shading TODO move this out of this method
                        Vector3 diffuse = Vector3.Multiply(lightSource.Colour.ToVector3(), m) * nL;
                        colour += new Colour(diffuse);

                        // Specular reflectance
                        Vector3 eh = Vector3.Normalize(hitPoint.Position - ray.Origin);
                        Vector3 r = Vector3.Normalize(2 * nL * n - L);
                        Vector3 specular = lightSource.Colour.ToVector3() * (float)Math.Pow(Vector3.Dot(r, eh), 40);
                        colour += new Colour(specular);
                    }
                }
                // Calculate reflection including fresnel effect
                if(recursionDepth > 0) {
                    // TODO Don't do this if the object doesn't reflect anything.
                    Ray reflectionRay = new Ray(hitPoint.Position + hitPoint.Normal * 0.1f, Vector3.Reflect(ray.Direction, hitPoint.Normal));
                    Vector3 reflection = CalculateColour(reflectionRay, recursionDepth - 1);
                    Vector3 reflectiveness = hitPoint.HitObject.Reflectiveness;
                    Vector3 fresnel = reflectiveness + (Vector3.One - reflectiveness) * (float)Math.Pow(1 - Vector3.Dot(n, ray.Direction * (-1)), 5);
                    colour += new Colour(reflection * fresnel);
                }
            }
            return colour;
        }

        public static Scene CornellBox() {
            Scene cornellBox = new Scene();
            Colour wallReflectiveness = new Colour(0.3f, 0.3f, 0.3f);
            Colour sphereReflectiveness = new Colour(0.4f, 0.4f, 0.4f);
            cornellBox.AddObjects(new Sphere[] {
                new Sphere(new Vector3(-1001, 0, 0), 1000, Colour.Red, wallReflectiveness),
                new Sphere(new Vector3(1001, 0, 0), 1000, Colour.Blue, wallReflectiveness),
                new Sphere(new Vector3(0, 0, 1001), 1000, Colour.White, wallReflectiveness),
                new Sphere(new Vector3(0, -1001, 0), 1000, Colour.White, wallReflectiveness),
                new Sphere(new Vector3(0, 1001, 0), 1000, Colour.White, wallReflectiveness),
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, Colour.Yellow, sphereReflectiveness),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, Colour.LightCyan, sphereReflectiveness)
            });
            cornellBox.AddLightSource(new LightSource(new Vector3(0, -0.9f, 0), new Colour(1, 1, 1)));
            return cornellBox;
        }
    }
}
