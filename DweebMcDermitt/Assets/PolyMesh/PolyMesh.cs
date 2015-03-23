#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace LevelEditor
{
	public class PolyMesh : PolyTree
	{
		public Shader ShaderToUse;
		
		public List<string> textureNames = new List<string>();
		public List<Texture2D> texturesToUse = new List<Texture2D>();
		public Mesh mesh;
		public Mesh meshToUse;
		public List<Vector3> keyPoints = new List<Vector3>();
		public float height = 3;
		public float floor = 0;
		public float zIndex = 0;
		public int matindex = 0;
		public int addMode = 0;
		public bool additive = true;

		static public void CreatePolyMesh(GameObject parent)
		{
			var obj = new GameObject("PolyMesh", typeof(MeshFilter), typeof(MeshRenderer));
			UnityEditor.Undo.RegisterCreatedObjectUndo(obj, "Created PolyMesh");
			obj.transform.parent = parent.transform;
			obj.transform.localPosition = new Vector3 (0,0,0);
			obj.transform.localRotation = Quaternion.identity;
			var polyMesh = obj.AddComponent<PolyMesh>();
			polyMesh.CreateSquare (2.0f);
		}
		public PolyMesh()
		{
		}
		public override void Clean()
		{
			MeshFilter m = gameObject.GetComponent<MeshFilter> ();
			if (m == null)
				m = gameObject.AddComponent<MeshFilter>();
			MeshRenderer mr = gameObject.GetComponent<MeshRenderer> ();
			if (mr == null)
				mr = gameObject.AddComponent<MeshRenderer>();
			mr.enabled = false;
			mesh = m.sharedMesh;
			if (mesh != null)
				mesh.Clear ();
			m.sharedMesh = mesh;
			
			CSGObject csg = gameObject.GetComponent<CSGObject> ();
			if (csg == null)
				csg = gameObject.AddComponent<CSGObject>();
			if (csg.faces != null)
			{
				csg.faces.Clear ();
				csg.rootNode = new BspNode ();
			}
		}

		public SubMaterial getSubMat()
		{
			if (ShaderToUse == null)
			{
				ShaderToUse = Shader.Find("Standard");
				Material mat = new Material(ShaderToUse);
				textureNames = MeshUtils.getTextures(mat);
				texturesToUse = new List<Texture2D>();
				for (int i = 0; i < textureNames.Count; ++i)
				{
					texturesToUse.Add(new Texture2D(1,1));
					texturesToUse[i].name = "Click to set " + textureNames[i];
					//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
				}
			}
			return new SubMaterial (ShaderToUse, textureNames, texturesToUse);
		}
		public void CreateSquare(float size)
		{
			keyPoints.AddRange(new Vector3[] { new Vector3(size,0, size),
				new Vector3(size,0, -size), new Vector3(-size,0, -size),
				new Vector3(-size,0, size)} );
			type = 1;
			//BuildFinishedMesh();
		}
		public List<Vector3> GetEdgePoints()
		{
			//Build the point list and calculate curves
			var points = new List<Vector3>();
			for (int i = 0; i < keyPoints.Count; i++)
			{
				Vector3 transPt = (keyPoints[i]);
				
				points.Add(transPt);
			}
			return points;
		}

		public bool test()
		{
			
			MeshFilter m = gameObject.GetComponent<MeshFilter> ();
			if (m == null)
				return false;
			if (m.sharedMesh == null)
				return false;
			if (m.sharedMesh.triangles.Length == 0)
				return false;
			return true;
		}

		public void Construct()
		{
			//Clean ();
			var meshes = new List<PolyMesh> ();//gameObject.GetComponentsInChildren<PolyMesh> ();
			for (int i = 0; i < transform.childCount; ++i)
			{
				if (transform.GetChild(i).GetComponent<PolyMesh>() != null && transform.GetChild(i).parent == transform)
				{
					meshes.Add(transform.GetChild(i).GetComponent<PolyMesh>());
				}
			}
			for (int i = 0; i < meshes.Count; ++i)
			{
				if (meshes[i].transform.parent == transform)
					meshes[i].Clean();
			}
			for (int i = 0; i < meshes.Count; ++i)
			{
				if (meshes[i].transform.parent == transform)
					meshes[i].Construct();
			}
			CSGObject csg = GetComponent<CSGObject> ();
			if (csg == null)
				csg = gameObject.AddComponent<CSGObject>();
			
			csg.trans = transform;

			MeshFilter m = gameObject.GetComponent<MeshFilter> ();
			if (m == null)
				m = gameObject.AddComponent<MeshFilter>();
			
			MeshRenderer mr = gameObject.GetComponent<MeshRenderer> ();
			if (mr == null)
				mr = gameObject.AddComponent<MeshRenderer>();
			mr.sharedMaterial = getSubMat ().mat;
			mesh = m.sharedMesh;
			if (mesh == null)
				mesh = new Mesh();
			mesh.name = "Mesh";
			mesh.Clear ();
			List<GameObject> gobjs = new List<GameObject>();
			List<CsgOperation.ECsgOperation> addModes = new List<CsgOperation.ECsgOperation>();
			for (int i = 0; i < meshes.Count; ++i)
			{
				if (meshes[i].transform.parent == transform)
				{
					gobjs.Add(meshes[i].gameObject);
					addModes.Add(meshes[i].getAddMode());
				}
			}
			mesh = PolyUtils.makeMesh (GetEdgePoints (), height, floor, transform);
			mesh.RecalculateNormals ();
			MeshUtils.calculateMeshTangents (mesh);
			m.sharedMesh = mesh;
			if (gobjs.Count > 0)
				csg.PerformCSGDef (addModes, gobjs.ToArray());
			for (int i = 0; i < meshes.Count; ++i)
			{
				if (meshes[i].transform.parent == transform)
					meshes[i].Clean();
			}


		}
		public CsgOperation.ECsgOperation getAddMode()
		{
			switch( addMode )
			{
			
				case 0:
				return CsgOperation.ECsgOperation.CsgOper_Additive;
				case 1:
				return CsgOperation.ECsgOperation.CsgOper_Subtractive;
			}
			return CsgOperation.ECsgOperation.CsgOper_Additive;
		}

		
		public override XmlElement Output(XmlDocument xml)
		{
			XmlElement element = xml.CreateElement("PolyMesh");
			element.SetAttribute ("rotate", toStr(transform.localRotation.eulerAngles));
			element.SetAttribute ("translate", toStr(transform.localPosition));
			element.SetAttribute ("add", addMode.ToString ());
			element.SetAttribute("height", fStr(height));
			element.SetAttribute("floor", fStr(floor));
			XmlElement shad = xml.CreateElement ("Material");
			shad.SetAttribute ("shader", ShaderToUse.name);
			for (int i = 0; i < textureNames.Count; ++i)
			{
				string path = UnityEditor.AssetDatabase.GetAssetPath(texturesToUse[i]);
				XmlElement texnode = xml.CreateElement("Texture");
				texnode.SetAttribute("name", textureNames[i]);
				texnode.SetAttribute("path", path);
				shad.AppendChild(texnode);
			}
			element.AppendChild (shad);

			XmlElement points = xml.CreateElement ("KeyPoints");
			
			for (int i = 0; i < keyPoints.Count; ++i)
			{
				Vector3 pt = keyPoints[i];
				
				XmlElement keypt = xml.CreateElement("Point");
				keypt.SetAttribute("value",toStr(pt));
				points.AppendChild(keypt);
			}
			element.AppendChild (points);
			for (int i = 0; i < transform.childCount; ++i)
			{
				if (transform.GetChild(i).GetComponent<PolyMesh>() != null)
				{
					element.AppendChild(transform.GetChild(i).GetComponent<PolyMesh>().Output(xml));
				}
			}
			return element;
		}

		public override void Import(XmlNode n)
		{
			transform.localEulerAngles = strV3(n.Attributes [0].Value);
			transform.localPosition = strV3(n.Attributes [1].Value);
			addMode = int.Parse(n.Attributes [2].Value);
			height = float.Parse(n.Attributes [3].Value);
			floor = float.Parse(n.Attributes [4].Value);
			XmlNode keypts = n.LastChild;

			if (keypts.Name != "KeyPoints")
			{
				foreach(XmlNode child in n.ChildNodes)
				{
					if (child.Name == "KeyPoints")
					{
						keypts = child;
						break;
					}
				}
			}
			
			foreach(XmlNode child in keypts.ChildNodes)
			{
				if (child.Name == "Point")
					keyPoints.Add(strV3(child.Attributes[0].Value));
			}
			XmlNode shad = n.FirstChild;
			if (shad.Name != "Material")
			{
				foreach(XmlNode child in n.ChildNodes)
				{
					if (child.Name == "Material")
					{
						shad = child;
						break;
					}
				}
			}
			ShaderToUse = Shader.Find (shad.Attributes [0].Value);
			
			foreach(XmlNode child in shad.ChildNodes)
			{
				textureNames.Add(child.Attributes[0].Value);

				Texture2D tex = Resources.LoadAssetAtPath(child.Attributes[1].Value, typeof(Texture2D)) as Texture2D;
				if (tex != null)
				texturesToUse.Add(tex);
				else
					texturesToUse.Add(new Texture2D(1,1));
			}
			
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
		}

	}

}
#endif