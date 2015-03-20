using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Poly2Tri;
namespace LevelEditor
{
	/*
	using Path = List<Vector2>;
	using Paths = List<List<Vector2>>;

	public class Poly
	{
		public List<Vector3> boundary = new List<Vector3>();

		public List<Poly> holes = new List<Poly>();
		public Poly(){}
		public Poly(Vector3[] verts)
		{
			boundary.AddRange (verts);
		}
	}

	public class HalfEdge
	{
		public int i0, i1;
		public HalfEdge(int e0, int e1)
		{
			i0 = e0;
			i1 = e1;
			hasNeighbor = false;
		}
		public bool hasNeighbor = false;
	}
	public class SubMesh
	{

		public List<Vector3> verts = new List<Vector3>();
		public List<Vector3> norms = new List<Vector3>();
		public List<Vector2> uvs = new List<Vector2>();
		public List<float> zones = new List<float>();
		public List<int> inds = new List<int>();
		public CSGObject getSolid()
		{
			CSGObject s = new CSGObject ();
			if (inds.Count <= 0)
				return s;
			//List<Vector3> pts = new List<Vector3> ();
			for (int i = 0; i < verts.Count; ++i)
			{
				Vector3 sample = verts[i];
				for (int j = i + 1; j < verts.Count; ++j)
				{
					if (Vector3.Distance(verts[j], sample) < 0.001f)
						verts[j] = sample;
				}
			}
			norms.AddRange (verts);
			List<Color> cols = new List<Color> ();
			for (int i = 0; i < inds.Count/3; ++i)
			{
				int index = i*3;
				int i0 = inds[index];
				int i1 = inds[index+1];
				int i2 = inds[index+2];
				Vector3 e1 = verts[i1]-verts[i0];
				Vector3 e2 = verts[i2]-verts[i0];
				Vector3 n = Vector3.Cross(e1,e2).normalized;
				norms[i0] = n;
				norms[i1] = n;
				norms[i2] = n;
			}
			for (int i = 0; i < zones.Count; ++i)
			{
				cols.Add(new Color((float)zones[i]/100.0f,0,0));
			}

			//for (int i = 0; i < verts.Count; ++i)
			//	pts.Add(new Vector3(verts[i].x, verts[i].y, verts[i].z));
			//s.setData(pts.ToArray(), inds.ToArray(), new Color3f(0.5,0.5,0.5));
			//for (int i = 0; i < inds/3; ++i)
			{
				Mesh m = new Mesh();
				m.vertices = verts.ToArray();
				m.triangles = inds.ToArray();
				m.normals = norms.ToArray();
				m.colors = cols.ToArray();
				m.uv = uvs.ToArray();
			}
			return s;
		}
		float winding(Path test)
		{
			float total = 0;
			//(x2-x1)(y2+y1)
			for (int i = 0; i < test.Count; ++i)
			{
				int i0 = i;
				int i1 = (i+1)%test.Count;
				Vector2 p0 = test[i0];
				Vector2 p1 = test[i1];
				total += (p1.x-p0.x)*(p1.y+p0.y);
			}
			return total;

		}
		public SubMesh()
		{
		}
		public SubMesh(CSGObject s)
		{
			Mesh m = s.toMesh ();
			inds.AddRange(m.triangles);
			//Point3d[] pts = s.getVertices ();
			verts.AddRange (m.vertices);
			uvs.AddRange (m.uv);
			for (int i = 0; i < m.vertexCount; ++i)
			{
				zones.Add(m.colors[i].r*100.0f);
			}
			//Reduce ();
		}

	}
*/
	public static class PolyUtils
	{
		static bool intersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
			
			Vector2 a = p2 - p1;
			Vector2 b = p3 - p4;
			Vector2 c = p1 - p3;
			
			float alphaNumerator = b.y*c.x - b.x*c.y;
			float alphaDenominator = a.y*b.x - a.x*b.y;
			float betaNumerator  = a.x*c.y - a.y*c.x;
			float betaDenominator  = alphaDenominator;
			float epsilon = 0.0001f;

			bool doIntersect = true;
			
