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
	
	void Start(){
		player = GameObject.Find ("Player");
		playerTransform = player.transform;
		col = GetComponent<Collider> ();
	}
	
	void Update () {
		cameraPosition = Camera.main.transform.position;
		playerPosition = playerTransform.position;
		if (interactedOn) {
			transform.parent = Camera.main.transform;

			col.attachedRigidbody.isKinematic = true;
		} else {
			transform.parent = null;
			//transform.position = new Vector3(transform.position.x, 1, transform.position.z);
			col.attachedRigidbody.isKinematic = false;
		}
	}
	
	public override void onInteract(){
		Debug.Log ("Grabbable triggered");
		interactedOn = !interactedOn;
	}
	
	override public bool isTriggered(){
		return interactedOn;
	}
}