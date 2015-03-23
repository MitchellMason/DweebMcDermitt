using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Poly2Tri;
namespace LevelEditor
{
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
				pts.Add(new Vector2(points[i].x, points[i].z));
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
				Vector2 p0 = new Vector2(test[i0].x,test[i0].z);
				Vector2 p1 = new Vector2(test[i1].x,test[i1].z);
				total += (p1.x-p0.x)*(p1.y+p0.y);
			}
			return total;
			
		}
		public static Mesh makeMesh(List<Vector3> points, float height, float floor, Transform transform)
		{
			Transform t = transform;
			for (int i = 0; i < points.Count; ++i)
			{
				Vector3 npt = t.worldToLocalMatrix.MultiplyPoint(points[i]);
			}

			if (!testPoints (points))
			{
				Debug.Log("Warning: Self-intersecting polygon detected.");
				return new Mesh();
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