using UnityEngine;
using System.Collections;

//when interacted upon, moves to the new position in specified time interval
public class OneWayLift : InteractionTarget{
	[SerializeField] private Vector3 newPositionRelative;
	[SerializeField] private float moveSpeed;
	
	private Vector3	newPosition;
	private bool interactedOn = false;
	private float startTime;
	private float journeyDistance;
	
	//public AudioSource door;
	[SerializeField] private AudioSource liftAudio;
	
	//store the distance between our location and the new one
	void Start(){
		newPosition = transform.position + newPositionRelative;
		journeyDistance = Vector3.Distance (transform.position, newPosition);
	}
	
	// If we're interacted upon, start moving the object to victory
	void Update () {
		if (interactedOn) {
			transform.position = Vector3.Lerp(transform.position, 
			                                  newPosition, 
			                                  (Time.time - startTime) * moveSpeed / journeyDistance);
		}
	}
	
	public override void onInteract(){
		Debug.Log ("Bookshelf triggered");
		if (!interactedOn) {
			startTime = Time.time;
			liftAudio.Play ();
		}
		//prevent starting over
		interactedOn = true;
	}
	
	override public bool isTriggered(){
		return interactedOn;
	}
}