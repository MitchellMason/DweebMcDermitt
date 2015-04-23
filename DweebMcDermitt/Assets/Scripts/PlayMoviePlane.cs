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

	[SerializeField] private AudioSource hit1;
	[SerializeField] private AudioSource hit2;
	[SerializeField] private AudioSource hit3;
	[SerializeField] private AudioSource myFace;

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
				if (!hit1.isPlaying) 
					hit1.Play ();
			} else if (HP == 1) {
				rend.material.mainTexture = textures[2];
				print ("texture set to third");
				if (!hit2.isPlaying) 
					hit2.Play ();
			} else if (HP <= 0) {
				rend.material.mainTexture = textures[3];
				print ("texture set to last");
				if (!hit3.isPlaying) {
					hit3.Play ();
					Debug.Log("Level should be fading");
					RestartLevel();
				}

				if(myFace != null) myFace.PlayDelayed(1f);
			}
			//GetComponent<Renderer> ().material.mainTexture = mov;

			 //mov.Play ();
			GetComponent<MeshRenderer> ().enabled = true;
			triggered = false;
		}
	}

	IEnumerator RestartLevel() {
		float fadeTime = GameObject.Find ("PlayerToggle").GetComponent<LevelFader> ().BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel (1);
	}

}

