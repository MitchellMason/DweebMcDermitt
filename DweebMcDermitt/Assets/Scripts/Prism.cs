 using UnityEngine;
using System.Collections;

public class Prism : LaserTarget {

	[SerializeField] private LaserTarget t1;
	[SerializeField] private LaserTarget t2;

	override public void onLaserShot (Transform laserEmmitter){
		t1.onLaserShot (transform);
		t2.onLaserShot (transform);
	}
	override public void onLaserStay(Transform laserEmmitter){
		t1.onLaserStay(transform);
		t2.onLaserStay(transform);
	}
	override public void onLaserLeave(){
		t1.onLaserLeave();
		t2.onLaserLeave();
	}
}
