using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        Colour Colour { get; }
        Colour Specular { get; }
        Colour Reflective { get; }
        Colour Emissive { get; }
        HitPoint CalculateHitPoint(Ray ray);
    }
}
