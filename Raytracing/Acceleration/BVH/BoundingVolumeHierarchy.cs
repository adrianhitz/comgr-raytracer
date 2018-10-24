using Raytracing.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace Raytracing.Acceleration.BVH {

    /// <summary>
    /// A bounding volume hierarchy implementation to accelerate the rendering of scenes or more specifically the calculation of hit points. The bounding
    /// volume hierarchy is rebuilt every time something is added. Not ideal, but it avoids using locks. The implementation overall is not thread safe,
    /// but there are no issues as long as only <see cref="FindClosestHitPoint(Ray)"/> is called from a parallel context.
    /// </summary>
    internal class BoundingVolumeHierarchy : ISceneObjectContainer {

        /// <summary>
        /// The spheres contained by this bounding volume hierarchy.
        /// </summary>
        private List<Sphere> spheres;

        /// <summary>
        /// The root node of this binding volume hierarchy
        /// </summary>
        private BVHNode Root { get; set; }

        /// <summary>
        /// Creates an empty bounding volume hierarchy
        /// </summary>
        public BoundingVolumeHierarchy() {
            spheres = new List<Sphere>();
        }

        /// <summary>
        /// Creates a bounding volume hierarchy with the given 
        /// </summary>
        /// <param name="sceneObjects"></param>
        public BoundingVolumeHierarchy(IEnumerable<ISceneObject> sceneObjects) {
            spheres = sceneObjects.Select(s => s.GetBoundingSphere()).ToList<Sphere>();
            CreateBVH();
        }

        /// <summary>
        /// Adds a scene object to this container.
        /// </summary>
        /// <param name="sceneObject">Scene object to add</param>
        public void Add(ISceneObject sceneObject) {
            spheres.Add(sceneObject.GetBoundingSphere());
            CreateBVH();
        }

        /// <summary>
        /// Adds multiple scene objects to this container.
        /// </summary>
        /// <param name="sceneObjects"></param>
        public void AddRange(IEnumerable<ISceneObject> sceneObjects) {
            spheres.AddRange(sceneObjects.Select(s => s.GetBoundingSphere()));
            CreateBVH();
        }

        /// <summary>
        /// Finds the closest intersection of a given ray with a drawable object in this container. If the contents have changed since the last call,
        /// the bounding volume hierarchy will be rebuilt first.
        /// </summary>
        /// <param name="ray">The ray, e.g. an eye ray</param>
        /// <returns>
        /// A <see cref="HitPoint"/> object containing information about the intersection, including the position in
        /// three-dimensional space and the surface normal.
        /// </returns>
        public HitPoint FindClosestHitPoint(Ray ray) {
            List<HitPoint> hitPoints = FindHitPointRecursive(ray, Root);
            return hitPoints.OrderBy(h => h.Lambda).FirstOrDefault(h => h.Lambda >= 0);
        }

        /// <summary>
        /// Recursively goes through all the nodes in this container (usually starting with the root node) to find all hit points for a given ray.
        /// </summary>
        /// <param name="ray">The ray</param>
        /// <param name="current">The node with which the recursive search should begin (usually the root node)</param>
        /// <returns>An unordered list of hit points</returns>
        private List<HitPoint> FindHitPointRecursive(Ray ray, Sphere current) {
            List<HitPoint> hitPoints = new List<HitPoint>();
            if(current is BoundingSphere boundingSphere) {
                HitPoint hitPoint = boundingSphere.WrappedObject.CalculateHitPoint(ray);
                if(hitPoint != null) hitPoints.Add(hitPoint);
            } else if(current is BVHNode bvh) {
                hitPoints.AddRange(FindHitPointRecursive(ray, bvh.Left));
                hitPoints.AddRange(FindHitPointRecursive(ray, bvh.Right));
            }
            return hitPoints;
        }

        /// <summary>
        /// Builds the bounding volume hierarchy based on <see cref="spheres"/>.
        /// </summary>
        private void CreateBVH() {
            List<Sphere> spheresTemp = new List<Sphere>(spheres);
            while(spheresTemp.Count > 1) {
                Root = SmallestBoundingSphere(spheresTemp);
            }
        }

        /// <summary>
        /// Finds the smallest potential bounding sphere for two objects in this list. Removes both objects from the list, adds them to a
        /// new bounding sphere/BVH node and then adds that to the list. Also returns the BVH node.
        /// </summary>
        /// <param name="spheres">A list of spheres</param>
        /// <returns>The BVH node with the smallest volume</returns>
        private BVHNode SmallestBoundingSphere(List<Sphere> spheres) {
            float minBsRadius = float.MaxValue;
            int sphere1 = -1, sphere2 = -1;
            for(int i = 0; i < spheres.Count - 1; i++) {
                for(int j = i + 1; j < spheres.Count; j++) {
                    float bsRadius = ((spheres[j].Position - spheres[i].Position).Length() + spheres[i].R + spheres[j].R) / 2;
                    if(bsRadius < minBsRadius) {
                        minBsRadius = bsRadius;
                        sphere1 = i;
                        sphere2 = j;
                    }
                }
            }
            BVHNode node = new BVHNode(spheres[sphere1], spheres[sphere2]);
            spheres.Remove(spheres[sphere2]);
            spheres.Remove(spheres[sphere1]);
            spheres.Add(node);
            return node;
        }
    }
}
