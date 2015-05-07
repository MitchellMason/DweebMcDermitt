using UnityEditor;

public class MaxAtlasSize : EditorWindow
{
	int[] kSizeValues = { 512, 1024, 2048, 4096, 8192 };
	string[] kSizeStrings = { "512", "1024", "2048", "4096", "8192" };
	
	void OnGUI()
	{
		LightmapEditorSettings.maxAtlasHeight = EditorGUILayout.IntPopup("Max Atlas Size", LightmapEditorSettings.maxAtlasHeight, kSizeStrings, kSizeValues);
		LightmapEditorSettings.maxAtlasWidth = LightmapEditorSettings.maxAtlasHeight;
	}
	
	[MenuItem("Window/Max Atlas Size")]
	static void Init()
	{
		EditorWindow window = EditorWindow.GetWindow(typeof(MaxAtlasSize));
		window.Show();
	}
}

