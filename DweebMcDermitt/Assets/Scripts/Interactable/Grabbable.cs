using UnityEngine;
using System.Collections;

public class Grabbable : InteractionTarget {
	
	override public void onInteract() {
		// float in front of the player or something idk
	}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}