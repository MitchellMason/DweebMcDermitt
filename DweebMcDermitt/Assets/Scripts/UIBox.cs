using UnityEngine;
using System.Collections;

public class UIBox : MonoBehaviour
{
	public bool dead = false;
	public GameObject text;
	Camera cam;

	void Start() {
		GameObject.Find ("rekt");
	}

	void Update() {
		cam = Camera.main;
		text.transform.parent = cam.transform;
		text.transform.position = cam.transform.position + cam.transform.forward * 10.0f + -cam.transform.right * 7.0f;
		if (dead) {
			print ("should be drawing");
			text.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}

