using UnityEngine;
using System.Collections;

public class Button : InteractionTarget {
	private bool pressed = false;
	
	override public void onInteract() {
		// trigger a sweet lift or something wild
	}
	
	override public bool isTriggered(){
		return pressed;
	}
}
