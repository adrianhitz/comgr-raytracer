using System.Numerics;

namespace Raytracing {

    /// <summary>
    /// Represents an drawable object in a <see cref="Scene"/>.
    /// </summary>
    public interface ISceneObject {

        /// <summary>
        /// The position (centre) of the object in three-dimensional space.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The object's surface material, which defines how it interacts with light.
        /// </summary>
        Material Material { get; }

        /// <summary>
        /// The (optional) texture that is projected onto the object.
        /// </summary>
        Texture Texture { get; }

        /// <summary>
        /// Calculates the closest <see cref="HitPoint"/> of a <see cref="Ray"/> if the <see cref="Ray"/> intersects it. Null otherwise.
        /// Returns a <see cref="HitPoint"/> containing information, most importantly the position in three-dimensional space where the hit occured.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> used to calculate the <see cref="HitPoint"/></param>
        /// <returns>A <see cref="HitPoint"/> containing information, most importantly the position in three-dimensional space where the hit occured.</returns>
        HitPoint CalculateHitPoint(Ray ray);
    }
}
