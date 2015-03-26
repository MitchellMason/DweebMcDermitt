using UnityEngine;
using System.Collections;

public class Mirror : LaserTarget {
	private LaserShooter shooter;
	private LineRenderer lineRenderer;
	Vector3 newDir;
	Vector3 laserEndPoint;
	bool justLeft = false;

	void Start(){
		lineRenderer = this.gameObject.AddComponent<LineRenderer>();
		shooter = new LaserShooter (lineRenderer);
	}
	
	override public void onLaserShot(LaserHitInfo laserHitInfo){
		shoot (laserHitInfo);
	}
	
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		shoot (laserHitInfo);
	}
	
	override public void onLaserLeave(){
		/*if (!justLeft) {
			shooter.endFire ();
			justLeft = true;
		}*/
		lineRenderer.SetWidth (0, 0);
	}

	void shoot(LaserHitInfo laserHitInfo){
		justLeft = false;
		Debug.DrawRay (laserHitInfo.hitPoint, laserHitInfo.hitSurfaceNormal, Color.green);
		Vector3 inDir = -(laserHitInfo.EmitterPosition-laserHitInfo.hitPoint).normalized;
		Debug.DrawRay (laserHitInfo.EmitterPosition, inDir * laserHitInfo.remainingDistance, Color.red);
		newDir = Vector3.Reflect(inDir, laserHitInfo.hitSurfaceNormal).normalized;
		//Debug.Log ("in: " + laserHitInfo.EmitterPosition + " hit: " + laserHitInfo.hitPoint + " normal: " + laserHitInfo.hitSurfaceNormal + " outDir: " + newDir + " end: " + laserEndPoint);

		Ray ray = new Ray (laserHitInfo.hitPoint, newDir);
		Debug.DrawRay (ray.origin, ray.direction, Color.blue);
		//shooter.fireLaser(ray, laserHitInfo.remainingDistance);
		shooter.fireLaser(ray, LaserUtils.LASER_DISTANCE);
	}
	
	override public bool isTriggered(){
		return false;
	}
}