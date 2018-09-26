using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        Colour Colour { get; }
        Colour Reflectiveness { get; }
        HitPoint CalculateHitPoint(Ray ray);
    }
}
