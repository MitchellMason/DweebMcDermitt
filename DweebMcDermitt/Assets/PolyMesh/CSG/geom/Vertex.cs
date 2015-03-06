using UnityEngine;

namespace ConstructiveSolidGeometry
{
    /// <summary>
    /// Represents a vertex of a polygon. Use your own vertex class instead of this
    /// one to provide additional features like texture coordinates and vertex
    /// colors. Custom vertex classes need to implement the IVertex interface.
    /// 
    /// </summary>
    public class Vertex : IVertex
    {
        public Vector3 pos { get; set; }
        public Vector3 normal;
		public Vector2 uv;
		public float zone { get; set; }
        // TODO: Memomry optimization - this could probably be a short rather than int, 
        //       accessed as a get/set int property that converts the -32,768 to 32,767 range to 0 to 65535
        public int index = -1;

        public Vertex(Vector3 pos)
        {
            //this.pos = (pos != Vector3.zero) ? pos : Vector3.zero;
            this.pos = pos;
            //this.normal = (this.normal != Vector3.zero) ? normal : Vector3.zero;
        }

        public Vertex(Vector3 pos, Vector3 normal)
        {
            //this.pos = (pos != Vector3.zero) ? pos : Vector3.zero;
            this.pos = pos;
            //this.normal = (normal != Vector3.zero) ? normal : Vector3.zero;
            this.normal = normal;
		}
		
		public Vertex(Vector3 pos, Vector3 normal, Vector2 uv, float zone)
		{
			//this.pos = (pos != Vector3.zero) ? pos : Vector3.zero;
			this.pos = pos;
			//this.normal = (normal != Vector3.zero) ? normal : Vector3.zero;
			this.normal = normal;
			this.uv = uv;
			this.zone = zone;
		}

        public IVertex clone()
        {
            return new Vertex(this.pos, this.normal, this.uv, this.zone);
        }

        public void flip()
        {
            this.normal *= -1f;
        }

        public IVertex interpolate(IVertex other, float t)
        {
            return new Vertex(
                lerp(this.pos, other.pos, t),
                lerp(this.normal, (other as Vertex).normal, t),
				lerp(this.uv, (other as Vertex).uv, t),
				lerp(this.zone, (other as Vertex).zone, t)
            );

        }
		public IVertex setZone(float zone)
		{
			return new Vertex(this.pos, this.normal, this.uv, zone);
		}

        protected Vector3 lerp(Vector3 a, Vector3 b, float t)
        {
            Vector3 ab = b - a;
            ab *= t;
            return a + ab;
		}
		protected Vector2 lerp(Vector2 a, Vector2 b, float t)
		{
			Vector2 ab = b - a;
			ab *= t;
			return a + ab;
		}
		protected float lerp(float a, float b, float t)
		{
			float ab = b - a;
			ab *= t;
			return a + ab;
		}
    }
}