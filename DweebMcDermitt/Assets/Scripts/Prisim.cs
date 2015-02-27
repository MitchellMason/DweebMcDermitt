using UnityEngine;
using System.Collections;

public class Prisim : LaserTarget {

	[SerializeField] private LaserTarget t1;
	[SerializeField] private LaserTarget t2;

	override public void onLaserShot (Transform laserEmmitter){
		t1.onLaserShot (laserEmmitter);
		t2.onLaserShot (laserEmmitter);
	}
	
	override public void onLaserStay(Transform laserEmmitter){
		t1.onLaserStay(laserEmmitter);
		t2.onLaserStay(laserEmmitter);
	}
	
	override public void onLaserLeave(){
		t1.onLaserLeave();
		t2.onLaserLeave();
	}
}
