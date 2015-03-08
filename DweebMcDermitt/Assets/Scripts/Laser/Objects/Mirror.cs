using UnityEngine;
using System.Collections;

public class Mirror : LaserTarget {
	private LaserShooter shooter;
	
	Vector3 newDir;
	Vector3 laserEndPoint;

	void Start(){
		shooter = new LaserShooter(this.GetComponent<LineRenderer>());
	}
	
	override public void onLaserShot(LaserHitInfo laserHitInfo){
		shoot (laserHitInfo);
	}
	
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		shoot (laserHitInfo);
	}
	
	override public void onLaserLeave(){
		if(!this.Equals(shooter.getStoredLaserObject())) shooter.endFire();
	}

	void shoot(LaserHitInfo laserHitInfo){
		Vector3 inDir = (laserHitInfo.laserEmitter-laserHitInfo.hitPoint).normalized;
		newDir = Vector3.Reflect(laserHitInfo.laserEmitter - laserHitInfo.hitPoint, laserHitInfo.hitSurfaceNormal).normalized;
		laserEndPoint = laserHitInfo.hitPoint + (newDir * laserHitInfo.remainingDistance);
		Debug.Log ("in: " + laserHitInfo.laserEmitter + " hit: " + laserHitInfo.hitPoint + " normal: " + laserHitInfo.hitSurfaceNormal + " outDir: " + newDir + " end: " + laserEndPoint);
		
		shooter.fireLaser(laserHitInfo.hitPoint, newDir, laserHitInfo.remainingDistance);
	}
	
	override public bool isTriggered(){
		return false;
	}
}