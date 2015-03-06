 using UnityEngine;
using System.Collections;

public class Prism : LaserTarget {

	[SerializeField] private LaserTarget t1;
	[SerializeField] private LaserTarget t2;

	override public void onLaserShot (LaserHitInfo laserHitInfo){
		t1.onLaserShot (laserHitInfo);
		t2.onLaserShot (laserHitInfo);
	}
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		t1.onLaserStay(laserHitInfo);
		t2.onLaserStay(laserHitInfo);
	}
	override public void onLaserLeave(){
		t1.onLaserLeave();
		t2.onLaserLeave();
	}
}
