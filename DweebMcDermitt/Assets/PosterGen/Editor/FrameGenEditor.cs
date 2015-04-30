using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PosterGen
{
	
	[CustomEditor (typeof(FrameGen)), CanEditMultipleObjects]
	public class FrameGenEditor : Editor
	{
		public string selectstring = "Generate";

		bool ran = false;
		public bool makeFrame = true;
		public int numDivides = 4;
		FrameGen fg;

		public override void OnInspectorGUI()
		{

			updateGUI();
			//EditorGUILayout.LabelField("Frame generation feature not activated.");
		}
		public void updateGUI()
		{
			try {
				fg = (FrameGen)target;

				if (fg.ShaderToUseback == null || fg.ShaderToUse == null || fg.textureNames.Count <= 0)
				{
					
					fg.ShaderToUse = Shader.Find("Bumped Diffuse");
					fg.ShaderToUseback = Shader.Find("Bumped Diffuse");
					
					
					Material mat = new Material(fg.ShaderToUse);
					fg.textureNames = MeshUtils.getTextures(mat);
					fg.texturesToUse = new List<Texture2D>();
					for (int i = 0; i < fg.textureNames.Count; ++i)
					{
						fg.texturesToUse.Add(new Texture2D(1,1));
						fg.texturesToUse[i].name = "Click to set " + fg.textureNames[i];
						//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
					}
					mat = new Material(fg.ShaderToUseback);
					
					fg.textureNamesback = MeshUtils.getTextures(mat);
					fg.texturesToUseback = new List<Texture2D>();
					for (int i = 0; i < fg.textureNamesback.Count; ++i)
					{
						fg.texturesToUseback.Add(new Texture2D(1,1));
						fg.texturesToUseback[i].name = "Click to set " + fg.textureNamesback[i];
						//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
					}
				}
				EditorGUILayout.LabelField("Frame Properties", EditorStyles.boldLabel);
				makeFrame = EditorGUILayout.Toggle("Generate frame: ",makeFrame);
				if (makeFrame)
				{
					if (fg.divides == null || fg.divides.Count <= 0)
					{
						fg.divides = new List<Vector2>();
						fg.divides.Add(new Vector2(0,0));
					}
					else if (!ran)
					{
						numDivides = fg.divides.Count;
					}

					numDivides = EditorGUILayout.IntField("Quads in frame:", numDivides);
					if (numDivides < 1 || numDivides > 10)
						numDivides = 1;

					if (fg.divides.Count != numDivides)
					{
						if (fg.divides.Count > numDivides)
						{
							while (fg.divides.Count > numDivides)
							{
								fg.divides.RemoveAt(fg.divides.Count-1);
							}
						}
						else
						{
							while (fg.divides.Count < numDivides)
							{
								Vector2 addme = fg.divides[fg.divides.Count-1] + new Vector2(0.01f,0);
								fg.divides.Add(addme);
							}
						}
					}

					EditorGUILayout.LabelField("X: position off border, Y: thickness");

					for (int i = 0; i < fg.divides.Count; ++i)
					{
						fg.divides[i] = EditorGUILayout.Vector2Field("Segment " + (i+1) + ":",fg.divides[i]);
					}
					
					EditorGUILayout.LabelField("Visual Editor");
					if (GUILayout.Button( "Open Visual Editor" ))
					{
						GridGUI gui = ScriptableObject.CreateInstance<GridGUI>();
						gui.CreateGen(fg);
					}
					
					EditorGUILayout.LabelField("Frame Material");
					Shader last = EditorGUILayout.ObjectField(fg.ShaderToUse, typeof(Shader), true) as Shader;

					if (fg.ShaderToUse != last || !ran)
					{
						//ran = true;
						if (fg.ShaderToUse != last)
						{
							fg.ShaderToUse = last;
							Material mat = new Material(fg.ShaderToUse);
							
							fg.textureNames = MeshUtils.getTextures(mat);
							fg.texturesToUse = new List<Texture2D>();
							for (int i = 0; i < fg.textureNames.Count; ++i)
							{
								fg.texturesToUse.Add(new Texture2D(1,1));
								fg.texturesToUse[i].name = "Click to set " + fg.textureNames[i];
								//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
							}
						}

					}

					for (int i = 0; i < fg.textureNames.Count; ++i)
					{
						fg.texturesToUse[i] = EditorGUILayout.ObjectField(fg.texturesToUse[i], typeof(Texture2D), true) as Texture2D;
					}
				}

				EditorGUILayout.LabelField("Backing Properties", EditorStyles.boldLabel);

				fg.border = EditorGUILayout.FloatField("Border size:", fg.border);
				
				//correct = EditorGUILayout.Toggle("Rescale UVs:", correct);
				fg.tile = EditorGUILayout.FloatField("Tile textures by:", fg.tile);

				
				EditorGUILayout.LabelField("Backing Material");
				Shader lastback = EditorGUILayout.ObjectField(fg.ShaderToUseback, typeof(Shader), true) as Shader;
				
				if (fg.ShaderToUseback != lastback || !ran)
				{
					ran = true;
					if (fg.ShaderToUseback != lastback)
					{
						fg.ShaderToUseback = lastback;
						Material mat = new Material(fg.ShaderToUseback);
						
						fg.textureNamesback = MeshUtils.getTextures(mat);
						fg.texturesToUseback = new List<Texture2D>();
						for (int i = 0; i < fg.textureNamesback.Count; ++i)
						{
							fg.texturesToUseback.Add(new Texture2D(1,1));
							fg.texturesToUseback[i].name = "Click to set " + fg.textureNamesback[i];
							//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
						}
					}
				}
				
				
				for (int i = 0; i < fg.textureNamesback.Count; ++i)
				{
					fg.texturesToUseback[i] = EditorGUILayout.ObjectField(fg.texturesToUseback[i], typeof(Texture2D), true) as Texture2D;
				}
				if (GUILayout.Button( selectstring ))
				{
					if (fg != null)
					{
						serializedObject.Update ();
						{
							FrameGen[] objs = fg.gameObject.GetComponentsInChildren<FrameGen>();
							foreach(FrameGen place in objs)
							{
								if (!place.isPoster)
									continue;
								FrameGen fgtemp = place;
								Undo.RecordObject(fgtemp, "Created Frame");
								fgtemp.setValues(fg);
								Vector2 scale = fgtemp.scaler;
								scale += new Vector2(fgtemp.border,fgtemp.border);

								if (fgtemp.obj != null)
								{
									Undo.DestroyObjectImmediate(fgtemp.obj);
								}
								
								if (fgtemp.obj2 != null)
								{
									Undo.DestroyObjectImmediate(fgtemp.obj2);
								}
								
								for (int i = 0; i < fgtemp.gameObject.transform.childCount; ++i)
								{
									GameObject testObj = fgtemp.gameObject.transform.GetChild(i).gameObject;
									if (testObj.name == fgtemp.gameObject.name + "_Backing"
									    || testObj.name == fgtemp.gameObject.name + "_Frame")
									{
										Undo.DestroyObjectImmediate(testObj);
									}
								}


								fgtemp.obj = MeshUtils.createObjFrame(MeshUtils.createBacking(scale,
								                                                  fgtemp.gameObject.name + "_Backing", true),
								                          fgtemp.texturesToUseback[0], fgtemp.ShaderToUseback,
								                          fgtemp.textureNamesback, fgtemp.texturesToUseback,
								                                 fgtemp.tile);
								Undo.RecordObject(fgtemp.gameObject.transform, "Created Frame");
								fgtemp.obj.transform.parent = fgtemp.gameObject.transform;
								fgtemp.obj.transform.position = fgtemp.gameObject.transform.position;
								fgtemp.obj.transform.localRotation = fgtemp.gameObject.transform.localRotation;

								Vector3 displace = new Vector3(0,0,-0.015f);
								//displace = Quaternion.Euler(fgtemp.rotate) * displace;
								fgtemp.obj.transform.localPosition = displace;//new Vector3(0,0,0);
								fgtemp.obj.transform.localRotation = new Quaternion();
								//fgtemp.obj.transform.Rotate (fgtemp.rotate);
								//fgtemp.gameObject.transform.Translate(-displace*4.0f);
								
								Undo.RegisterCreatedObjectUndo(fgtemp.obj, "Created Frame");

								if (fgtemp.divides.Count > 0 && makeFrame)
								{
									
									fgtemp.obj2 = MeshUtils.createObjFrame(MeshUtils.createFrame(scale,fgtemp.divides,
									                                                            fgtemp.gameObject.name + "_Frame"),
									                                      fgtemp.texturesToUse[0], fgtemp.ShaderToUse,
									                                      fgtemp.textureNames, fgtemp.texturesToUse,
									                                      fgtemp.tile);
									fgtemp.obj2.transform.parent = fgtemp.obj.transform;
									fgtemp.obj2.transform.localPosition = new Vector3(0,0,0);
									fgtemp.obj2.transform.localRotation = new Quaternion();//fgtemp.obj.transform.localRotation;
									Undo.RegisterCreatedObjectUndo(fgtemp.obj2, "Created Frame");
								}
							}
						}
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