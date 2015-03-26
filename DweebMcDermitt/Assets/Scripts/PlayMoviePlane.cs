using UnityEngine;
using System.Collections;

public class PlayMoviePlane : MonoBehaviour
{
	public MovieTexture mov;
	//public Texture first;
	//public Texture second;
	//public Texture third;
	//public Texture final;
	public bool triggered = false;

	Renderer rend;

	public int HP = 3;
	public Texture[] textures;

	void Start() {
		rend = GetComponent<Renderer>();
		rend.material.mainTexture = textures[0];
	}

	void Update () {
		Camera cam = Camera.main;
		float pos = (cam.nearClipPlane + 0.05f);
		transform.parent = cam.transform;
		transform.position = cam.transform.position + cam.transform.forward * pos;
		if (triggered) {
			if (HP == 2) {
				rend.material.mainTexture = textures[0];
				print ("texture set to first");
			} else if (HP == 1) {
				rend.material.mainTexture = textures[2];
				print ("texture set to third");
			} else if (HP <= 0) {
				rend.material.mainTexture = textures[3];
				print ("texture set to last");
			}
			//GetComponent<Renderer> ().material.mainTexture = mov;

			 //mov.Play ();
			GetComponent<MeshRenderer> ().enabled = true;
			triggered = false;
		}
	}

}

