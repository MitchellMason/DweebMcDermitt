using UnityEngine;
using System.Collections;

public class LaserButton : LaserTarget {

	public TriggerTarget target;
	public bool onShot;

	void Start(){
		onShot = false;
	}

	void Update(){
		if (onShot) {
			target.onTrigger(this);
		}
	}
	
	override public void onLaserShot(LaserHitInfo laserHitInfot){
		onShot = true;
	}
	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){}
}