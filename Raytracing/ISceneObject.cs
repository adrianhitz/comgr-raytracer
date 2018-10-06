using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        Vector3 Position { get; }
        Material Material { get; }
        Texture Texture { get; }
        HitPoint CalculateHitPoint(Ray ray);
    }
}
