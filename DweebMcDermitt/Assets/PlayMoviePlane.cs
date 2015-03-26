using UnityEngine;
using System.Collections;

public class PlayMoviePlane : MonoBehaviour
{
	public MovieTexture mov;
	public bool triggered = false;

	void Update () {
		Camera cam = Camera.main;
		float pos = (cam.nearClipPlane + 0.05f);
		transform.parent = cam.transform;
		transform.position = cam.transform.position + cam.transform.forward * pos;
		if (triggered) {
			GetComponent<Renderer> ().material.mainTexture = mov;
			mov.Play ();
			print ("played");
			triggered = false;
		}
	}

}

