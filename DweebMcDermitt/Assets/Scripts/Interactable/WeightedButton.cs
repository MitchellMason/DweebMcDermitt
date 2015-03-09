using UnityEngine;
using System.Collections;

public class WeightedButton : PositionTarget {
	private bool pressed = false;
	private float weightThreshold = 10.0f;
	
	void OnTriggerEnter(Collider col){
		pressed = false;
		if(col.rigidbody != null && col.rigidbody.mass > weightThreshold){
			pressed = true;
		}
		if(col.GetComponent<Player>() != null){
			pressed = true;
		}
	}
	
	void OnTriggerExit(Collider col){
		pressed = false;
	}
	
	override public bool isTriggered(){
		return pressed;
	}
	
	override public void onPosition() {
		// Get off of me fatty I'm trying to live my life.
	}
}
