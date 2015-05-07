using UnityEngine;
using System.Collections;

public class ShowDweeb : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Camera cam = Camera.main;
		float pos = (cam.nearClipPlane - 0.2f);
		transform.parent = cam.transform;
		transform.position = cam.transform.position + cam.transform.forward * pos + cam.transform.up * -1.9f + cam.transform.right * -.2f;
	}
}
