namespace Raytracing {
    public interface ISceneObject {
        HitPoint? CalculateHitPoint(Ray ray);
    }

    public struct HitPoint {
        public float Lambda { get; }
        public ISceneObject HitObject { get; }
        
        public HitPoint(float lambda, ISceneObject hitObject) {
            this.Lambda = lambda;
            this.HitObject = hitObject;
        }
    }
}
