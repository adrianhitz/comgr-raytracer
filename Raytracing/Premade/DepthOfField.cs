using Raytracing.Helpers;
using Raytracing.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Premade {
    public static partial class Premade {
        public static class DepthOfField {
            public static Camera Camera() {
                Vector3 position = new Vector3(0, 0, -5);
                Vector3 lookAt = Vector3.Zero;
                float fov = (float)Math.PI / 5;
                float apertureRadius = 0.5f;
                return new Camera(position, lookAt, fov, apertureRadius);
            }

            public static Scene Scene() {
                List<ISceneObject> sceneObjects = new List<ISceneObject>();
                List<LightSource> lightSources = new List<LightSource>();
                Material sphereMaterial = new Material(Colour.White, new Vector3(0.2f, 0.2f, 0.2f));
                for(int i = -2; i <= 2; i++) {
                    Vector3 position = new Vector3(0.4f * i, 0.2f * i, -0.5f * i);
                    sceneObjects.Add(new Sphere(position, 0.25f, sphereMaterial));
                }

                lightSources.Add(new LightSource(new Vector3(0, -2, -5), Colour.White, 0));
                return new Scene(sceneObjects, lightSources);
            }
        }
    }
}
