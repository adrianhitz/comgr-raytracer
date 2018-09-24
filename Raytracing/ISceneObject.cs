using System;
using System.Numerics;

namespace Raytracing {
    public interface ISceneObject {
        HitPoint CalculateHitPoint(Ray ray);
    }

    public class HitPoint {
        public bool Exists { get; }
        public float Lambda {
            get {
                if(Exists) return Lambda;
                throw new InvalidOperationException("No hitpoint found. Can't access Lambda.");
            }
            private set {
                Lambda = value;
            }
        }
        public static HitPoint None = new HitPoint();

        private HitPoint() {
            this.Exists = false;
            this.Lambda = 0;
        }

        public HitPoint(float lambda) {
            this.Exists = true;
            this.Lambda = lambda;
        }
    }
}
