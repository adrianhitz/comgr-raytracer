using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing.Premade {
    public static partial class Premade {
        public static class DepthOfField {
            public static Camera Camera() {
                Vector3 position = new Vector3(0, -2, -5);
                Vector3 lookAt = Vector3.Zero;
                float fov = (float)Math.PI / 5;
                float apertureRadius = 0.1f;
                return new Camera(position, lookAt, fov, apertureRadius);
            }

            public static Scene Scene() {
                List<ISceneObject> sceneObjects = new List<ISceneObject>();
                List<LightSource> lightSources = new List<LightSource>();
                Material sphereMaterial = new Material(Colour.White, new Vector3(0.2f, 0.2f, 0.2f));
                for(int i = -3; i <= 2; i++) {
                    Vector3 position = new Vector3(0.4f * i, 0, -0.7f * i);
                    sceneObjects.Add(new Sphere(position, 0.3f, sphereMaterial));
                }
                Sphere floor = new Sphere(new Vector3(0, 1001f, 0), 1000, sphereMaterial);
                Sphere backWall = new Sphere(new Vector3(0, 0, 1004), 1000, new Material(Colour.White * 0.7f, Colour.Black));
                sceneObjects.Add(floor);
                sceneObjects.Add(backWall);
                lightSources.Add(new LightSource(new Vector3(0, -10, -1), Colour.White, 1));
                return new Scene(sceneObjects, lightSources);
            }
        }
    }
}
