using System.Numerics;

namespace Raytracing {

    /// <summary>
    /// Represents a sphere in three-dimensional space.
    /// </summary>
    interface ISphere {

        /// <summary>
        /// The sphere's position/centre in three-dimensional space.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The sphere's radius.
        /// </summary>
        float R { get; }
    }
}
