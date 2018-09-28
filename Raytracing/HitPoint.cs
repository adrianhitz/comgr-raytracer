using System.Numerics;

namespace Raytracing {
    public class HitPoint {
        public float Lambda { get; }
        public Vector3 Position { get; }
        public Vector3 Normal { get; }
        public ISceneObject HitObject { get; }

        public HitPoint(float lambda, Vector3 position, Vector3 normal, ISceneObject hitObject) {
            this.Lambda = lambda;
            this.Position = position;
            this.Normal = normal;
            this.HitObject = hitObject;
        }

        internal Colour Diffuse(LightSource lightSource) {
            Vector3 L = Vector3.Normalize(lightSource.Position - this.Position);
            float nL = Vector3.Dot(this.Normal, L);
            Colour diffuse = Colour.Black;
            return nL >= 0 ? (Colour)(Vector3.Multiply(lightSource.Colour, this.HitObject.Colour) * nL) : Colour.Black;
        }
    }
}
