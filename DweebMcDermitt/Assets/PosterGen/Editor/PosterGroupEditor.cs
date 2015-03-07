using UnityEngine;
using UnityEditor;
using System.Collections;

namespace PosterGen
{
	[CustomEditor (typeof(PosterGroup))]
	public class PosterGroupEditor : Editor
	{
		
		PosterGroup group;
		PosterGenUtils utils = new PosterGenUtils();
		public string selectstring = "Select images from the Asset pane and click here";

		public override void OnInspectorGUI()
		{
			
			group = (PosterGroup)target;

			Options options = group.options;
			group.reposition = EditorGUILayout.Toggle("Reposition posters:", group.reposition);
			if (group.reposition)
			{
				options.overlap = EditorGUILayout.Toggle("Overlap posters:", options.overlap);
				if (!options.overlap)
					options.offset = EditorGUILayout.FloatField("Space between:", options.offset);
				options.rise = EditorGUILayout.FloatField("Lift posters up by:", options.rise);

			}

			if (options.ShaderToUse.name == "Custom/FadeoutEdge")
			{
				options.border = Mathf.Abs (EditorGUILayout.FloatField("Width of fade:", options.border));

			}
			
			bool hasOtherTex = false;
			for (int i = 0; i < options.textureNames.Count; ++i)
			{
				if (options.textureNames[i] != "_MainTex")
				{	
					hasOtherTex = true;
					options.texturesToUse[i] = EditorGUILayout.ObjectField(options.texturesToUse[i], typeof(Texture2D), true) as Texture2D;
				}
			}
			
			if (hasOtherTex)
			{
				options.scalemaps = EditorGUILayout.FloatField("Tile other textures:", options.scalemaps);
			}
			Transform trans = group.transform;

			if (GUILayout.Button( "Adjust posters" ))
			{
				Undo.RecordObject(group, "Adjusted posters");

				Vector2 last = new Vector2(0,0);
				float transform = 0;
				for (int i = 0; i < trans.childCount; ++i)
				{
					Transform t = trans.GetChild(i);
					GameObject gobj = t.gameObject;
					FrameGen fg = gobj.GetComponent<FrameGen>();
					if (!fg.isPoster)
						continue;

					Material temp = (gobj.GetComponent<MeshRenderer>() as MeshRenderer).sharedMaterial;
					Undo.RecordObject(temp, "Adjusted posters");
					if (temp.HasProperty("_Fade"))
					{
						temp.SetFloat("_Fade", options.border);
					}
					
					for (int j = 0; j < options.textureNames.Count; ++j)
					{
						if (options.texturesToUse[j].width > 1 || options.texturesToUse[j].height > 1)
						{
							temp.SetTexture(options.textureNames[j], options.texturesToUse[j]);
							temp.SetTextureScale(options.textureNames[j], new Vector2(options.scalemaps*(fg.scaler.x*2.0f),options.scalemaps*(fg.scaler.y*2.0f)));
						}
					} 
					if (group.reposition)
					{
						Undo.RecordObject(t, "Adjusted posters");
						Vector2 scaler = fg.scaler;
						transform += last.x + scaler.x;
						transform += options.offset;
						t.transform.localPosition = new Vector3(0,0,0);
						t.transform.localRotation = new Quaternion();
						if (options.xy)
						{
							if (!options.overlap)
							{
								t.Translate(new Vector3(transform,options.rise,0));
							}
						}
						
						else if (options.yz)
						{
							if (!options.overlap)
							{
								t.Translate(new Vector3(0,options.rise,transform));
							}
							t.Rotate(new Vector3(0,90,0));
							
						}
						else if (options.xz)
						{
							if (!options.overlap)
							{
								t.Translate(new Vector3(transform,0,options.rise));
							}
							t.Rotate(new Vector3(-90,0,0));

						}
						last = scaler;
					}
				}
			}
				
		}
	}
}

