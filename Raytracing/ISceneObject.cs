using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        Vector3 Diffuse { get; }
        Vector3 Specular { get; }
        Vector3 Reflective { get; }
        Vector3 Emissive { get; }
        HitPoint CalculateHitPoint(Ray ray);
    }
}