namespace Raytracing {
    public interface ISceneObject {
        Material Material { get; }
        Texture Texture { get; }
        HitPoint CalculateHitPoint(Ray ray);
    }
}
