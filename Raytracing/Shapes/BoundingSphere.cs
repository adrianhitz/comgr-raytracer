using System.Numerics;

namespace Raytracing.Shapes {

    /// <summary>
    /// A bounding sphere is a sphere that completely encompasses an object.
    /// </summary>
    public class BoundingSphere : Sphere {
        
        /// <summary>
        /// The object this bounding sphere is wrapping
        /// </summary>
        public ISceneObject WrappedObject { get; }

        /// <summary>
        /// Creates a new bounding sphere that 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="r"></param>
        /// <param name="content"></param>
        public BoundingSphere(Vector3 position, float r, ISceneObject content) : base(position, r) {
            this.WrappedObject = content;
        }
    }
}