			if (alphaDenominator == 0 || betaDenominator == 0) {
				doIntersect = false;
			} else {
				
				if (alphaDenominator > -epsilon) {
					if (alphaNumerator < epsilon || alphaNumerator > alphaDenominator) {
						doIntersect = false;
					}
				} else if (alphaNumerator > -epsilon || alphaNumerator < alphaDenominator) {
					doIntersect = false;
				}
				
				if (doIntersect && betaDenominator > 0) {
					if (betaNumerator < epsilon || betaNumerator > betaDenominator) {
						doIntersect = false;
					}
				} else if (betaNumerator > -epsilon || betaNumerator < betaDenominator) {
					doIntersect = false;
				}
			}
			
			return doIntersect;
		}
		static bool testPoints(List<Vector3> points)
		{
			List<Vector2> pts = new List<Vector2> ();
			for (int i = 0; i < points.Count; ++i)
				pts.Add(new Vector2(points[i].x, points[i].y));
			for (int i = 0; i < pts.Count; ++i)
			{
				for (int j = i+1; j < pts.Count; ++j)
				{
					if (Vector2.Distance(pts[i],pts[j]) <= 0.0001f)
						return false;
				}
			}
			
			for (int i = 0; i < pts.Count; ++i)
			{
				Vector2 v0 = pts[i];
				Vector2 v1 = pts[(i+1)%pts.Count];
				for (int j = i+1; j < pts.Count; ++j)
				{
					Vector2 v2 = pts[j];
					Vector2 v3 = pts[(j+1)%pts.Count];
					if (intersects(v0,v1,v2,v3))
						return false;
				}
			}
			return true;
		}
		
