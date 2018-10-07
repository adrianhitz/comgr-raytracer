using System;
using System.Collections.Generic;

namespace Raytracing.Acceleration {

    /// <summary>
    /// A container for scene objects, based on a list (no acceleration).
    /// </summary>
    class SceneObjectList : ISceneObjectContainer {
        private readonly List<ISceneObject> sceneObjects;

        /// <summary>
        /// Creates an empty scene object list.
        /// </summary>
        public SceneObjectList() {
            this.sceneObjects = new List<ISceneObject>();
        }

        /// <summary>
        /// Creates a scene object list with the passed scene objects.
        /// </summary>
        /// <param name="sceneObjects">Scene objects</param>
        public SceneObjectList(IEnumerable<ISceneObject> sceneObjects) {
            this.sceneObjects = new List<ISceneObject>(sceneObjects);
        }

        /// <summary>
        /// Adds a scene object to this container.
        /// </summary>
        /// <param name="sceneObject">Scene object to add</param>
        public void Add(ISceneObject sceneObject) => this.sceneObjects.Add(sceneObject);

        /// <summary>
        /// Adds multiple scene objects to this container.
        /// </summary>
        /// <param name="sceneObjects">Scene objects to add</param>
        public void AddRange(IEnumerable<ISceneObject> sceneObjects) => this.sceneObjects.AddRange(sceneObjects);

        /// <summary>
        /// Finds the closest hit point for a given ray among all the objects in this container.
        /// Returns the closest hit point. Null if none is found.
        /// </summary>
        /// <param name="ray">Ray for which the hit points should be found</param>
        /// <returns>The closest hit point. Null if none is found.</returns>
        public HitPoint FindClosestHitPoint(Ray ray) {
            HitPoint closestHitPoint = null;
            foreach(ISceneObject sceneObject in sceneObjects) {
                HitPoint hitPoint = sceneObject.CalculateHitPoint(ray);
                if(hitPoint != null && (closestHitPoint == null || hitPoint?.Lambda < closestHitPoint?.Lambda)) {
                    closestHitPoint = hitPoint;
                }
            }
            return closestHitPoint;
        }
    }
}
