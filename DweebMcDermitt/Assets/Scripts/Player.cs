using UnityEngine;
using System.Collections;

public class Player : LaserTarget {
	override public void onLaserShot(Transform laserEmmitter){}
	override public void onLaserStay(Transform laserEmmitter){}
	override public void onLaserLeave(){}
}