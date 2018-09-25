using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        Colour Colour { get; }
        HitPoint? CalculateHitPoint(Ray ray);
    }

    public struct HitPoint {
        public float Lambda { get; }
        public Vector3 Position { get; }
        public Vector3 Normal { get; }
        public ISceneObject HitObject { get; }

        public HitPoint(float lambda, Vector3 location, Vector3 normal, ISceneObject hitObject) {
            this.Lambda = lambda;
            this.Position = location;
            this.Normal = normal;
            this.HitObject = hitObject;
        }
    }
}
