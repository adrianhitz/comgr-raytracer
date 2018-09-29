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
        // TODO doesn't work with recursionDepth > 1
        public Colour CalculateColour(Ray ray, int recursionDepth = 3) {
            HitPoint hitPoint = FindClosestHitPoint(ray);
            Colour colour = new Colour();
            if(hitPoint != null) {
                Vector3 n = hitPoint.Normal; // TODO remove?
                foreach(LightSource lightSource in LightSources) {
                    Vector3 L = Vector3.Normalize(lightSource.Position - hitPoint.Position); // TODO remove?
                    bool occluded = IsOccluded(hitPoint, lightSource);

                    Vector3 m = hitPoint.HitObject.Colour; // TODO remove?
                    float nL = Vector3.Dot(n, L); // TODO remove?

                    // Diffuse reflection
                    Colour diffuse = hitPoint.Diffuse(lightSource);
                    if(occluded) diffuse *= 0.1f; // TODO make a property or something out of this literal
                    colour += diffuse;

                    // Phong reflection
                    if(!occluded) {
                        Vector3 phong = hitPoint.Phong(lightSource, ray.Origin, 40); // TODO move k value out of here
                        colour += new Colour(phong);
                    }
                }
                // Regular reflection including fresnel
                if(recursionDepth > 0 && hitPoint.HitObject.Reflective != Colour.Black) {
                    Vector3 reflectedDirection = Vector3.Normalize(Vector3.Reflect(ray.Direction, hitPoint.Normal));
                    Ray reflectionRay = new Ray(hitPoint.Position + hitPoint.Normal * 0.1f, reflectedDirection);
                    Vector3 reflection = CalculateColour(reflectionRay, recursionDepth - 1);
                    Vector3 reflectiveness = hitPoint.HitObject.Reflective;
                    Vector3 fresnel = reflectiveness + (Vector3.One - reflectiveness) * (float)Math.Pow(1 - Vector3.Dot(n, Vector3.Reflect(ray.Direction, n)), 5);
                    colour += new Colour(reflection * fresnel);
                }
            }
            return colour;
        }

        private bool IsOccluded(HitPoint hitPoint, LightSource lightSource) {
            Vector3 L = lightSource.Position - hitPoint.Position;
            Ray shadowFeeler = new Ray(hitPoint.Position + hitPoint.Normal * 0.001f, L);
            HitPoint feelerHitPoint = FindClosestHitPoint(shadowFeeler);
            return feelerHitPoint != null && feelerHitPoint.Lambda <= L.Length();
        }

        public static Scene CornellBox() {
            Scene cornellBox = new Scene();
            Colour specular = new Colour(1, 1, 1);
            Colour wallReflectiveness = new Colour(0.1f, 0.1f, 0.1f);
            Colour sphereReflectiveness = new Colour(0.2f, 0.2f, 0.2f);
            cornellBox.AddObjects(new Sphere[] {
                new Sphere(new Vector3(-1001, 0, 0), 1000, Colour.Red, specular, wallReflectiveness),
                new Sphere(new Vector3(1001, 0, 0), 1000, Colour.Blue, specular, wallReflectiveness),
                new Sphere(new Vector3(0, 0, 1001), 1000, Colour.White, specular, wallReflectiveness),
                new Sphere(new Vector3(0, -1001, 0), 1000, Colour.White, specular, wallReflectiveness),
                new Sphere(new Vector3(0, 1001, 0), 1000, Colour.White, specular, wallReflectiveness),
                new Sphere(new Vector3(0, 0, -1005), 1000, Colour.White, specular, wallReflectiveness),
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, Colour.Yellow, specular, sphereReflectiveness),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, Colour.LightCyan, specular, sphereReflectiveness)
            });
            cornellBox.AddLightSource(new LightSource(new Vector3(0, -0.9f, 0), new Colour(1, 1, 1)));
            return cornellBox;
        }
    }
}
