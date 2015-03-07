//C# Example
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PosterGen
{
	public class PosterGenEditor : EditorWindow
	{
		Options options = new Options();
		PosterGenUtils utils= new PosterGenUtils();
		public List<string> buttontext;
		public string selectstring = "Select images from the Asset pane and click here";
		bool run = false;

		[MenuItem("GameObject/3D Object/Posters")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(PosterGenEditor));
		}

		void OnGUI()
		{
			updateGUI();
		}
		public void updateGUI()
		{
			try {

				if (options.Equals(null) || utils.Equals(null))
				{
					options = new Options();
					
					options.ShaderToUse = Shader.Find ("Custom/FadeoutEdge");
					utils= new PosterGenUtils();
					selectstring = "Select images from the Asset pane and click here";
					run = false;
				}
				if (options.ShaderToUse == null)
					options.ShaderToUse = Shader.Find ("Custom/FadeoutEdge");
				options.area = EditorGUILayout.FloatField("Area of each poster:", options.area);
				
				options.usemax = EditorGUILayout.Toggle("Limit dimensions:", options.usemax);
				if (options.usemax)
					options.maxdim = EditorGUILayout.FloatField("Maximum dimensions:", options.maxdim);

				options.overlap = EditorGUILayout.Toggle("Overlap posters:", options.overlap);
				if (!options.overlap)
					options.offset = EditorGUILayout.FloatField("Space between:", options.offset);

				options.rise = EditorGUILayout.FloatField("Lift posters up by:", options.rise);

				if (options.ShaderToUse.name == "Custom/FadeoutEdge")
				{
					options.border = Mathf.Abs (EditorGUILayout.FloatField("Width of fade:", options.border));
					//options.fades = true;
				}

				EditorGUILayout.LabelField("Create posters on:");

				options.xy = EditorGUILayout.Toggle("XY plane (wall):", options.xy);
				options.yz = EditorGUILayout.Toggle("YZ plane (wall):", options.yz);
				options.xz = EditorGUILayout.Toggle("XZ plane (floor):", options.xz);

				bool hasDiffuseTex = false;
				bool hasOtherTex = false;

				Shader newshader = EditorGUILayout.ObjectField(options.ShaderToUse, typeof(Shader), true) as Shader;
				if (newshader.name == "None" || newshader != options.ShaderToUse || !run)
				{
					options.ShaderToUse = newshader;
					Material mat = new Material(options.ShaderToUse);
					options.textureNames = MeshUtils.getTextures(mat);
					options.texturesToUse = new List<Texture2D>();
					buttontext = new List<string>();
					for (int i = 0; i < options.textureNames.Count; ++i)
					{
						options.texturesToUse.Add(new Texture2D(1,1));
						options.texturesToUse[i].name = "Click to set " + options.textureNames[i];
						//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
					}
					run = true;
				}

				for (int i = 0; i < options.textureNames.Count; ++i)
				{
					if (options.textureNames[i] == "_MainTex")
					{
						hasDiffuseTex = true;
					}
					else
					{	
						hasOtherTex = true;
						options.texturesToUse[i] = EditorGUILayout.ObjectField(options.texturesToUse[i], typeof(Texture2D), true) as Texture2D;

					}
				}
				if (hasOtherTex)
				{
					options.scalemaps = EditorGUILayout.FloatField("Tile other textures by:", options.scalemaps);
				}
				if (hasDiffuseTex)
				{
					if (GUILayout.Button( selectstring ))
					{
						if (!options.xy && !options.yz && !options.xz)
						{
							selectstring = "Please select one plane to create your posters on.";
						}
						else
						{
							if (options.xy)
							{
								options.xz = false;
								options.yz = false;
							}
							else if (options.yz)
							{
								options.xz = false;
							}
							selectstring = "Select images from the Asset pane and click here.";
							utils.options = options;
							if (!utils.createPictures())
							{
								selectstring = "No valid textures selected.";
							}
						}
					}
				}
				else
				{
					selectstring = "Please select a material with a diffuse texture.";
					if (GUILayout.Button( selectstring ))
					{
					}
				}
			}

			catch(System.NullReferenceException e)
			{
				//So it doesn't crash if left open during playtesting.
			}

		}
	}
}