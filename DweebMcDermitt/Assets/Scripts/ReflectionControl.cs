using UnityEngine;
using System.Collections;

//have the camera this is attatched to always look away from the player
public class ReflectionControl : MonoBehaviour {
	private Transform playerPos;
	private Vector3 lookAt;
	void Start(){
		GameObject tempPlayer = GameObject.FindGameObjectWithTag ("Player");
		playerPos = tempPlayer.transform;
	}
	// Update is called once per frame
	void Update () {
		lookAt = playerPos.position;
		lookAt.x = transform.position.x + (transform.position.x - lookAt.x);
		this.transform.LookAt (lookAt);
		this.transform.Rotate (0f, 180f, 0f);
	}
}
