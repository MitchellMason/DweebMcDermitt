using UnityEngine;
using System.Collections;

public class LevelFader : MonoBehaviour {

	public Texture2D fadeOutTexture; 	// Texture overlaying the screen
	public float fadeSpeed = 0.8f; 		// fade speed

	private int drawDepth = -1000;		// texture's order in the draw hierarchy: low numbers render on top
	private float alpha = 1.0f;			// texture's alpha value between 0 and 1
	private int fadeDir = -1;			// fade direction: in = -1   out = 1

	void onGUI () {
		// fade out/in the alpha value using a direction, speed, and time
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force (clamp) the number between 0 and 1 because GUI.color uses alpha values between 0 and 1
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.b, alpha);	// set alpha value
		GUI.depth = drawDepth;										// make black texture render on top
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);	// fit texture to entire screen

	}

	public float BeginFade(int direction) {
		fadeDir = direction;
		return(fadeSpeed);
	}

	void OnLevelWasLoaded() {
		BeginFade (-1);		// fade in
	}
}
