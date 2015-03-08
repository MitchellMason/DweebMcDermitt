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
		Debug.DrawRay (laserHitInfo.hitPoint, laserHitInfo.hitSurfaceNormal, Color.green);
		Vector3 inDir = -(laserHitInfo.EmitterPosition-laserHitInfo.hitPoint).normalized;
		Debug.DrawRay (laserHitInfo.EmitterPosition, inDir * laserHitInfo.remainingDistance, Color.red);
		newDir = Vector3.Reflect(inDir, laserHitInfo.hitSurfaceNormal).normalized;
		//Debug.Log ("in: " + laserHitInfo.EmitterPosition + " hit: " + laserHitInfo.hitPoint + " normal: " + laserHitInfo.hitSurfaceNormal + " outDir: " + newDir + " end: " + laserEndPoint);

		Ray ray = new Ray (laserHitInfo.hitPoint, newDir);
		Debug.DrawRay (ray.origin, ray.direction, Color.blue);
		shooter.fireLaser(ray, laserHitInfo.remainingDistance);
	}
	
	override public bool isTriggered(){
		return false;
	}
}