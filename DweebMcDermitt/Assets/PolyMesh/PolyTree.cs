
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace LevelEditor
{
	public class PolyTree : MonoBehaviour
	{
		public float gfloor = 0.0f, gceil = 3.0f;
		public MeshCollider meshCollider;
		public int type = 0;
		public void BuildMesh()
		{
			Clean ();
			var meshes = new List<PolyMesh> ();
			for (int i = 0; i < transform.childCount; ++i)
			{
				if (transform.GetChild(i).GetComponent<PolyMesh>() != null)
				{
					meshes.Add(transform.GetChild(i).GetComponent<PolyMesh>());
				}
			}
			for (int i = 0; i < meshes.Count; ++i)
			{
				meshes[i].Clean();
			}
			
			Mesh m = GetComponent<MeshFilter> ().sharedMesh;
			if (m == null)
				m = new Mesh();
			m.name = "Mesh";
			m.Clear ();
			for (int i = 0; i < meshes.Count; ++i)
			{
				if (meshes[i].transform.parent == transform)
					meshes[i].Construct();
			}
			List<GameObject> gobjs = new List<GameObject>();
			List<CsgOperation.ECsgOperation> addModes = new List<CsgOperation.ECsgOperation>();
			for (int i = 0; i < meshes.Count; ++i)
			{
				//if (meshes[i].transform.parent == transform)
				{
				gobjs.Add(meshes[i].gameObject);
				addModes.Add(meshes[i].getAddMode());
				}
			}
			CSGObject csg = GetComponent<CSGObject> ();
			if (csg == null)
				csg = gameObject.AddComponent<CSGObject>();

			csg.PerformCSG (addModes, gobjs.ToArray());
			//if (m != null)
			{
				for (int i = 0; i < m.subMeshCount; ++i)
				{
					int[] spots = m.GetIndices(i);
					PolyUtils.revAr (spots);
					m.SetIndices(spots, MeshTopology.Triangles, i);
				}
				m.RecalculateBounds ();
				m.RecalculateNormals ();
				MeshUtils.calculateMeshTangents (m);
				MeshCollider col = GetComponent<MeshCollider> ();
				if (col == null)
				{
					col = gameObject.AddComponent<MeshCollider>();
				}
				col.sharedMesh = GetComponent<MeshFilter>().sharedMesh;
				for (int i = 0; i < meshes.Count; ++i)
				{
					//if (meshes[i].transform.parent == transform)
					meshes[i].Clean();
				}
			}
			UnityEditor.Unwrapping.GenerateSecondaryUVSet (m);
		}
		
		public void BuildFinishedMesh()
		{
			if (transform.parent == null)
			{
				BuildMesh ();
			}
			else
			{
				transform.parent.gameObject.GetComponent<PolyTree>().BuildFinishedMesh();
			}
		}
		public void AddRoom()
		{
			PolyMesh.CreatePolyMesh (gameObject);
		}
		public void AddAdj()
		{
		}
		public string toStr(Vector3 input)
		{
			string str = "(";
			float scale = 100000.0f;
			input = input * scale;
			str += Mathf.Round(input.x)/scale + ", ";
			str += Mathf.Round(input.y)/scale + ", ";
			str += Mathf.Round(input.z)/scale + ")";
			return str;
		}
		public virtual void Clean()
		{

			MeshFilter m = GetComponent<MeshFilter> ();
			if (m == null)
				m = gameObject.AddComponent<MeshFilter>();

			
			MeshRenderer mr = gameObject.GetComponent<MeshRenderer> ();
			if (mr == null)
				mr = gameObject.AddComponent<MeshRenderer>();
			var mesh = m.sharedMesh;
			if (mesh == null)
				mesh = new Mesh();
			else
			{
				mesh.Clear ();
				mesh = new Mesh();
			}
			mesh.name = "Mesh";
			m.sharedMesh = mesh;
			m.sharedMesh.name = "Mesh";
			CSGObject csg = GetComponent<CSGObject> ();
			if (csg == null)
				csg = gameObject.AddComponent<CSGObject>();
		}
		public virtual XmlElement Output(XmlDocument xml)
		{
			XmlElement element = xml.CreateElement("PolyTree");
			element.SetAttribute ("rotate", toStr(transform.localRotation.eulerAngles));
			element.SetAttribute ("translate", toStr(transform.localPosition));
			
			//var meshes = new List<PolyMesh> ();//gameObject.GetComponentsInChildren<PolyMesh> ();
			for (int i = 0; i < transform.childCount; ++i)
			{
				if (transform.GetChild(i).GetComponent<PolyMesh>() != null)
				{
					element.AppendChild(transform.GetChild(i).GetComponent<PolyMesh>().Output(xml));
				}
			}
			return element;
		}
		public static Vector3 strV3(string blockDataString) {
			//gets and returns the vector3 from a blockDataString
			string returnstring;
			var startChar = 0;
			
			returnstring = blockDataString;
			//Debug.Log (returnstring);
			
			startChar = 1;
			var endChar = returnstring.IndexOf(",");
			var lastEnd = endChar;
			var returnx = System.Convert.ToDecimal(returnstring.Substring(startChar,endChar-1));
			
			startChar = lastEnd+1;
			endChar = returnstring.IndexOf(",", startChar);
			lastEnd = endChar;
			var returny = System.Convert.ToDecimal(returnstring.Substring(startChar,endChar-startChar));
			
			startChar = lastEnd+1;
			endChar = returnstring.Length-startChar;
			var returnz = System.Convert.ToDecimal(returnstring.Substring(startChar,endChar-1));
			
			//print(returnx + "  " + returny + "  " + returnz);
			
			var returnVector3 = new Vector3((float)returnx,(float)returny,(float)returnz);
			
			return returnVector3;
		}

		public virtual void Import(XmlNode n)
		{
			transform.localEulerAngles = strV3(n.Attributes [0].Value);
			transform.localPosition = strV3(n.Attributes [1].Value);
			//Debug.Log (transform.localPosition);
			foreach(XmlNode child in n.ChildNodes)
			{
				if (child.Name == "PolyMesh")
				{
					var obj = new GameObject("PolyMesh");
					obj.transform.parent = transform;
					
					var polyMesh = obj.AddComponent<PolyMesh>();
					polyMesh.Import(child);
				}
			}
			BuildFinishedMesh ();
		}
		public virtual void Export(string fileName)
		{
			string path = System.IO.Path.GetFullPath(fileName);
			XmlDocument xml = new XmlDocument();
			xml.AppendChild(Output(xml));
			string str = xml.OuterXml;
			str = str.Replace ("><", ">\n<");

			string[] lines = str.Split ('\n');
			int indents = 0;
			for (int i = 0; i < lines.Length; ++i)
			{
				int counter = indents;
				if (lines[i].StartsWith("</"))
				{
					--indents;
					--counter;
				}
				else if (lines[i].StartsWith("<"))
					++indents;
				if (lines[i].EndsWith("/>"))
					--indents;
				for (int j = 0; j < counter; ++j)
				{
					lines[i] = "\t" + lines[i];
				}
			}

			System.IO.File.WriteAllLines(path, lines);
			Debug.Log ("File written to " + path);
		}
	}

}
#endif