using UnityEngine;
using System.Collections;

//when interacted upon, moves to the new position in specified time interval
public class LiftWithDialogue : TriggerTarget{
	[SerializeField] private Vector3 newPositionRelative;
	[SerializeField] private float moveSpeed;
	
	private Vector3	newPosition;
	private bool isTriggered = false;
	private float startTime;
	private float journeyDistance;
	
	//public AudioSource door;
	[SerializeField] private AudioSource liftAudio;
	[SerializeField] private AudioSource endingDialogue;
	
	bool played = false;
	
	//store the distance between our location and the new one
	void Start(){
		newPosition = transform.position + newPositionRelative;
		journeyDistance = Vector3.Distance (transform.position, newPosition);
	}
	
	// If we're interacted upon, start moving the object to victory
	void Update () {
		if (isTriggered) {
			transform.position = Vector3.Lerp(transform.position, 
			                                  newPosition, 
			                                  (Time.time - startTime) * moveSpeed / journeyDistance);
		}
		if (played && !endingDialogue.isPlaying) {
			Debug.Log("trigger them credits");
			Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
	
	public override void onTrigger(MonoBehaviour trigger){
		Debug.Log ("Bookshelf triggered");
		if (!isTriggered) {
			startTime = Time.time;
			liftAudio.Play ();
			if (liftAudio != null) {
				endingDialogue.PlayDelayed(7f);
				played = true;
			}
		}
		//prevent starting over
		isTriggered = true;
	}
	
	//	override public bool isTriggered(){
	//		return interactedOn;
	//	}
}