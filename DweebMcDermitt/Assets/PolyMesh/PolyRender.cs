
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LevelEditor
{
	public class PolyRender : MonoBehaviour
	{
		static public void CreatePolyRender(GameObject parent, List<SubMaterial> m)
		{
			var obj = new GameObject("PolyRender");
			obj.transform.parent = parent.transform;
			obj.transform.localPosition = new Vector3 (0,0,0);
			obj.transform.localRotation = Quaternion.identity;
			var polyRender = obj.AddComponent<PolyRender>();
			polyRender.MakeMesh (m);
		}
		public void MakeMesh(List<SubMaterial> m)
		{
			for (int i = 0; i < m.Count; ++i)
			{
				var obj = new GameObject("PolySub_" + (i+1).ToString(), typeof(MeshFilter), typeof(MeshRenderer));
				obj.transform.parent = transform;
				obj.transform.localPosition = new Vector3 (0,0,0);
				obj.transform.localRotation = Quaternion.identity;
				var mr = obj.GetComponent<MeshRenderer>();
				mr.sharedMaterial = m[i].mat;
				var mf = obj.GetComponent<MeshFilter>();
				var mesh = new Mesh();
				mesh.vertices = m[i].verts.ToArray();
				mesh.uv = m[i].uvs.ToArray();
				mesh.triangles = m[i].indices.ToArray();
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				MeshUtils.calculateMeshTangents(mesh);
				mf.sharedMesh = mesh;
			}
		}
	}

}
#endif