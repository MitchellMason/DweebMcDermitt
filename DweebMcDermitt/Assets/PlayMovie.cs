using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour
{
	public MovieTexture mov;

	void Start () {
		GetComponent<Renderer> ().material.mainTexture = mov;
		mov.Play ();
	}
}

