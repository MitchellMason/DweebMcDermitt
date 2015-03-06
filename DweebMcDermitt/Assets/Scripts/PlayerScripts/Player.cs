using UnityEngine;
using System.Collections;

public class Player : LaserTarget {
	override public void onLaserShot(LaserHitInfo laserHitInfo){}
	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){}
}