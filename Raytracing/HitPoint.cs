using System.Numerics;

namespace Raytracing {
    public class HitPoint {
        public float Lambda { get; }
        public Vector3 Position { get; }
        public Vector3 Normal { get; }
        public ISceneObject HitObject { get; }

        public HitPoint(float lambda, Vector3 position, Vector3 normal, ISceneObject hitObject) {
            this.Lambda = lambda;
            this.Position = position;
            this.Normal = normal;
            this.HitObject = hitObject;
        }
    }
}
