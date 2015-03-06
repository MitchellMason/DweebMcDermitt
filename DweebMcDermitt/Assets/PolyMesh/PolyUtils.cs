using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstructiveSolidGeometry;
using Poly2Tri;

namespace LevelEditor
{
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
	public class PlaneEdges
	{
		public float div = 1.0f;
		public Vector3 b0, b1, n, offset;
		public float dist = 0.0f;
		public List<Poly> polys = new List<Poly>();
		public UnityEngine.Plane p;
		public PlaneEdges(){}
		public PlaneEdges(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			p = new UnityEngine.Plane (v0, v1, v2);
			n = p.normal;
			b0 = new Vector3 (0.0f, 1.0f, 0.0f).normalized; b1 = new Vector3 (0.0f, 0.0f, 1.0f).normalized;
			if (Mathf.Abs(n.z) > 0.001f)
			{
				b0 = new Vector3(1.0f,0.0f,n.y/n.z).normalized;
				b1 = new Vector3(0.0f,1.0f,n.x/n.z).normalized;
			}
			else if (Mathf.Abs(n.y) > 0.001f)
			{
				b0 = new Vector3 (1.0f, -n.x/n.y, 0.0f).normalized;
				b1 = new Vector3 (0.0f, 0.0f, 1.0f).normalized;
			}
			else
				n = new Vector3(Mathf.Sign(n.x),0.0f,0.0f);
			dist = p.distance;
			offset = n * p.distance;
		}
		public void RemoveDuplicates()
		{
			for (int i = 0; i < polys.Count; ++i)
			{
				Poly poly = polys[i];
				for (int j = i+1; j < polys.Count; ++j)
				{
					Poly test = polys[j];
					int matches = 0;
					for (int l = 0; l < test.boundary.Count; ++l)
					{
						Vector3 testsam = test.boundary[l];
						for (int m = 0; m < poly.boundary.Count; ++m)
						{
							Vector3 sample = poly.boundary[m];
							if (Vector3.Distance(testsam, sample) < 0.01f)
								++matches;
						}
					}
					if (matches >= 3)
					{
						polys.RemoveAt(j);
						--j;
					}
				}
			}
		}
		public Paths polysToPaths()
		{
			Paths paths = new Paths ();
			for (int i = 0; i < polys.Count; ++i)
			{
				Path path = new Path();
				for (int j = 0; j < polys[i].boundary.Count/3; ++j)
				{
					int index = j * 3;
					
					List<Vector3> pts = new List<Vector3>();
					for (int k = 0; k < 3; ++k)
					{
						//Vector3 pt = new Vector3(, trig.Points[j].Yf, 0.0f);
						Vector3 pt = (polys[i].boundary[index+k]);
						pts.Add(pt);
					}

					if (Vector3.Distance(Vector3.Cross(pts[2]-pts[0], pts[1]-pts[0]).normalized, n) > 0.0001f)
					{
						Vector3 temp = pts[0];
						pts[0] = pts[1];
						pts[1] = temp;
					}
					for (int k = 0; k < pts.Count; ++k)
					{
						Vector3 sam = pts[k];
						Vector3 sam1 = sam + offset;
						float u = Vector3.Dot(b0, sam1);
						float v = Vector3.Dot(b1, sam1);
						Vector3 sam2 = (u * b0 + v * b1);
						path.Add(new Vector2(u,v));
					}
				}
				paths.Add(path);
			}
			return paths;
		}
		public List<Vector3> pathsToPolys(Poly2Tri.Polygon poly)
		{
			List<Vector3> path = new List<Vector3> ();
			for (int i = 0; i < poly.Triangles.Count; ++i)
			{
				DelaunayTriangle trig = poly.Triangles[i];
				List<Vector3> pts = new List<Vector3>();
				for (int j = 0; j < 3; ++j)
				{
					//Vector3 pt = new Vector3(, trig.Points[j].Yf, 0.0f);
					Vector3 pt = (trig.Points[j].Xf * b0 + trig.Points[j].Yf * b1) - offset;


					pts.Add(pt);
				}
				if (Vector3.Distance(Vector3.Cross(pts[1]-pts[0], pts[2]-pts[0]).normalized, n) > 0.0001f)
				{
					Vector3 temp = pts[0];
					pts[0] = pts[1];
					pts[1] = temp;
				}
				path.AddRange(pts);
			}
			return path;
		}
	}
	public class SubMesh
	{

