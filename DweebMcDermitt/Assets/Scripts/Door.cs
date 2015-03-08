using UnityEngine;
using System.Collections;

public class Door : TriggerTarget {
	
	Animation anim;
	bool hasAnimated = false;
	string animationOpenDoor = "Take 001";
	string animationCloseDoor = "";
	
	override public void onTrigger(MonoBehaviour t) {
		anim = this.GetComponent<Animation>();

		if (!hasAnimated) {
			this.PlayAnimation (animationOpenDoor);
		}
		//else {
		//this.PlayAnimation (animationCloseDoor);
		//}
	}

	public void PlayAnimation(string s) {
		anim.Blend(s);
	}
}
