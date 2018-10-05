using System.Numerics;

namespace Raytracing {
    public class Material {
        public Vector3 Diffuse { get; }
        public Vector3 Specular { get; }
        public Vector3 Reflective { get; }
        public Vector3 Emissive { get; }

        public Material(Vector3 diffuse, Vector3 specular, Vector3 reflective, Vector3 emissive) {
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Reflective = reflective;
            this.Emissive = emissive;
        }

        public Material(Vector3 diffuse, Vector3 specular, Vector3 reflective) : this(diffuse, specular, reflective, Colour.Black) { }
    }
}