		public List<Vector3> verts = new List<Vector3>();
		public List<Vector3> norms = new List<Vector3>();
		public List<Vector2> uvs = new List<Vector2>();
		public List<float> zones = new List<float>();
		public List<int> inds = new List<int>();
		List<PlaneEdges> planes = new List<PlaneEdges> ();
		public CSG getSolid()
		{
			CSG s = new CSG ();
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
				s = CSG.fromMeshNT(m);
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
		public SubMesh(CSG s)
		{
			Mesh m = s.toMesh ();
			inds.AddRange(m.triangles);
			//Point3d[] pts = s.getVertices ();
			/*for (int i = 0; i < pts.Length; ++i)
			{
				verts.Add(new Vector3((float)pts[i].x,(float)pts[i].y,(float)pts[i].z));
			}*/
			verts.AddRange (m.vertices);
			uvs.AddRange (m.uv);
			for (int i = 0; i < m.vertexCount; ++i)
			{
				zones.Add(m.colors[i].r*100.0f);
			}
			//Reduce ();
		}

	}
	public static class PolyUtils
	{
		static bool intersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
			
			Vector2 a = p2 - p1;
			Vector2 b = p3 - p4;
			Vector2 c = p1 - p3;
			
			float alphaNumerator = b.y*c.x - b.x*c.y;
			float alphaDenominator = a.y*b.x - a.x*b.y;
			float betaNumerator  = a.x*c.y - a.y*c.x;
			float betaDenominator  = alphaDenominator; /*2013/07/05, fix by Deniz*/

			float epsilon = 0.00001f;

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

		public static SubMesh makeSub(List<Vector3> points, float height, float floor)
		{
			if (!testPoints (points))
			{
				Debug.Log("Warning: Self-intersecting polygon detected.");
				return new SubMesh();
			}


			float total = 0;
			float perimeter = 0;
			List<float> traverse = new List<float> ();
			traverse.Add(0.0f);
			for (int i = 0; i < points.Count; ++i)
			{
				Vector2 sample = points[i];
				int ind = (i+1)%points.Count;
				total += (points[ind].x-sample.x) * (points[ind].y + sample.y);
				perimeter += Vector2.Distance(points[ind], sample);
				traverse.Add(perimeter);
			}
			float scalingfact = (perimeter - Mathf.Floor(perimeter));
			if (total < 0)
			{
				points.Reverse();
			}
			//var vertices = points.ToArray();
			
			List<Vector3> vertices = new List<Vector3> ();
			List<Vector2> uvs = new List<Vector2> ();
			//List<Vector2> uvs = new List<Vector2> ();
			
			List<Poly2Tri.PolygonPoint> _points = new List<Poly2Tri.PolygonPoint> ();
			for (int i = 0; i < points.Count; ++i)
			{
				Vector2 sample = points[i];
				_points.Add (new Poly2Tri.PolygonPoint(sample.x, sample.y));
				vertices.Add (new Vector3(sample.x, sample.y, -height));
				vertices.Add (new Vector3(sample.x, sample.y, floor));
				uvs.Add(new Vector2(sample.x, sample.y));
				uvs.Add(new Vector2(sample.x, sample.y));
				//uvs.Add (sample);
				//uvs.Add (sample);
			}
			Poly2Tri.Polygon poly = new Poly2Tri.Polygon(_points);
			
			Poly2Tri.P2T.Triangulate (poly);
			IList<Poly2Tri.DelaunayTriangle> trigs = poly.Triangles;
			
			List<int> indices = new List<int> ();
			for (int i = 0; i < trigs.Count; ++i)
			{
				for (int j = 2; j >= 0; --j)
				{
					Vector2 v = new Vector2(trigs[i].Points[j].Xf, trigs[i].Points[j].Yf);
					
					Vector3 vert = new Vector3(v.x, v.y, floor);
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
				for (int j = 0; j < 3; ++j)
				{
					Vector2 v = new Vector2(trigs[i].Points[j].Xf, trigs[i].Points[j].Yf);
					
					Vector3 vert =new Vector3(v.x, v.y, -height);
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
					new Vector3(points[i].x, points[i].y, floor),
					new Vector3(points[i].x, points[i].y, -height),
					new Vector3(points[ind1].x, points[ind1].y, floor),
					new Vector3(points[ind1].x, points[ind1].y, -height)};
				Vector2[] u = {
					new Vector2(traverse[i], floor),
					new Vector2(traverse[i], -height),
					new Vector2(traverse[i+1], floor),
					new Vector2(traverse[i+1], -height)
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
			SubMesh sub = new SubMesh ();
			sub.inds = indices;
			sub.verts = vertices;
			sub.uvs = uvs;
			return sub;
		}

		public static CSG reducePolys(CSG input)
		{
			SubMesh sub = new SubMesh(input);
			//sub.Reduce ();
			return sub.getSolid ();
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
