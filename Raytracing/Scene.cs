using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {
    public class Scene {
        private List<ISceneObject> SceneObjects { get; }
        private List<LightSource> LightSources { get; }
        private static readonly float HIT_POINT_ADJUSTMENT = 0.01f;
        public int PhongK { get; set; } = 40;
        private Vector3 ambientLight = Colour.Black;
        public Vector3 AmbientLight {
            get => ambientLight;
            set => ambientLight = new Vector3(Math.Min(Math.Max(value.X, 0), 1), Math.Min(Math.Max(value.Y, 0), 1), Math.Min(Math.Max(value.Z, 0), 1));
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

        public Vector3 CalculateColour(Ray ray, int recursionDepth = 1) {
            HitPoint hitPoint = FindClosestHitPoint(ray);
            Vector3 colour = AmbientLight;
            colour += hitPoint.HitObject.Material.Emissive;
            if(hitPoint != null) {
                foreach(LightSource lightSource in LightSources) {
                    bool occluded = IsOccluded(ray, hitPoint, lightSource);

                    // Diffuse reflection
                    Vector3 diffuse = hitPoint.Diffuse(lightSource);
                    if(occluded) diffuse *= shadowBrightness;
                    colour += diffuse;

                    // Phong reflection
                    if(!occluded) colour += hitPoint.Phong(lightSource, ray.Origin, PhongK);
                }
                // Regular reflection including fresnel
                if(recursionDepth > 0 && !hitPoint.HitObject.Material.Reflective.Equals(Colour.Black)) {
                    Vector3 reflection = CalculateReflection(ray, hitPoint, recursionDepth - 1);
                    Vector3 fresnel = hitPoint.Fresnel(ray);
                    colour += reflection * fresnel * hitPoint.HitObject.Material.Reflective;
                }
            }
            return colour;
        }

        private Vector3 CalculateReflection(Ray ray, HitPoint hitPoint, int recursionDepth) {
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
            Vector3 specularWall = new Vector3(0, 0, 0);
            Vector3 specularSphere = new Vector3(1, 1, 1);
            Vector3 wallReflectiveness = new Vector3(0.05f, 0.05f, 0.05f);
            Vector3 sphereReflectiveness = new Vector3(0.1f, 0.1f, 0.1f);
            Material whiteWall = new Material(Colour.White, specularWall, wallReflectiveness);
            cornellBox.AddObjects(new Sphere[] {
                new Sphere(new Vector3(1001, 0, 0), 1000, new Material(Colour.Red, specularWall, wallReflectiveness)),
                new Sphere(new Vector3(-1001, 0, 0), 1000, new Material(Colour.Blue, specularWall, wallReflectiveness)),
                new Sphere(new Vector3(0, 0, 1001), 1000, whiteWall),
                new Sphere(new Vector3(0, -1001, 0), 1000, whiteWall),
                new Sphere(new Vector3(0, 1001, 0), 1000, whiteWall),
                new Sphere(new Vector3(0, 0, -1005), 1000,whiteWall),
                new Sphere(new Vector3(0.6f, 0.7f, -0.6f), 0.3f, new Material(Colour.Yellow, specularSphere, sphereReflectiveness), new Texture(@"Resources\pluto.jpg", rotationOffset: 0.8f*(float)Math.PI)),
                new Sphere(new Vector3(-0.3f, 0.4f, 0.3f), 0.6f, new Material(Colour.LightCyan, specularSphere, sphereReflectiveness), new Texture(@"Resources\earth.jpg", rotationOffset: 1.2f*(float)Math.PI))
            });
            cornellBox.AddLightSource(new LightSource(new Vector3(0, -0.9f, -0.6f), new Vector3(0.15f, 0.7f, 0.15f)));
            cornellBox.AddLightSource(new LightSource(new Vector3(0.4f, -0.9f, 0f), new Vector3(0.7f, 0.15f, 0.15f)));
            cornellBox.AddLightSource(new LightSource(new Vector3(-0.4f, -0.9f, 0f), new Vector3(0.15f, 0.15f, 0.7f)));
            return cornellBox;
        }
    }
}
