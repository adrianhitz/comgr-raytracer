using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {
    public class Scene {
        private List<ISceneObject> SceneObjects { get; }
        private List<LightSource> LightSources { get; }
        private static readonly float HIT_POINT_ADJUSTMENT = 0.01f;
        public int PhongK { get; set; } = 40;
        private Colour ambientLight = Colour.Black;
        public Colour AmbientLight {
            get => ambientLight;
            set => ambientLight = new Colour(Math.Min(Math.Max(value.R, 0), 1), Math.Min(Math.Max(value.G, 0), 1), Math.Min(Math.Max(value.B, 0), 1));
        }
        private float shadowBrightness = 0.1f;
        public float ShadowBrightness {
            get => shadowBrightness;
            set => shadowBrightness = Math.Min(Math.Max(value, 0), 1);
        }

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

        public Colour CalculateColour(Ray ray, int recursionDepth = 1) {
            HitPoint hitPoint = FindClosestHitPoint(ray);
            Colour colour = AmbientLight;
            colour += hitPoint.HitObject.Emissive;
            if(hitPoint != null) {
                foreach(LightSource lightSource in LightSources) {
                    bool occluded = IsOccluded(ray, hitPoint, lightSource);

                    // Diffuse reflection
                    Colour diffuse = hitPoint.Diffuse(lightSource);
                    if(occluded) diffuse *= shadowBrightness;
                    colour += diffuse;

                    // Phong reflection
                    if(!occluded) colour += new Colour(hitPoint.Phong(lightSource, ray.Origin, PhongK));
                }
                // Regular reflection including fresnel
                if(recursionDepth > 0 && !hitPoint.HitObject.Reflective.Equals(Colour.Black)) {
                    Vector3 reflection = CalculateReflection(ray, hitPoint, recursionDepth - 1);
                    Vector3 fresnel = hitPoint.Fresnel(ray);
                    colour += new Colour(reflection * fresnel * hitPoint.HitObject.Reflective);
                }
            }
            return colour;
        }

        private Colour CalculateReflection(Ray ray, HitPoint hitPoint, int recursionDepth) {
            Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
            Vector3 reflectedDirection = Vector3.Normalize(Vector3.Reflect(ray.Direction, hitPoint.Normal));
            Ray reflectionRay = new Ray(adjustedPosition, reflectedDirection);
            return CalculateColour(reflectionRay, recursionDepth);
        }

        private bool IsOccluded(Ray ray, HitPoint hitPoint, LightSource lightSource) {
            Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
            Vector3 L = lightSource.Position - adjustedPosition;
            Ray shadowFeeler = new Ray(adjustedPosition, L);
            HitPoint feelerHitPoint = FindClosestHitPoint(shadowFeeler);
            return feelerHitPoint != null && feelerHitPoint.Lambda <= L.Length();
        }

        public static Scene CornellBox() {
            Scene cornellBox = new Scene();
            Colour specularWall = new Colour(0, 0, 0);
            Colour specularSphere = new Colour(1, 1, 1);
            Colour wallReflectiveness = new Colour(0.05f, 0.05f, 0.05f);
            Colour sphereReflectiveness = new Colour(0.4f, 0.4f, 0.4f);
            cornellBox.AddObjects(new Sphere[] {
                new Sphere(new Vector3(1001, 0, 0), 1000, Colour.Red, specularWall, wallReflectiveness),
                new Sphere(new Vector3(-1001, 0, 0), 1000, Colour.Blue, specularWall, wallReflectiveness),
                new Sphere(new Vector3(0, 0, 1001), 1000, Colour.White, specularWall, wallReflectiveness),
                new Sphere(new Vector3(0, -1001, 0), 1000, Colour.White, specularWall, wallReflectiveness),
                new Sphere(new Vector3(0, 1001, 0), 1000, Colour.White, specularWall, wallReflectiveness),
                new Sphere(new Vector3(0, 0, -1005), 1000, Colour.White, specularWall, wallReflectiveness),
                new Sphere(new Vector3(0.6f, 0.7f, -0.6f), 0.3f, Colour.Yellow, specularWall, sphereReflectiveness),
                new Sphere(new Vector3(-0.3f, 0.4f, 0.3f), 0.6f, Colour.LightCyan, specularWall, sphereReflectiveness)
            });
            cornellBox.AddLightSource(new LightSource(new Vector3(0, -0.9f, -0.3f), new Colour(0.15f, 0.7f, 0.15f)));
            cornellBox.AddLightSource(new LightSource(new Vector3(0.4f, -0.9f, 0.3f), new Colour(0.7f, 0.15f, 0.15f)));
            cornellBox.AddLightSource(new LightSource(new Vector3(-0.4f, -0.9f, 0.3f), new Colour(0.15f, 0.15f, 0.7f)));
            return cornellBox;
        }
    }
}
