using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace LevelEditor
{
	[CustomEditor(typeof(PolyMesh)), CanEditMultipleObjects]
	public class PolyMeshEditor : Editor
	{
		enum State { Hover, Drag, DragSelected, RotateSelected, ScaleSelected, Extrude }

		const float clickRadius = 0.12f;
		bool editing = true;
		bool tabDown;
		State state;
		bool ran = false;

		List<Vector3> keyPoints;

		Matrix4x4 worldToLocal;
		Quaternion inverseRotation;
		
		Vector3 mousePosition;
		Vector3 clickPosition;
		Vector3 screenMousePosition;
		MouseCursor mouseCursor = MouseCursor.Arrow;
		float snap;

		int dragIndex;
		List<int> selectedIndices = new List<int>();
		int nearestLine;
		Vector3 splitPosition;
		bool extrudeKeyDown;
		bool doExtrudeUpdate;
		bool multiple = false;

		#region Inspector GUI
		public void OnDisable()
		{
			Tools.hidden = false;
		}
		public void OnDestroy()
		{
			Tools.hidden = false;
		}
		public override void OnInspectorGUI()
		{
			if (serializedObject.isEditingMultipleObjects)
				multiple = true;
			else
				multiple = false;
			if (!multiple)
			{
				if (target == null)
					return;

				if (polyMesh.keyPoints.Count == 0)
					polyMesh.CreateSquare(0.5f);
				serializedObject.Update();
				
				if (serializedObject.ApplyModifiedProperties() ||
				    (Event.current.type == EventType.ValidateCommand &&
				 Event.current.commandName == "UndoRedoPerformed"))
				{
					OnUndoRedo();
					
				}
				//Toggle editing mode
				if (editing)
				{
					if (GUILayout.Button("Stop Editing"))
					{
						editing = false;
						HideWireframe(false);
					}
				}
				else if (GUILayout.Button("Edit PolyMesh"))
				{
					editing = true;
					HideWireframe(hideWireframe);
				}

				//Mesh settings
				if (meshSettings = EditorGUILayout.Foldout(meshSettings, "Mesh"))
				{
					polyMesh.height = EditorGUILayout.FloatField("Height", polyMesh.height);
					polyMesh.floor = EditorGUILayout.FloatField("Floor", polyMesh.floor);
					polyMesh.zIndex = EditorGUILayout.FloatField("Z-Index", polyMesh.zIndex);
					polyMesh.addMode = EditorGUILayout.IntField("AddMode", polyMesh.addMode);
					
					if (polyMesh.ShaderToUse == null)
					{
						polyMesh.ShaderToUse = Shader.Find("Bumped Diffuse");
						Material mat = new Material(polyMesh.ShaderToUse);
						polyMesh.textureNames = MeshUtils.getTextures(mat);
						polyMesh.texturesToUse = new List<Texture2D>();
						for (int i = 0; i < polyMesh.textureNames.Count; ++i)
						{
							polyMesh.texturesToUse.Add(new Texture2D(1,1));
							polyMesh.texturesToUse[i].name = "Click to set " + polyMesh.textureNames[i];
							//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
						}
					}
					Shader last = EditorGUILayout.ObjectField(polyMesh.ShaderToUse, typeof(Shader), true) as Shader;
					
					if (polyMesh.ShaderToUse != last || !ran)
					{
						ran = true;
						if (polyMesh.ShaderToUse != last)
						{
							polyMesh.ShaderToUse = last;
							Material mat = new Material(polyMesh.ShaderToUse);
							
							polyMesh.textureNames = MeshUtils.getTextures(mat);
							polyMesh.texturesToUse = new List<Texture2D>();
							for (int i = 0; i < polyMesh.textureNames.Count; ++i)
							{
								polyMesh.texturesToUse.Add(new Texture2D(1,1));
								polyMesh.texturesToUse[i].name = "Click to set " + polyMesh.textureNames[i];
								//buttontext.Add ("Select a texture and click here to set " + options.textureNames[i]);
							}
						}
						
					}
					
					for (int i = 0; i < polyMesh.textureNames.Count; ++i)
					{
						polyMesh.texturesToUse[i] = EditorGUILayout.ObjectField(polyMesh.texturesToUse[i], typeof(Texture2D), true) as Texture2D;
					}
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Room"))
					{
						polyMesh.AddRoom();
					}
					EditorGUILayout.Space();
					if (GUILayout.Button("Build Mesh"))
					{
						polyMesh.BuildFinishedMesh();
					}
				}


				//Editor settings
				if (editorSettings = EditorGUILayout.Foldout(editorSettings, "Editor"))
				{
					gridSnap = EditorGUILayout.FloatField("Grid Snap", gridSnap);
					autoSnap = EditorGUILayout.Toggle("Auto Snap", autoSnap);
					//globalSnap = EditorGUILayout.Toggle("Global Snap", globalSnap);
					EditorGUI.BeginChangeCheck();
					hideWireframe = EditorGUILayout.Toggle("Hide Wireframe", hideWireframe);
					if (EditorGUI.EndChangeCheck())
						HideWireframe(hideWireframe);


					editKey = (KeyCode)EditorGUILayout.EnumPopup("[Toggle Edit] Key", editKey);
					splitKey = (KeyCode)EditorGUILayout.EnumPopup("[Split] Key", splitKey);
					extrudeKey = (KeyCode)EditorGUILayout.EnumPopup("[Delete] Key", extrudeKey);

				}

				//Update mesh
				if (GUI.changed)
					polyMesh.BuildFinishedMesh();
			}
			else
			{

			}
		}

		#endregion

		#region Scene GUI

		void OnSceneGUI()
		{
			if (editing)
			{
				Tools.hidden = true;
			}
			else
			{
				Tools.hidden = false;
			}
			if (!multiple)
			{

				if (target == null)
					return;
				
				//Update lists
				if (keyPoints == null)
				{
					keyPoints = new List<Vector3>(polyMesh.keyPoints);	
				}
				
				//Load handle matrix
				Handles.matrix = polyMesh.transform.localToWorldMatrix;
				
				//Draw points and lines
				DrawAxis();
				Handles.color = Color.white;
				for (int i = 0; i < keyPoints.Count; i++)
				{
					Handles.color = nearestLine == i ? Color.green : Color.white;
					DrawSegment(i);
					if (selectedIndices.Contains(i))
					{
						Handles.color = Color.green;
						DrawCircle(keyPoints[i], 0.08f);
					}
					else
						Handles.color = Color.white;
					DrawKeyPoint(i);
				}
				if (KeyPressed(editKey))
				{
					editing = !editing;
				}

				if (editing)
				{

					//Quit on tool change
					if (e.type == EventType.KeyDown)
					{
						switch (e.keyCode)
						{
						case KeyCode.Q:
						case KeyCode.W:
						case KeyCode.E:
						case KeyCode.R:
							return;
						}
					}
					//Quit if panning or no camera exists
					if (Tools.current == Tool.View || (e.isMouse && e.button > 0) || Camera.current == null || e.type == EventType.ScrollWheel)
						return;

					//Quit if laying out
					if (e.type == EventType.Layout)
					{
						HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
						return;
					}

					//Cursor rectangle
					EditorGUIUtility.AddCursorRect(new Rect(0, 0, Camera.current.pixelWidth, Camera.current.pixelHeight), mouseCursor);
					mouseCursor = MouseCursor.Arrow;

					//Extrude key state
					if (e.keyCode == extrudeKey)
					{
						if (extrudeKeyDown)
						{
							if (e.type == EventType.KeyUp)
								extrudeKeyDown = false;
						}
						else if (e.type == EventType.KeyDown)
							extrudeKeyDown = true;
					}

					//Update matrices and snap
					worldToLocal = polyMesh.transform.worldToLocalMatrix;
					inverseRotation = Quaternion.Inverse(polyMesh.transform.rotation) * Camera.current.transform.rotation;
					snap = gridSnap;
					
					//Update mouse position
					screenMousePosition = new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y,0.0f);
					var plane = new Plane(-polyMesh.transform.up, polyMesh.transform.position);
					var ray = Camera.current.ScreenPointToRay(screenMousePosition);
					float hit;
					if (plane.Raycast(ray, out hit))
						mousePosition = worldToLocal.MultiplyPoint(ray.GetPoint(hit));
					else
						return;

					//Update nearest line and split position
					nearestLine = NearestLine(out splitPosition);
					
					//Update the state and repaint
					var newState = UpdateState();
					if (state != newState)
						SetState(newState);
					HandleUtility.Repaint();
					//e.Use();
				}
			}
		}

		void HideWireframe(bool hide)
		{
			if (polyMesh.GetComponent<Renderer>() != null)
				EditorUtility.SetSelectedWireframeHidden(polyMesh.GetComponent<Renderer>(), hide);
		}

		void RecordUndo()
		{
			Undo.RecordObject(polyMesh, "PolyMesh Changed");
		}

		void RecordDeepUndo()
		{
			Undo.RegisterFullObjectHierarchyUndo(polyMesh, "PolyMesh Changed");

		}

		#endregion

		#region State Control

		//Initialize state
		void SetState(State newState)
		{
			state = newState;
		}

		//Update state
		State UpdateState()
		{
			if (Tools.current == Tool.Move)
			{
				state = vertMode ();
			}
			return state;
		}
		State vertMode()
		{
			switch (state)
			{
				//Hovering
			case State.Hover:
				
				DrawNearestLineAndSplit();
				
				if ( TryDragSelected())
					return State.DragSelected;
				
				if (TrySplitLine())
					return State.Hover;
				if (TryDeleteSelected())
					return State.Hover;
				if (TryHoverKeyPoint(out dragIndex) && TryDragKeyPoint(dragIndex))
					return State.Drag;
				
				break;
				
				//Dragging
			case State.Drag:
			{
				mouseCursor = MouseCursor.MoveArrow;
				DrawCircle(keyPoints[dragIndex], clickRadius);
				MoveKeyPoint(dragIndex, mousePosition - clickPosition);
			}
				if (TryStopDrag())
					return State.Hover;
				break;
				
				//Box Selecting
				
				//Dragging selected
			case State.DragSelected:
				mouseCursor = MouseCursor.MoveArrow;
				MoveSelected(mousePosition - clickPosition);
				if (TryStopDrag())
					return State.Hover;
				break;
				
			}
			return state;
		}
		
		bool TryDeleteSelected()
		{
			if (extrudeKeyDown)
			{
				extrudeKeyDown = false;
				int index = NearestPoint(keyPoints);
				if (index >= 0)
				{
					if (keyPoints.Count - 1 >= 3)
					{
						//for (int i = selectedIndices.Count - 1; i >= 0; i--)
						{
							//var index = selectedIndices[i];
							keyPoints.RemoveAt(index);
						}
						//selectedIndices.Clear();
						UpdatePoly(true, true);
						return true;
					}
				}
			}
			return false;
		}

		//Update the mesh on undo/redo
		void OnUndoRedo()
		{
			keyPoints = new List<Vector3>(polyMesh.keyPoints);
			polyMesh.BuildFinishedMesh();
		}
		
		void TransformPoly(Matrix4x4 matrix)
		{
			for (int i = 0; i < keyPoints.Count; i++)
			{
				keyPoints[i] = matrix.MultiplyPoint(polyMesh.keyPoints[i]);
			}
		}
		
		void UpdatePoly(bool sizeChanged, bool recordUndo)
		{
			if (recordUndo)
				RecordUndo();
			if (sizeChanged)
			{
				polyMesh.keyPoints = new List<Vector3>(keyPoints);
			}
			else
			{
				for (int i = 0; i < keyPoints.Count; i++)
				{
					polyMesh.keyPoints[i] = keyPoints[i];
				}
			}
			polyMesh.BuildFinishedMesh();
		}

		void MoveKeyPoint(int index, Vector3 amount)
		{
			var moveCurve = selectedIndices.Contains((index + 1) % keyPoints.Count);
		
			if (doSnap)
			{
				Vector3 rnd = polyMesh.transform.TransformPoint(new Vector3(0,0,0));
				rnd = rnd-Snap (rnd);
				keyPoints[index] = Snap(polyMesh.keyPoints[index] + amount)+rnd;
			}
			else
			{
				keyPoints[index] = polyMesh.keyPoints[index] + amount;
			}
		}



		void MoveSelected(Vector3 amount)
		{
			foreach (var i in selectedIndices)
				MoveKeyPoint(i, amount);
		}

		#endregion

		#region Drawing

		void DrawAxis()
		{
			Handles.color = Color.red;
			var size = HandleUtility.GetHandleSize(Vector3.zero) * 0.1f;
			Handles.DrawLine(new Vector3(-size,0, 0), new Vector3(size,0, 0));
			Handles.DrawLine(new Vector3(0,0, -size), new Vector3(0,0, size));
		}

		void DrawKeyPoint(int index)
		{
			Handles.DotCap(0, keyPoints[index], Quaternion.identity, HandleUtility.GetHandleSize(keyPoints[index]) * 0.03f);
		}


		void DrawSegment(int index)
		{
			var from = keyPoints[index];
			var to = keyPoints[(index + 1) % keyPoints.Count];

			Handles.DrawLine(from, to);
		}

		void DrawCircle(Vector3 position, float size)
		{
			Handles.CircleCap(0, position, inverseRotation, HandleUtility.GetHandleSize(position) * size);
		}

		void DrawNearestLineAndSplit()
		{

			if (nearestLine >= 0)
			{
				Handles.color = Color.green;
				DrawSegment(nearestLine);
				Handles.color = Color.red;
				Handles.DotCap(0, splitPosition, Quaternion.identity, HandleUtility.GetHandleSize(splitPosition) * 0.03f);
			}
		}

		#endregion

		#region State Checking

		bool TryHoverKeyPoint(out int index)
		{
			if (TryHover(keyPoints, Color.white, out index))
			{
				mouseCursor = MouseCursor.MoveArrow;
				return true;
			}
			return false;
		}


		bool TryDragKeyPoint(int index)
		{
			if (TryDrag(keyPoints, index))
			{
				return true;
			}
			return false;
		}

		bool TryHover(List<Vector3> points, Color color, out int index)
		{

			//if (Tools.current == Tool.None)
			{
				index = NearestPoint(points);
				if (index >= 0 && IsHovering(points[index]))
				{
					Handles.color = color;
					DrawCircle(points[index], clickRadius);
					return true;
				}
			}
			index = -1;
			return false;
		}

		bool TryDrag(List<Vector3> points, int index)
		{
			if (e.type == EventType.MouseDown && IsHovering(points[index]))
			{
				clickPosition = mousePosition;
				return true;
			}
			return false;
		}

		bool TryStopDrag()
		{
			if (e.type == EventType.MouseUp)
			{
				dragIndex = -1;
				UpdatePoly(false, true);
				return true;
			}
			return false;
		}


		bool TryDragSelected()
		{
			if (selectedIndices.Count > 0 && TryDragButton(GetSelectionCenter(), 0.2f))
			{
				clickPosition = mousePosition;
				return true;
			}
			return false;
		}


		bool TryDragButton(Vector3 position, float size)
		{
			size *= HandleUtility.GetHandleSize(position);
			if (Vector3.Distance(mousePosition, position) < size)
			{
				if (e.type == EventType.MouseDown)
					return true;
				else
				{
					mouseCursor = MouseCursor.MoveArrow;
					Handles.color = Color.green;
				}
			}
			else
				Handles.color = Color.white;
			var buffer = size / 2;
			Handles.DrawLine(new Vector3(position.x - buffer, position.y), new Vector3(position.x + buffer, position.y));
			Handles.DrawLine(new Vector3(position.x, position.y - buffer), new Vector3(position.x, position.y + buffer));
			Handles.RectangleCap(0, position, Quaternion.identity, size);
			return false;
		}

		bool TrySplitLine()
		{
			if (nearestLine >= 0 && KeyPressed(splitKey))
			{
				if (nearestLine == keyPoints.Count - 1)
				{
					keyPoints.Add(splitPosition);
				}
				else
				{
					keyPoints.Insert(nearestLine + 1, splitPosition);
				}
				UpdatePoly(true, true);
				return true;
			}
			return false;
		}

		bool IsHovering(Vector3 point)
		{
			return Vector3.Distance(mousePosition, point) < HandleUtility.GetHandleSize(point) * clickRadius;
		}
		
		int NearestPoint(List<Vector3> points)
		{
			var near = -1;
			var nearDist = float.MaxValue;
			for (int i = 0; i < points.Count; i++)
			{
				var dist = Vector3.Distance(points[i], mousePosition);
				if (dist < nearDist)
				{
					nearDist = dist;
					near = i;
				}
			}
			return near;
		}
		
		int NearestLine(out Vector3 position)
		{
			var near = -1;
			var nearDist = float.MaxValue;
			position = keyPoints[0];
			var linePos = Vector3.zero;
			for (int i = 0; i < keyPoints.Count; i++)
			{
				var j = (i + 1) % keyPoints.Count;
				var line = keyPoints[j] - keyPoints[i];
				var offset = mousePosition - keyPoints[i];
				var dot = Vector3.Dot(line.normalized, offset);
				if (dot >= 0 && dot <= line.magnitude)
				{
					linePos = keyPoints[i] + line.normalized * dot;
					var dist = Vector3.Distance(linePos, mousePosition);
					if (dist < nearDist)
					{
						nearDist = dist;
						position = linePos;
						near = i;
					}
				}
			}
			return near;
		}

		bool KeyPressed(KeyCode key)
		{
			return e.type == EventType.KeyDown && e.keyCode == key;
		}

		bool KeyReleased(KeyCode key)
		{
			return e.type == EventType.KeyUp && e.keyCode == key;
		}
		
		Vector3 Snap(Vector3 value)
		{
			value.x = Mathf.Round(value.x / snap) * snap;
			value.z = Mathf.Round(value.z / snap) * snap;
			return value;
		}

		Vector3 GetSelectionCenter()
		{
			if (polyMesh.keyPoints.Count > 1)
			{
				var center = Vector3.zero;
				var area = 0f;
				var b = polyMesh.keyPoints[polyMesh.keyPoints.Count - 1];
				foreach (var i in selectedIndices)
				{
					var a = polyMesh.keyPoints[i];
					var k = a.y * b.x - a.x * b.y;
					area += k;
					center.x += (a.x + b.x) * k;
					center.y += (a.y + b.y) * k;
					b = a;
				}
				area *= 3;
				if (Mathf.Approximately(area, 0))
					return Vector3.zero;
				else
					return center / area;
			}
			else
				return polyMesh.keyPoints[0];
		}

		#endregion

		#region Properties

		PolyMesh polyMesh
		{
			get { return (PolyMesh)target; }
		}

		Event e
		{
			get { return Event.current; }
		}

		bool control
		{
			get { return Application.platform == RuntimePlatform.OSXEditor ? e.command : e.control; }
		}

		bool doSnap
		{
			get { return autoSnap ? !control : control; }
		}

		static bool meshSettings
		{
			get { return EditorPrefs.GetBool("PolyMeshEditor_meshSettings", false); }
			set { EditorPrefs.SetBool("PolyMeshEditor_meshSettings", value); }
		}

		static bool colliderSettings
		{
			get { return EditorPrefs.GetBool("PolyMeshEditor_colliderSettings", false); }
			set { EditorPrefs.SetBool("PolyMeshEditor_colliderSettings", value); }
		}

		static bool uvSettings
		{
			get { return EditorPrefs.GetBool("PolyMeshEditor_uvSettings", false); }
			set { EditorPrefs.SetBool("PolyMeshEditor_uvSettings", value); }
		}

		static bool editorSettings
		{
			get { return EditorPrefs.GetBool("PolyMeshEditor_editorSettings", false); }
			set { EditorPrefs.SetBool("PolyMeshEditor_editorSettings", value); }
		}

		static bool autoSnap
		{
			get { return EditorPrefs.GetBool("PolyMeshEditor_autoSnap", false); }
			set { EditorPrefs.SetBool("PolyMeshEditor_autoSnap", value); }
		}

		static float gridSnap
		{
			get { return EditorPrefs.GetFloat("PolyMeshEditor_gridSnap", 1); }
			set { EditorPrefs.SetFloat("PolyMeshEditor_gridSnap", value); }
		}

		static bool hideWireframe
		{
			get { return EditorPrefs.GetBool("PolyMeshEditor_hideWireframe", true); }
			set { EditorPrefs.SetBool("PolyMeshEditor_hideWireframe", value); }
		}

		public KeyCode editKey
		{
			get { return (KeyCode)EditorPrefs.GetInt("PolyMeshEditor_editKey", (int)KeyCode.Tab); }
			set { EditorPrefs.SetInt("PolyMeshEditor_editKey", (int)value); }
		}


		public KeyCode splitKey
		{
			get { return (KeyCode)EditorPrefs.GetInt("PolyMeshEditor_splitKey", (int)KeyCode.S); }
			set { EditorPrefs.SetInt("PolyMeshEditor_splitKey", (int)value); }
		}


		public KeyCode extrudeKey
		{
			get { return (KeyCode)EditorPrefs.GetInt("PolyMeshEditor_extrudeKey", (int)KeyCode.D); }
			set { EditorPrefs.SetInt("PolyMeshEditor_extrudeKey", (int)value); }
		}

		#endregion

		#region Menu Items


		#endregion
	}
}