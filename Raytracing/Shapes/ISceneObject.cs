using System.Numerics;

namespace Raytracing.Shapes {

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
        /// Calculates the closest <see cref="HitPoint"/> of a <see cref="Ray"/> if the <see cref="Ray"/> intersects it. Null otherwise.
        /// Returns a <see cref="HitPoint"/> containing information, most importantly the position in three-dimensional space where the hit occured.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> used to calculate the <see cref="HitPoint"/></param>
        /// <returns>A <see cref="HitPoint"/> containing information, most importantly the position in three-dimensional space where the hit occured.</returns>
        HitPoint CalculateHitPoint(Ray ray);

        /// <summary>
        /// Calculates and returns a bounding sphere that encompasses this scene object.
        /// </summary>
        /// <returns>A bounding sphere that encompasses this scene object.</returns>
        BoundingSphere GetBoundingSphere();
    }
}
