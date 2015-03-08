using UnityEngine;
using System.Collections;

public class WeightedButton : PositionTarget {
	private bool pressed = false;
	
	void OnTriggerEnter(Collider col){
		if(col.collider.tag.Equals("Player")){
			pressed = true;
		}
		else{
			pressed = false;
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
