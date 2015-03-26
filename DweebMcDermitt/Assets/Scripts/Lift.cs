using UnityEngine;
using System.Collections;

//when interacted upon, moves to the new position in specified time interval
public class Lift : InteractionTarget{
	[SerializeField] private Vector3 newPositionRelative;
	[SerializeField] private float moveSpeed;
	
	private Vector3	newPosition;
	private bool interactedOn = false;
	private float startTime;
	private float journeyDistance;
	private Vector3 originalPosition;
	private float journeyDistance2;
	
	[SerializeField] private AudioSource cubeAudio;
	
	//store the distance between our location and the new one
	void Start(){
		newPosition = transform.position + newPositionRelative;
		journeyDistance = Vector3.Distance (transform.position, newPosition);
		originalPosition = transform.position;
		journeyDistance2 = Vector3.Distance (newPosition, originalPosition);
	}
	
	// If we're interacted upon, start moving the object to victory
	void Update () {
		if (interactedOn) {
			transform.position = Vector3.Lerp (transform.position, 
			                                   newPosition, 
			                                   (Time.time - startTime) * moveSpeed / journeyDistance);
		}
		if (!interactedOn) {
			transform.position = Vector3.Lerp (newPosition, originalPosition, 
			                                   (Time.time - startTime) * moveSpeed / journeyDistance2);
		}
	}
	
	public override void onInteract(){
		Debug.Log ("Lift triggered");
		if (!interactedOn) {
			startTime = Time.time;
			cubeAudio.Play ();
			
		}
		//prevent starting over
		interactedOn = !interactedOn;
		
	}
	
	override public bool isTriggered(){
		return interactedOn;
	}
}