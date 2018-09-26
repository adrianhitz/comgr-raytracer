using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        Colour Colour { get; }
        HitPoint CalculateHitPoint(Ray ray);
    }
}
