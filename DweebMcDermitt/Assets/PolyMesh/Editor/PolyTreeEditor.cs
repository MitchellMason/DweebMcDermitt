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