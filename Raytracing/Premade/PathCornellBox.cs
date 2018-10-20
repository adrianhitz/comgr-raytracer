using Raytracing.Acceleration;
using Raytracing.Helpers;
using Raytracing.Pathtracing;
using Raytracing.Shapes;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracing.Premade {
    public static partial class Premade {
        public class PathCornellBox {
            public static PathtracingScene Scene(Scene.AccelerationStructure accelerationStructure = Raytracing.Scene.AccelerationStructure.None) {
                Material whiteMaterial = new Material(Colour.White, Colour.Black, Colour.Black);
                var cornellBoxSpheres = new List<ISceneObject> {
                    new Sphere(new Vector3(1001, 0, 0), 1000, new Material(Colour.Red, Colour.Black, Colour.Black)),
                    new Sphere(new Vector3(-1001, 0, 0), 1000, new Material(Colour.Blue, Colour.Black, Colour.Black)),
                    new Sphere(new Vector3(0, 0, 1001), 1000, whiteMaterial),
                    new Sphere(new Vector3(0, -1001, 0), 1000, whiteMaterial),
                    new Sphere(new Vector3(0, 1001, 0), 1000, whiteMaterial),
                    new Sphere(new Vector3(0, 0, -1005), 1000, whiteMaterial),
                    new Sphere(new Vector3(0.6f, 0.7f, -0.6f), 0.3f, new Material(Colour.Yellow, Colour.Black, Colour.Black)),
                    new Sphere(new Vector3(-0.3f, 0.4f, 0.3f), 0.6f, new Material(Colour.LightCyan, Colour.Black, Colour.Black)),
                    new Sphere(new Vector3(0, -10.99f, 0), 10, new Material(Colour.Black, Colour.Black, Colour.Black, new Vector3(20, 20, 20)))
                };
                SceneObjectList sceneObjects = new SceneObjectList(cornellBoxSpheres);
                return new PathtracingScene(sceneObjects);
            }

        }
    }
}
