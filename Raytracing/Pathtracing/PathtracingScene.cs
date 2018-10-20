using Raytracing.Acceleration;
using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Numerics;

namespace Raytracing.Pathtracing {
    public class PathtracingScene {
        private ISceneObjectContainer SceneObjects { get; }
        private static readonly float HIT_POINT_ADJUSTMENT = 0.01f;

        public PathtracingScene(ISceneObjectContainer sceneObjects) {
            this.SceneObjects = sceneObjects;
        }

        public HitPoint FindClosestHitPoint(Ray ray) {
            return SceneObjects.FindClosestHitPoint(ray);
        }

        public Vector3 CalculateColour(Ray ray, Random random, int recursionDepth) {
            Vector3 colour = Vector3.Zero;
            if(recursionDepth > 0) {
                HitPoint hitPoint = SceneObjects.FindClosestHitPoint(ray);
                if(hitPoint != null) {
                    Vector3 n = hitPoint.Normal;
                    Vector3 randomVector;
                    do {
                        randomVector = random.NextVector3() * 2 - Vector3.One;
                    } while(randomVector.Length() > 1 && randomVector.Angle(n) >= Math.PI / 2);
                    Vector3 adjustedPosition = hitPoint.Position - ray.Direction * HIT_POINT_ADJUSTMENT;
                    Ray newRay = new Ray(hitPoint.Position, randomVector);
                    float lambert = Vector3.Dot(randomVector, n);
                    Vector3 BRDF = hitPoint.Material.Diffuse;
                    colour = hitPoint.Material.Emissive;
                    colour += CalculateColour(newRay, random, recursionDepth - 1) * lambert * BRDF;
                    colour /= 1 / (2 * (float)Math.PI);
                }
            }
            return colour;
        }
    }
}
