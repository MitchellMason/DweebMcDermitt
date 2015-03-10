using UnityEngine;
using System.Collections;

public class WeightedButton : PositionTarget {
	private bool pressed = false;
	private float weightThreshold = 10.0f;
	[SerializeField] private AudioSource buttonAudio;
	
	void OnTriggerEnter(Collider col){
		pressed = false;
		if(col.GetComponent<Rigidbody>() != null && col.GetComponent<Rigidbody>().mass > weightThreshold){
			pressed = true;
			buttonAudio.Play ();
			Debug.Log ("weighted button on");
		}
		if(col.GetComponent<Player>() != null){
			pressed = true;
			buttonAudio.Play ();
			Debug.Log ("weighted button on");
		}
	}
	
	void OnTriggerExit(Collider col){
		Debug.Log ("weighted button off");
		pressed = false;
	}
	
	override public bool isTriggered(){
		return pressed;
	}
	
	override public void onPosition() {
		// Get off of me fatty I'm trying to live my life.
	}
}
