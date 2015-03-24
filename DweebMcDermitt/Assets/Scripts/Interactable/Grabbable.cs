using UnityEngine;
using System.Collections;

public class Grabbable : InteractionTarget {

	private GameObject player;
	private Transform playerTransform;
	private Vector3 playerPosition;

	private Vector3 cameraPosition;
	private GameObject hitObject;
	private RaycastHit hit;
	private Collider col;
	private bool interactedOn = false;
	private bool colliding = false;

	private Camera camera;
	private Vector3 center;
	
	void Start(){
		camera = Camera.main;
		center = transform.position;
		player = GameObject.Find ("Player");
		playerTransform = player.transform;
		col = GetComponent<Collider> ();
	}
	
	void Update () {

		center = camera.ScreenToWorldPoint(new Vector3(Screen.height/2, Screen.width/2, camera.nearClipPlane));
		cameraPosition = camera.transform.position;
		playerPosition = playerTransform.position;
		if (interactedOn) {

			transform.parent = camera.transform;
			col.attachedRigidbody.useGravity = false;

			// Stop out of control collision
			if (colliding) {
				print ("colliding");
			} else {
				col.attachedRigidbody.velocity = new Vector3(0,0,0);
				col.attachedRigidbody.angularVelocity = new Vector3(0,0,0);
				transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
				transform.position = camera.transform.position + camera.transform.forward * 1.5f;
			}
		} else {
			transform.parent = null;
			col.attachedRigidbody.useGravity = true;
		}
	}

	void OnCollisionEnter(Collision c) {
		colliding = true;
	}
	void OnCollisionStay(Collision c) {
		colliding = true;
	}
	void OnCollisionExit(Collision c) {
		colliding = false;
	}

	public override void onInteract(){
		Debug.Log ("Grabbable triggered");
		interactedOn = !interactedOn;
	}
	
	override public bool isTriggered(){
		return interactedOn;
	}
}