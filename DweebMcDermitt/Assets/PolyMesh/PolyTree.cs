using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstructiveSolidGeometry;

namespace LevelEditor
{
	public class PolyTree : MonoBehaviour
	{
		public float gfloor = 0.0f, gceil = 3.0f;
		public MeshCollider meshCollider;
		public void BuildMesh()
		{
			int count = 0;
			CSG s = new CSG();
			bool init = false;
			List<SubMaterial> subs = new List<SubMaterial> ();
			for (int j = 0; j < transform.childCount; ++j)
			{
				GameObject gobj1 = transform.GetChild (j).gameObject;

				if (gobj1.GetComponent<PolyMesh>() == null)
					continue;
				
				PolyMesh smesh = gobj1.GetComponent<PolyMesh>();
				if (!init)
				{
					smesh.matindex = count++;
					subs.Add(smesh.getSubMat());
					s = smesh.getSub().getSolid();
					init = true;
				}
				else
				{
					smesh.matindex = count++;
					subs.Add(smesh.getSubMat());
					CSG s2 = smesh.getSub().getSolid();
					var tmp = s.union(s2);
					s = tmp;
				}
				for (int i = 0; i < gobj1.transform.childCount; ++i)
				{
					GameObject gobj = gobj1.transform.GetChild (i).gameObject;
					if (gobj.GetComponent<PolyAdjust>() != null)
					{
						PolyAdjust adj = gobj.GetComponent<PolyAdjust>();
						
						subs.Add(adj.getSubMat());
						CSG s2 = adj.getSub().getSolid();
						if (adj.addMode == 0)
						{
							var tmp = s.subtract(s2);//modeller.getDifference();
							s = tmp;
						}
						else if (adj.addMode == 1)
						{
							var tmp = s.union(s2);//modeller.getUnion();
							s = tmp;
						}
						else if (adj.addMode == 2)
						{
							var tmp = s.intersect(s2);
							s = s.subtract(tmp);
							tmp.setZone(adj.matindex);
							s = s.subtract(tmp);
							s = s.union(tmp);
						}
					}
				}
			}
			for (int i = 0; i < transform.childCount; ++i)
			{
				GameObject gobj = transform.GetChild (i).gameObject;
				if (gobj.GetComponent<PolyAdjust>() != null)
				{
					PolyAdjust adj = gobj.GetComponent<PolyAdjust>();
					adj.matindex = count++;
					subs.Add(adj.getSubMat());
					CSG s2 = adj.getSub().getSolid();
					if (adj.addMode == 0)
					{
						var tmp = s.subtract(s2);//modeller.getDifference();
						s = tmp;
					}
					else if (adj.addMode == 1)
					{
						var tmp = s.union(s2);//modeller.getUnion();
						s = tmp;
					}
					else if (adj.addMode == 2)
					{
						var tmp = s2.intersect(s);
						//s = s.subtract (tmp);
						s = s.overwrite(tmp, adj.matindex);
						//s = s.subtract(tmp);
						//s = s.union(tmp);
					}
				}
			}
			List<Vector3> vertices = new List<Vector3>();
			SubMesh sub = new SubMesh (s);
			vertices = sub.verts;
			//Find the mesh (create it if it doesn't exist)
			//var meshRenderer = GetComponent<MeshRenderer> ();
			//var meshFilter = GetComponent<MeshFilter>();
			var mesh = new Mesh ();
			mesh.name = "RoomRender";
			//Update the mesh
			mesh.Clear();
			mesh.vertices = vertices.ToArray();

			//mesh.colors = colors;
			//mesh.uv = sub.uvs.ToArray();
			int[] ind = sub.inds.ToArray();
			PolyUtils.revAr(ind);
			mesh.triangles = ind;
			//mesh.subMeshCount = subs.Count;
			//Material[] sharedMats = new Material[subs.Count];
			List<int> contains = new List<int>(ind);
			for (int i = 0; i < subs.Count; ++i)
			{
				List<int> subinds = new List<int>();
				for (int j = 0; j < ind.Length; ++j)
				{
					int index = ind[j];
					if ((int)Mathf.Round(sub.zones[index]) == i)
					{
						subinds.Add(index);
						contains.Remove(index);
					}
				}
				//mesh.SetIndices(subinds.ToArray(), MeshTopology.Triangles, i);
				//sharedMats[i] = subs[i].mat;
				subs[i].uvs = sub.uvs;
				subs[i].verts = sub.verts;
				subs[i].indices = subinds;

			}
			for (int i = 0; i < contains.Count; ++i)
			{
				Debug.Log(sub.zones[contains[i]]);
			}
			PolyRender[] renders = gameObject.GetComponentsInChildren<PolyRender> ();
			for (int i = 0; i < renders.Length; ++i)
			{
				DestroyImmediate(renders[i].gameObject);
			}
			PolyRender.CreatePolyRender (gameObject, subs);
			//meshRenderer.sharedMaterials = sharedMats;
			mesh.RecalculateNormals();
			mesh.Optimize();
			//MeshUtils.calculateMeshTangents(mesh);
			
			if (meshCollider == null)
			{
				meshCollider = gameObject.AddComponent<MeshCollider>();
			}
			Mesh collider = meshCollider.sharedMesh;
			if (collider == null)
			{
				collider = new Mesh();
				collider.name = "PolySprite_Collider";
			}
			collider.vertices = mesh.vertices ;
			collider.triangles = mesh.triangles ;
			collider.RecalculateBounds();
			collider.RecalculateNormals();
			collider.Optimize();
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = collider;
			


		}
		
		public void BuildFinishedMesh()
		{
			BuildMesh ();
		}
		public void AddRoom()
		{
			PolyMesh.CreatePolyMesh (gameObject);
		}
		public void AddAdj()
		{
			PolyAdjust.CreatePolyAdjust(gameObject);
		}
		public void Export(string fileName)
		{
			List<string> lines = new List<string> ();
			var meshFilter = GetComponent<MeshFilter>();
			var mesh = meshFilter.sharedMesh;
			Vector3[] verts = mesh.vertices;
			for (int i = 0; i < verts.Length; ++i)
			{
				lines.Add("v " + verts[i].x + " " + verts[i].y + " " + verts[i].z);
			}
			int[] inds = mesh.triangles;
			
			for (int i = 0; i < inds.Length/3; ++i)
			{
				int index = i*3;
				lines.Add("f " + (inds[index]+1) + " " + (inds[index+1]+1) + " " + (inds[index+2]+1));
			}
			string path = System.IO.Path.GetFullPath(fileName);
			System.IO.File.WriteAllLines(fileName, lines.ToArray());
			Debug.Log ("File written to " + path);
		}
	}

}