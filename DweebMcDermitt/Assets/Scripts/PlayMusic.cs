using UnityEngine;
using System.Collections;

public class playMusic : MonoBehaviour {
	
	public AudioClip backgroundMusic;
	public AudioSource source;
	
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		source.PlayOneShot(backgroundMusic, .5f);
	}

	void Update() {

	}
}