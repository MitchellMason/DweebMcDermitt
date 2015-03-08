using UnityEngine;
using System.Collections;

public class MagnifyingGlass : LaserTarget {
	override public void onLaserShot(LaserHitInfo laserHitInfo){}
	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}