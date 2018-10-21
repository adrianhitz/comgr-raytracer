using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing.Premade {
    public partial class Premade {
        public static class Lab03 {
            public static Camera Camera() {
                Vector3 position = new Vector3(0, 0, -4);
                Vector3 lookAt = new Vector3(0, 0, 6);
                float fov = (float)Math.PI / 5;
                return new Camera(position, lookAt, fov);
            }
            
            public static Scene Scene() {
                Vector3 specularWall = new Vector3(0.05f, 0.05f, 0.05f);
                Vector3 specularSphere = new Vector3(0.15f, 0.15f, 0.15f);
                Material whiteWall = new Material(Colour.White, specularWall);
                var cornellBoxSpheres = new List<ISceneObject> {
                    new Sphere(new Vector3(1001, 0, 0), 1000, new Material(Colour.Red, specularWall)),
                    new Sphere(new Vector3(-1001, 0, 0), 1000, new Material(Colour.Blue, specularWall)),
                    new Sphere(new Vector3(0, 0, 1001), 1000, whiteWall),
                    new Sphere(new Vector3(0, -1001, 0), 1000, whiteWall),
                    new Sphere(new Vector3(0, 1001, 0), 1000, whiteWall),
                    new Sphere(new Vector3(0, 0, -1005), 1000,whiteWall),
                    new Sphere(new Vector3(0.6f, 0.7f, -0.6f), 0.3f, new Material(Colour.Yellow, specularSphere, new Vector3(0.5f, 0.5f, 0.5f))),
                    new Sphere(new Vector3(-0.3f, 0.4f, 0.3f), 0.6f, new Material(Colour.LightCyan, specularSphere))
                };
                var cornellBoxLight = new List<LightSource> {
                    new LightSource(new Vector3(0, -0.9f, -0.6f), new Vector3(0, 0.5f, 0.5f)),
                    new LightSource(new Vector3(0.4f, -0.9f, 0f), new Vector3(0.5f, 0.5f, 0)),
                    new LightSource(new Vector3(-0.4f, -0.9f, 0f), new Vector3(0.5f, 0, 0.5f))
                };
                return new Scene(cornellBoxSpheres, cornellBoxLight);
            }
        }
    }
}