		static float winding(List<Vector3> test)
		{
			float total = 0;
			//(x2-x1)(y2+y1)
			for (int i = 0; i < test.Count; ++i)
			{
				int i0 = i;
				int i1 = (i+1)%test.Count;
				Vector2 p0 = new Vector2(test[i0].x,test[i0].y);
				Vector2 p1 = new Vector2(test[i1].x,test[i1].y);
				total += (p1.x-p0.x)*(p1.y+p0.y);
			}
			return total;
			
		}
		public static Mesh makeMesh(List<Vector3> points, float height, float floor, Transform transform)
		{
			if (!testPoints (points))
			{
				Debug.Log("Warning: Self-intersecting polygon detected.");
				return new Mesh();
			}

			Transform t = transform;
			for (int i = 0; i < points.Count; ++i)
			{
				points[i] = t.localToWorldMatrix.MultiplyPoint(points[i]);
			}
			List<Vector3> npoints = new List<Vector3> ();
			for (int i = 0; i < points.Count; ++i)
			{
				int ind0 = i;
				int ind1 = (i+1)%points.Count;
				int ind2 = (i+2)%points.Count;
				Vector3 v0 = points[ind0];
				Vector3 v1 = points[ind1];
				Vector3 v2 = points[ind2];
				Vector3 e0 = (v1-v0).normalized;
				Vector3 e1 = (v2-v0).normalized;
				if (Vector3.Distance(e0, e1) > 0.0001f)
					npoints.Add(points[ind1]);
			}
			points = npoints;
			float total = 0;
			float perimeter = 0;
			List<float> traverse = new List<float> ();
			traverse.Add(0.0f);
			for (int i = 0; i < points.Count; ++i)
			{
				Vector3 sample = points[i];
				int ind = (i+1)%points.Count;
				total += (points[ind].x-sample.x) * (points[ind].z + sample.z);
				perimeter += Vector3.Distance(points[ind], sample);
				traverse.Add(perimeter);
				//Debug.Log(sample);
			}
			if (total > 0)
			{
				points.Reverse();
				traverse.Clear();
				perimeter = 0;
				traverse.Add(0.0f);
				for (int i = 0; i < points.Count; ++i)
				{
					Vector3 sample = points[i];
					int ind = (i+1)%points.Count;
					perimeter += Vector3.Distance(points[ind], sample);
					traverse.Add(perimeter);
					//Debug.Log(sample);
				}
			}
			float scalingfact = (perimeter - Mathf.Floor(perimeter));
			//var vertices = points.ToArray();
			
			List<Vector3> vertices = new List<Vector3> ();
			List<Vector2> uvs = new List<Vector2> ();
			//List<Vector2> uvs = new List<Vector2> ();
			
			List<Poly2Tri.PolygonPoint> _points = new List<Poly2Tri.PolygonPoint> ();
			for (int i = 0; i < points.Count; ++i)
			{
				Vector3 sample = points[i];
				_points.Add (new Poly2Tri.PolygonPoint(sample.x, sample.z));
				vertices.Add (new Vector3(sample.x,height, sample.z));
				vertices.Add (new Vector3(sample.x,-floor, sample.z));
				uvs.Add(new Vector2(sample.x, sample.z));
				uvs.Add(new Vector2(sample.x, sample.z));
				//uvs.Add (sample);
				//uvs.Add (sample);
			}
			Poly2Tri.Polygon poly = new Poly2Tri.Polygon(_points);
			
			Poly2Tri.P2T.Triangulate (poly);
			IList<Poly2Tri.DelaunayTriangle> trigs = poly.Triangles;
			
			List<int> indices = new List<int> ();
			for (int i = 0; i < trigs.Count; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					Vector2 v = new Vector2(trigs[i].Points[j].Xf, trigs[i].Points[j].Yf);
					
					Vector3 vert = new Vector3(v.x, -floor, v.y);
					for (int k = 0; k < vertices.Count; ++k)
					{
						if (Vector3.Distance(vertices[k], vert) < 0.0001f)
						{
							indices.Add(k);
							break;
						}
					}
				}
			}
			for (int i = 0; i < trigs.Count; ++i)
			{
				for (int j = 2; j >= 0; --j)
				{
					Vector2 v = new Vector2(trigs[i].Points[j].Xf, trigs[i].Points[j].Yf);
					
					Vector3 vert =new Vector3(v.x, height, v.y);
					for (int k = 0; k < vertices.Count; ++k)
					{
						if (Vector3.Distance(vertices[k], vert) < 0.0001f)
						{
							indices.Add(k);
							break;
						}
					}
				}
			}
			for (int i = 0; i < points.Count; ++i)
			{
				int ind1 = (i+1)%points.Count;
				Vector3[] v = {
					new Vector3(points[i].x,-floor, points[i].z),
					new Vector3(points[i].x,height, points[i].z),
					new Vector3(points[ind1].x,-floor, points[ind1].z),
					new Vector3(points[ind1].x,height, points[ind1].z)};
				Vector2[] u = {
					new Vector2(traverse[i], -floor),
					new Vector2(traverse[i], height),
					new Vector2(traverse[i+1], -floor),
					new Vector2(traverse[i+1], height)
				};
				//uvs.Add(new Vector2(curLen, 0.0f+floor));
				//uvs.Add(new Vector2(curLen, 0.0f-height));
				//curLen += Vector3.Distance(points[i], points[ind1]);
				//uvs.Add(new Vector2(curLen, 0.0f+floor));
				//uvs.Add(new Vector2(curLen, 0.0f-height));
				int curind = vertices.Count;
				for (int j = 0; j < v.Length; ++j)
				{
					vertices.Add (v[j]);
					uvs.Add(u[j]);
				}
				int[] inds = {0, 1, 2, 1, 3, 2};
				for (int j = 0; j < inds.Length; ++j)
				{
					indices.Add (inds[j] + curind);
				}
			}
			indices = revL (indices);		
			Mesh mesh = new Mesh ();
			for (int i = 0; i < vertices.Count; ++i)
			{
				vertices[i] = t.worldToLocalMatrix.MultiplyPoint(vertices[i]);
			}
			mesh.vertices = vertices.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.triangles = revL(indices).ToArray ();
			return mesh;
		}

		public static List<int> revL(List<int> indices)
		{
			for (int i = 0; i < indices.Count/3; ++i)
			{
				int index = i*3;
				int temp = indices[index];
				indices[index] = indices[index+2];
				indices[index+2] = temp;
			}
			return indices;
		}
		
		public static void revAr(int[] indices)
		{
			for (int i = 0; i < indices.Length/3; ++i)
			{
				int index = i*3;
				int temp = indices[index];
				indices[index] = indices[index+2];
				indices[index+2] = temp;
			}
			return;
		}
	}

}