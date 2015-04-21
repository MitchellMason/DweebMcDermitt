using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(CameraChoice))]
public class CameraChoiceEditor : Editor {

	public override void OnInspectorGUI () {
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("No_Oculus"), true);
		serializedObject.ApplyModifiedProperties();
		if (GUI.changed)
		{
			choice.set();
		}
	}
	CameraChoice choice
	{
		get { return (CameraChoice)target; }
	}
}
