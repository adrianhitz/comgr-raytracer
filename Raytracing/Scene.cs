﻿using Raytracing.Acceleration;
using Raytracing.Acceleration.BVH;
using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing {

    /// <summary>
    /// Represents a scene with objects and light sources, that can be drawn by the <see cref="Raytracer"/>.
    /// </summary>
    public class Scene {
        private ISceneObjectContainer SceneObjects { get; }
        private List<LightSource> LightSources { get; }
        private static readonly float HIT_POINT_ADJUSTMENT = 0.01f;

        /// <summary>
        /// The k value used for the calculation of Phong reflection.
        /// </summary>
        public int PhongK { get; set; } = 40;

        /// <summary>
        /// The scene's ambient light. Default is none. Component values outside the range [0,1] will be clamped.
        /// </summary>
        public Vector3 AmbientLight {
            get => ambientLight;
            set => ambientLight = value.Clamp();
        }
        private Vector3 ambientLight = Colour.Black;

        /// <summary>
        /// Brightness of shadows, with 0 representing completely black shadows and 1 representing no shadows.
        /// Values outside the range [0,1] will be clamped.
        /// </summary>
        public float ShadowBrightness {
            get => minShadowBrightness;
            set => minShadowBrightness = Math.Min(Math.Max(value, 0), 1);
        }
        private float minShadowBrightness = 0.1f;

        /// <summary>
        /// Represents acceleration structures that can be used to accelerate the rendering of the scene by having to compare fewer objects.s
        /// </summary>
        public enum AccelerationStructure {
            None, BVH, Grid
        };

        /// <summary>
        /// Which acceleration structure this scene is using. Default is none.
        /// </summary>
        public AccelerationStructure Acceleration { get; set; } = AccelerationStructure.None;

        /// <summary>
        /// Creates a scene
        /// </summary>
        /// <param name="sceneObjects">Drawable scene objects</param>
        /// <param name="lightSources">Light sources</param>
        /// <param name="acceleration">Which acceleration structure the scene should use</param>
        public Scene(IEnumerable<ISceneObject> sceneObjects, IEnumerable<LightSource> lightSources, AccelerationStructure acceleration = AccelerationStructure.None) {
            switch(acceleration) {
                case AccelerationStructure.BVH:
                    this.SceneObjects = new BoundingVolumeHierarchy(sceneObjects);
                    break;
                case AccelerationStructure.Grid:
                    throw new NotImplementedException();
                default:
                    this.SceneObjects = new SceneObjectList(sceneObjects);
                    break;
            }
            this.LightSources = new List<LightSource>(lightSources);
        }

        /// <summary>
        /// Creates an empty scene
        /// </summary>
        /// <param name="acceleration">Which acceleration structure the scene should use</param>
        public Scene(AccelerationStructure acceleration = AccelerationStructure.None)
            : this(new List<ISceneObject>(), new List<LightSource>(), acceleration) { }

        /// <summary>
        /// Adds a drawable object to the scene
        /// </summary>
        /// <param name="sceneObject">A drawable object</param>
        public void AddObject(ISceneObject sceneObject) {
            SceneObjects.Add(sceneObject);
        }

        /// <summary>
        /// Adds multiple drawable objects to the scene
        /// </summary>
        /// <param name="sceneObjects">Drawable objects</param>
        public void AddObjects(IEnumerable<ISceneObject> sceneObjects) {
            SceneObjects.AddRange(sceneObjects);
        }

        /// <summary>
        /// Adds a light source to the scene
        /// </summary>
        /// <param name="lightSource">A light source</param>
        public void AddLightSource(LightSource lightSource) {
            LightSources.Add(lightSource);
        }

        /// <summary>
        /// Adds multiple light sources to the scene
        /// </summary>
        /// <param name="lightSources">Multiple light sources.</param>
        public void AddLightSources(IEnumerable<LightSource> lightSources) {
            LightSources.AddRange(lightSources);
        }

        /// <summary>
        /// Finds the closest intersection of a given ray with a drawable object in the scene.
        /// </summary>
        /// <param name="ray">The ray, e.g. an eye ray</param>
        /// <returns>
        /// A <see cref="HitPoint"/> object containing information about the intersection, including the position in
        /// three-dimensional space and the surface normal.
        /// </returns>
        public HitPoint FindClosestHitPoint(Ray ray) {
            return SceneObjects.FindClosestHitPoint(ray);
        }

        /// <summary>
        /// Calculates the colour of a point that is "seen" by a given ray, including ambient light, shadows, diffuse reflection,
        /// phong reflection and regular reflection with fresnel (depending on the <paramref name="recursionDepth"/>).
        /// </summary>
        /// <param name="ray">The ray used to "see"</param>
        /// <param name="recursionDepth">How far the recursive calculation of reflection should go. 0 means no reflection,
        /// 1 just regular reflections, 2 reflections of reflections, etc.</param>
        /// <returns>A vector containing the linear colour values in (R, G, B) order.</returns>
        public Vector3 CalculateColour(Ray ray, int shadowSamples, Random random, int recursionDepth = 1) {
            HitPoint hitPoint = FindClosestHitPoint(ray);
            Vector3 colour = AmbientLight;
            if(hitPoint != null) {
                colour += hitPoint.Material.Emissive;
                foreach(LightSource lightSource in LightSources) {
                    float illumination = CalculateIllumination(ray, hitPoint, lightSource, shadowSamples, random);
                    illumination = Math.Max(illumination, minShadowBrightness);
                    // Diffuse reflection
                    Vector3 diffuse = hitPoint.Diffuse(lightSource);
                    diffuse *= illumination;
                    colour += diffuse;

                    // Phong reflection
                    if(illumination >= 0.9f) colour += hitPoint.Specular(lightSource, ray.Origin, PhongK);
                }
                // Regular reflection including fresnel
                if(recursionDepth > 0 && !hitPoint.Material.Specular.Equals(Colour.Black)) {
                    Vector3 reflection = CalculateReflection(ray, hitPoint, shadowSamples, random, recursionDepth - 1);
                    Vector3 fresnel = hitPoint.Fresnel(ray);
                    colour += reflection * fresnel;
                }
            }
            return colour;
        }

        /// <summary>
        /// Calculates the reflection for a hit point.
        /// </summary>
        /// <param name="ray">The original eye ray</param>
        /// <param name="hitPoint">The hit point</param>
        /// <param name="shadowSamples">The number of shadow samples</param>
        /// <param name="recursionDepth">The recursion depth ("reflections of reflections")</param>
        /// <returns>The colour of this hit point's reflection</returns>
        private Vector3 CalculateReflection(Ray ray, HitPoint hitPoint, int shadowSamples, Random random, int recursionDepth) {
            Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
            Vector3 reflectedDirection = Vector3.Normalize(Vector3.Reflect(ray.Direction, hitPoint.Normal));
            Ray reflectionRay = new Ray(adjustedPosition, reflectedDirection);
            return CalculateColour(reflectionRay, shadowSamples, random, recursionDepth);
        }

        /// <summary>
        /// Checks whether a given <see cref="HitPoint"/> is illuminated by a specific <see cref="LightSource"/>.
        /// </summary>
        /// <param name="ray">The ray that created the <see cref="HitPoint"/> (used to adjust the position to reduce graphical noise)</param>
        /// <param name="hitPoint">The HitPoint</param>
        /// <param name="lightSource">The LightSource</param>
        /// <returns>True if the hit point is occluded, false if it is illuminated.</returns>
        [Obsolete]
        private bool IsOccluded(Ray ray, HitPoint hitPoint, LightSource lightSource) {
            Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
            Vector3 L = lightSource.Position - adjustedPosition;
            Ray shadowFeeler = new Ray(adjustedPosition, L);
            HitPoint feelerHitPoint = FindClosestHitPoint(shadowFeeler);
            return feelerHitPoint != null && feelerHitPoint.Lambda <= L.Length();
        }

        /// <summary>
        /// Calculates how much a hit point gets illuminated by a specific light source.
        /// </summary>
        /// <param name="ray">The eye ray</param>
        /// <param name="hitPoint">The hit point</param>
        /// <param name="lightSource">The light source</param>
        /// <param name="shadowSamples">The number of shadow samples to be taken</param>
        /// <returns>A number in the range [0, 1] where 0 is no illumination and 1 is complete illumination</returns>
        private float CalculateIllumination(Ray ray, HitPoint hitPoint, LightSource lightSource, int shadowSamples, Random random) {
            if(shadowSamples <= 0) return 1;
            Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
            int reachLight = 0;
            Vector3 L = lightSource.Position - adjustedPosition;
            Ray[] shadowFeelers = lightSource.GenerateShadowFeelers(adjustedPosition, random, shadowSamples);
            foreach(Ray shadowFeeler in shadowFeelers) {
                HitPoint feelerHitPoint = FindClosestHitPoint(shadowFeeler);
                if(!(feelerHitPoint != null && feelerHitPoint.Lambda <= L.Length())) reachLight++;
            }
            return reachLight / (float)shadowSamples;
        }
    }
}
