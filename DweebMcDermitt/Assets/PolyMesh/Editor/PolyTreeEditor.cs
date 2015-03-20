using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace LevelEditor
{
	[CustomEditor(typeof(PolyTree)), CanEditMultipleObjects]
	public class PolyTreeEditor : Editor
	{
		string fileName = "out.xml";	
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Add Room"))
			{
				polyTree.AddRoom();
			}
			EditorGUILayout.LabelField ("Build options:");
			/*polyTree.gfloor = EditorGUILayout.FloatField ("Floor",polyTree.gfloor);
			polyTree.gceil = EditorGUILayout.FloatField ("Ceiling",polyTree.gceil);*/
			if (GUILayout.Button("Build"))
			{
				polyTree.BuildFinishedMesh();
			}
			EditorGUILayout.LabelField ("Export options:");
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Path:");
			fileName = EditorGUILayout.TextField (fileName);
			if (GUILayout.Button("Export"))
			{
				polyTree.Export(fileName);
			}
			EditorGUILayout.EndHorizontal ();
		}
		
		PolyTree polyTree
		{
			get { return (PolyTree)target; }
		}

	}
	public class PolyTreeWindow: EditorWindow
	{
		string selectstring = "Create a new room";	
		[MenuItem("GameObject/3D Object/Room")]
		static void CreatePolyTree()
		{
			EditorWindow.GetWindow(typeof(PolyTreeWindow));
		}
		/*List<Vector3> ReadPts(JSONNode parent)
		{
			List<Vector3> pts = new List<Vector3> ();
			foreach(JSONNode child in parent["Keypoints"].Children)
			{
				Vector3 pt = new Vector3();
				for (int i = 0; i < 3; ++i)
				{
					pt[i] = child[i].AsFloat;
				}
				pts.Add(pt);
			}
			return pts;
		}
		void Recurse(JSONNode child, Transform parent)
		{
			if (child["PolyMesh"] != null)
			{
				Vector3 pos = new Vector3();
				Vector3 rot = new Vector3();
				for (int i = 0; i < 3; ++i)
				{
					pos[i] = child["PolyMesh"]["Position"][i].AsFloat;
					rot[i] = child["PolyMesh"]["Rotation"][i].AsFloat;
				}
				var obj = new GameObject("PolyMesh");
				obj.transform.parent = parent;
				obj.transform.localPosition = pos;
				obj.transform.localEulerAngles = rot;
				var polyMesh = obj.AddComponent<PolyMesh>();
				List<Vector3> pts = ReadPts(child["PolyMesh"]);
				polyMesh.keyPoints = pts;
				polyMesh.height = child["PolyMesh"]["Height"].AsFloat;
				polyMesh.floor = child["PolyMesh"]["Floor"].AsFloat;
				
				polyMesh.ShaderToUse = Shader.Find(child["PolyMesh"]["Shader"]);
				
				Material mat = new Material(polyMesh.ShaderToUse);
				polyMesh.textureNames = MeshUtils.getTextures(mat);

				for (int i = 0; i < polyMesh.textureNames.Count; ++i)
				{
					polyMesh.texturesToUse.Add(new Texture2D(1,1));
					var asset = AssetDatabase.LoadMainAssetAtPath(child["PolyMesh"]["TexturesToUse"][i]);
					{
						if (asset.GetType() == typeof(Texture2D))
						{
							polyMesh.texturesToUse[i] = asset as Texture2D;
							continue;
						}
					}
				}
			}
		}*/
		void OnGUI()
		{
			if (GUILayout.Button("Import"))
			{
				var file = EditorUtility.OpenFilePanel("Load xml file containing room", "", "xml");
				var input = new XmlDocument(); 
				input.Load(file);
				var child = input.FirstChild;
				if (child.Name == "PolyTree")
				{
					var obj = new GameObject("PolyTreeImport");
					Undo.RegisterCreatedObjectUndo(obj, "Imported PolyTree");
					var polyTree = obj.AddComponent<PolyTree>();
					polyTree.Import(child);
				}
				/*var input = JSONClass.LoadFromFile(file);
				if (input["PolyTree"] != null)
				{
					var obj = new GameObject("PolyTreeImport");
					Undo.RegisterCreatedObjectUndo(obj, "Imported PolyTree");
					var polyTree = obj.AddComponent<PolyTree>();
					Vector3 pos = new Vector3();
					Vector3 rot = new Vector3();
					for (int i = 0; i < 3; ++i)
					{
						pos[i] = input["PolyTree"]["Position"][i].AsFloat;
						rot[i] = input["PolyTree"]["Rotation"][i].AsFloat;
					}
					obj.transform.localEulerAngles = rot;
					obj.transform.localPosition = pos;
					foreach(JSONNode child in input["PolyTree"]["Children"].Children)
					{
						Recurse(child, obj.transform);
					}
					polyTree.BuildMesh();
				}*/
			}
			EditorGUILayout.Space();
			if (GUILayout.Button( selectstring ))
			{
				var obj = new GameObject("PolyTree");
				Undo.RegisterCreatedObjectUndo(obj, "Created PolyTree");
				var polyTree = obj.AddComponent<PolyTree>();
				
				//polyTree.CreateSquare(polyMesh, 0.5f);
				polyTree.AddRoom ();
			}
		}
	}
}