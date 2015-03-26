using UnityEngine;
using System.Collections;

public class PlayMoviePlane : MonoBehaviour
{
	public MovieTexture mov;
	public bool triggered = false;

	void Update () {
		if (triggered) {
			GetComponent<Renderer> ().material.mainTexture = mov;
			mov.Play ();
			print ("played");
			triggered = false;
		}
	}

}

