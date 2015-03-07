using UnityEditor;
using UnityEngine;
using System.Collections;

namespace PosterGen
{
	//[CustomEditor(typeof(RoomGen))]
	public class GridGUI : EditorWindow {

		
		Vector2 scrollPos = new Vector2(0,0);
		
		public Rect windowRect = new Rect(20, 20, 400, 100);
		float ratio = 32.0f;
		float snapto = 1.0f;
		float zoom = 0.02f;
		float frequency = 0.01f;
		int numDivides = 4;
		public FrameGen fg;

		public void CreateGen (FrameGen temp)
		{
			fg = temp;
			numDivides = fg.divides.Count;
			GridGUI m_window = EditorWindow.GetWindow<GridGUI>();
			//setting the min and max size of the window will make it so that scaling the window will have no effect
			m_window.minSize = new Vector2(500, 500);
		}

		
		void OnGUI () {

			float zoomer = ratio/zoom;
			float mult = snapto * zoomer;
			float freq = frequency * zoomer;
			Vector2 pos = new Vector2(position.width, position.height);
			Vector2 lup =  new Vector2(25,125);
			Vector2 rdown = pos - new Vector2(25,25);

			Rect r = new Rect(lup.x,lup.y,rdown.x-lup.x,rdown.y-lup.y);

			Vector2 box = rdown - lup;
			Vector2 center = box / 2.0f;
			
			Handles.color = Color.gray;
			for (float i = freq; i < (box.x)/2.0f; i += freq)
			{
				Handles.DrawLine(new Vector3(center.x+lup.x+i,lup.y), new Vector3(center.x+lup.x+i, rdown.y));
				Handles.DrawLine(new Vector3(center.x+lup.x-i,lup.y), new Vector3(center.x+lup.x-i, rdown.y));
			}
			
			Handles.DrawLine(new Vector3(center.x+lup.x,lup.y), new Vector3(center.x+lup.x, rdown.y));
			for (float i = freq; i < (box.y)/2.0f; i += freq)
			{
				Handles.DrawLine(new Vector3(lup.x,center.y+lup.y+i), new Vector3(rdown.x, center.y+lup.y+i));
				Handles.DrawLine(new Vector3(lup.x,center.y+lup.y-i), new Vector3(rdown.x, center.y+lup.y-i));
			}

			Handles.color = Color.white;

			center.x = 0;
			
			Handles.DrawAAPolyLine(2.0f,new Vector3(center.x+lup.x,lup.y), new Vector3(center.x+lup.x, rdown.y));
			Handles.DrawAAPolyLine(2.0f,new Vector3(lup.x,center.y+lup.y), new Vector3(rdown.x, center.y+lup.y));

			Handles.DrawPolyLine( new Vector3(lup.x, lup.y),
			                       	new Vector3(rdown.x, lup.y),
			                       	new Vector3(rdown.x, rdown.y),
									new Vector3(lup.x,rdown.y),
									new Vector3(lup.x,lup.y-1));

			
			Handles.color = Color.white;
			
			Vector2 last = new Vector2(0,0) + (center+lup);

			for (int i = 0; i < fg.divides.Count; ++i)
			{
				Vector2 current = new Vector2(fg.divides[i].y,-fg.divides[i].x)*zoomer + (center+lup);
				
				Handles.DrawSolidDisc(new Vector3(current.x,current.y,0),new Vector3(0,0,1), 4.0f);
				Handles.DrawAAPolyLine(7.0f,last, current);
				Handles.DrawAAPolyLine(7.0f,last, current);
				last = current;
			}
			Handles.DrawAAPolyLine(7.0f,last, new Vector2((center+lup).x,last.y));
			Handles.DrawAAPolyLine(7.0f,last, new Vector2((center+lup).x,last.y));
			
			Handles.color = Color.red;

			Vector2 mouse = Event.current.mousePosition-(center+lup);
			Vector2 rmouse = mouse + center + lup;//new Vector2(Mathf.Round(mouse.x/mult)*mult, Mathf.Round(mouse.y/mult)*mult)+center+lup;
			if (r.Contains(rmouse))
			{
				Handles.DrawSolidDisc(new Vector3(rmouse.x,rmouse.y,0),new Vector3(0,0,1), 4.0f);
			}

			BeginWindows ();
			windowRect = new Rect(25, 20, position.width-50, 100);
			// All GUI.Window or GUILayout.Window must come inside here
			GUILayout.Window (1, windowRect, DoWindow, "Options");		
			
			// Collect all the windows between the two.
			EndWindows ();		

			Repaint();

		}
		
		void DoWindow(int windowID) {
			EditorGUILayout.BeginHorizontal ();
			//snapto = Mathf.Max (Mathf.Min (EditorGUILayout.FloatField("Snap to:", snapto),100.0f),0.001f);
			zoom = Mathf.Max (Mathf.Min (EditorGUILayout.FloatField("Zoom:", zoom), 100.0f),0.001f);
			frequency = Mathf.Max (Mathf.Min (EditorGUILayout.FloatField("Grid frequency:", frequency), 100.0f),0.001f);

			
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
			EditorGUILayout.EndHorizontal();
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			EditorGUILayout.BeginHorizontal ();
			for (int i = 0; i < fg.divides.Count; ++i)
			{
				fg.divides[i] = EditorGUILayout.Vector2Field("Segment " + (i+1) + ":",fg.divides[i]);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndScrollView();
		}
	}
}