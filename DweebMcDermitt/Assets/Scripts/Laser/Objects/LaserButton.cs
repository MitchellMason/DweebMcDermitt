using UnityEngine;
using System.Collections;

public class LaserButton : LaserTarget {
	
	[SerializeField] private TriggerTarget target;
	[SerializeField] private AudioSource sound;
	[SerializeField] private Animation anim;

	private bool beingShot;
	private Color originalColor;
	
	void Start(){
		beingShot = false;
	}
	
	override public void onLaserShot(LaserHitInfo laserHitInfot){
		beingShot = true;
		if(target != null) target.onTrigger(this);
		sound.Play();
		anim.Play (AnimationPlayMode.Stop);
		Debug.Log ("LaserButton shot");
	}
	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){
		beingShot = false;
		sound.Stop ();
	}
	
	override public bool isTriggered(){
		return beingShot;
	}
}