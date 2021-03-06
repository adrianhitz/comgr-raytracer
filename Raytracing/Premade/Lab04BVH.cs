﻿using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing.Premade {
    public static partial class Premade {
        public static class Lab04BVH {
            public static Camera Camera() {
                Vector3 position = new Vector3(0, 0, -4);
                Vector3 lookAt = new Vector3(0, 0, 6);
                float fov = (float)Math.PI / 5;
                return new Camera(position, lookAt, fov);
            }

            public static Scene Scene(Scene.AccelerationStructure accelerationStructure = Raytracing.Scene.AccelerationStructure.BVH) {
                int n = 20;
                List<ISceneObject> sceneObjects = new List<ISceneObject>();
                Random random = new Random();

                for(int k = 0; k < 3; k++) {
                    for(int i = 0; i < n; i++) {
                        for(int j = 0; j < n; j++) {
                            Vector3 diffuse = random.NextVector3();
                            Vector3 specular = new Vector3(0.1f, 0.1f, 0.1f);
                            Material material = new Material(diffuse, specular);
                            Vector3 position = new Vector3((6f / n) * i - 2f, (6f / n) * j - 2f, k * 0.5f - 0.5f);
                            float r = (6f / n) * (0.95f / 2);

                            sceneObjects.Add(new Sphere(position, r, material));
                        }
                    }
                }
                sceneObjects.Add(new Sphere(new Vector3(0, 0, 1003f), 1000, new Material(Colour.White, new Vector3(0.1f, 0.1f, 0.1f))));

                LightSource[] lightSources = new LightSource[] {
                    new LightSource(new Vector3(-1, -1, -2), Colour.White)
                };

                return new Scene(sceneObjects, lightSources, accelerationStructure);
            }
        }
    }
}
