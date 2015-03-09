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
		}
		if(col.gameObject.tag.Equals("Player")){
			pressed = true;
			buttonAudio.Play ();
		}
	}
	
	void OnTriggerExit(Collider col){
		pressed = false;
	}
	
	override public bool isTriggered(){
		Debug.Log (this.gameObject.name + " is " + pressed);
		return pressed;
	}
	
	override public void onPosition() {
		// Get off of me fatty I'm trying to live my life.
	}
}
