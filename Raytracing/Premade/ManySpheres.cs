using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing.Premade {
    public static partial class Premade {
        public static class ManySpheres {
            public static Camera Camera() {
                Vector3 position = new Vector3(0, 0, -4);
                Vector3 lookAt = new Vector3(0, 0, 6);
                float fov = (float)Math.PI / 5;
                return new Camera(position, lookAt, fov);
            }

            public static Scene Scene(Scene.AccelerationStructure accelerationStructure = Raytracing.Scene.AccelerationStructure.BVH) {
                int n = 10;
                List<ISceneObject> sceneObjects = new List<ISceneObject>();
                Random random = new Random();
                
                for(int i = 0; i < n; i++) {
                    for(int j = 0; j < n; j++) {
                        Vector3 diffuse = random.NextVector3();
                        Vector3 specular = new Vector3(0.8f, 0.8f, 0.8f);
                        Material material = new Material(diffuse, specular);
                        Vector3 position = new Vector3((3f / n) * i - 1.5f, (3f / n) * j - 1.5f, 0);
                        float r = (3f / n) * (0.9f / 2);

                        sceneObjects.Add(new Sphere(position, r, material));
                    }
                }
                for(int i = 0; i < n; i++) {
                    for(int j = 0; j < n; j++) {
                        Vector3 diffuse = random.NextVector3();
                        Vector3 specular = new Vector3(0.8f, 0.8f, 0.8f);
                        Vector3 reflective = new Vector3(0.5f, 0.5f, 0.5f);
                        Material material = new Material(diffuse, specular, reflective);
                        Vector3 position = new Vector3((3f / n) * (i + 0.5f) - 1.5f, (3f / n) * (j + 0.5f) - 1.5f, 0.2f);
                        float r = (3f / n) * (0.95f / 2);

                        sceneObjects.Add(new Sphere(position, r, material));
                    }
                }
                sceneObjects.Add(new Sphere(new Vector3(0, 0, 1003f), 1000, new Material(Colour.White, new Vector3(0.1f, 0.1f, 0.1f))));

                LightSource[] lightSources = new LightSource[] {
                    new LightSource(new Vector3(-1, -1, -2), Colour.White)
                };

                return new Scene(sceneObjects, lightSources);

            }
        }
    }
}
