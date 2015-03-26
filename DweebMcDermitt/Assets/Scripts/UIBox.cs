using UnityEngine;
using System.Collections;

public class UIBox : MonoBehaviour
{
	public bool dead = false;
	public int depth = 5;
	void OnGUI() {
		if (dead) {
			GUI.depth = depth;
			print ("should be drawing");
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "U R DED");
		}
	}
}

