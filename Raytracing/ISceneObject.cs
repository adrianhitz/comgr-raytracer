using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        HitPoint? CalculateHitPoint(Ray ray);
    }

    public struct HitPoint {
        public float Lambda { get; }
        public Vector3 Location { get; }
        public Vector3 Normal { get; }
        public ISceneObject HitObject { get; }

        public HitPoint(float lambda, Vector3 location, Vector3 normal, ISceneObject hitObject) {
            this.Lambda = lambda;
            this.Location = location;
            this.Normal = normal;
            this.HitObject = hitObject;
        }
    }
}
