using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	
	// Check for clicks and move on to the next scene
	void Update () {
		if(Input.anyKeyDown){
			Application.LoadLevel("Intro");
		}
	}
}
