using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace LevelEditor
{
	[CustomEditor(typeof(PolyTree)), CanEditMultipleObjects]
	public class PolyTreeEditor : Editor
	{
		
		string fileName = "out.obj";	
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Add Room"))
			{
				polyTree.AddRoom();
			}
			if (GUILayout.Button("Add Global Adjustor"))
			{
				polyTree.AddAdj();
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
		[MenuItem("GameObject/3D Object/Room", false, 1000)]
		static void CreatePolyTree()
		{
			var obj = new GameObject("PolyTree", typeof(MeshFilter), typeof(MeshRenderer));
			var polyTree = obj.AddComponent<PolyTree>();

			//polyTree.CreateSquare(polyMesh, 0.5f);
			polyTree.AddRoom ();
		}

	}
}