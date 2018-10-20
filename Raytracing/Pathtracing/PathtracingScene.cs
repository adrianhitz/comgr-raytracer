using Raytracing.Acceleration;
using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Numerics;

namespace Raytracing.Pathtracing {

    /// <summary>
    /// Represents a scene. Contains objects that can be drawn by the pathtracer.
    /// </summary>
    public class PathtracingScene {
        private ISceneObjectContainer SceneObjects { get; }
        private static readonly float HIT_POINT_ADJUSTMENT = 0.01f;

        /// <summary>
        /// Creates a scene from a container of scene objects
        /// </summary>
        /// <param name="sceneObjects"></param>
        public PathtracingScene(ISceneObjectContainer sceneObjects) {
            this.SceneObjects = sceneObjects;
        }

        /// <summary>
        /// Finds the closest intersection of a given ray with a scene object in the scene.
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
        /// Calculates the colour of a point that is "seen" by a given ray
        /// </summary>
        /// <param name="ray">The ray used to "see"</param>
        /// <param name="random">An instance of <see cref="Random"/></param>
        /// <param name="recursionDepth">How many times the ray will be reflected</param>
        /// <returns>A vector containing the linear colour values in (R, G, B) order.</returns>
        public Vector3 CalculateColour(Ray ray, Random random, int recursionDepth) {
            HitPoint hitPoint = SceneObjects.FindClosestHitPoint(ray);
            Vector3 colour = Colour.Black;
            if(hitPoint != null) {
                colour = hitPoint.Material.Emissive; ;
                if(recursionDepth > 0) {
                    Vector3 n = hitPoint.Normal;
                    Vector3 randomVector;
                    do {
                        randomVector = random.NextVector3() * 2 - Vector3.One;
                    } while(randomVector.Length() > 1 && randomVector.Angle(n) >= Math.PI / 2);
                    Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
                    Ray newRay = new Ray(adjustedPosition, randomVector);
                    float lambert = Vector3.Dot(randomVector, n);
                    Vector3 BRDF = hitPoint.Material.Diffuse;
                    colour += CalculateColour(newRay, random, recursionDepth - 1) * lambert * BRDF;
                    colour /= 1 / (2 * (float)Math.PI);
                }
            }
            return colour;
        }
    }
}
