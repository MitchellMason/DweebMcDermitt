using UnityEngine;
using System.Collections;

public class Mirror : LaserTarget {
	public LaserTarget Target;

	override public void onLaserShot(Transform laserEmmitter){
		Target.onLaserShot (transform);
	}
	
	override public void onLaserStay(Transform laserEmmitter){
		Target.onLaserStay(transform);
	}
	
	override public void onLaserLeave(){
		Target.onLaserLeave();
	}
}
