using UnityEngine;
using System.Collections;

public class Door : TriggerTarget {
	
	[SerializeField] Animation anim;
	bool hasAnimated = false;

	void Start(){
		anim.wrapMode = WrapMode.ClampForever;
	}

	override public void onTrigger(MonoBehaviour t) {
		Debug.Log ("Playing");
		if (!hasAnimated) {
			anim.Play (AnimationPlayMode.Stop);
			hasAnimated = true;
		}
		else{
			//reverse the animation
			hasAnimated = false;
		}
	}
}
