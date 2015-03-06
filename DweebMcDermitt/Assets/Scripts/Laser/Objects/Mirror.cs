using UnityEngine;
using System.Collections;

public class Mirror : LaserTarget {
	private LineRenderer lr;

	void Start(){
		//Set the line renderer
		lr = this.GetComponent<LineRenderer> ();
		lr.SetWidth (LaserUtils.LASER_WIDTH, LaserUtils.LASER_WIDTH);
	}

	override public void onLaserShot(LaserHitInfo laserHitInfo){
		lr.enabled = true;
		drawLaser (laserHitInfo);
	}
	
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		this.onLaserShot (laserHitInfo);
		drawLaser (laserHitInfo);
	}
	
	override public void onLaserLeave(){
		lr.enabled = false;
	}

	private void drawLaser(LaserHitInfo laserHitInfo){
		Vector3 inDir = (laserHitInfo.laserEmitter.position - laserHitInfo.hitPoint).normalized;
		Vector3 outDir = inDir + (2f * Vector3.Dot(-laserHitInfo.hitSurfaceNormal, inDir) * laserHitInfo.hitSurfaceNormal);
		//fire the laser
		LaserUtils.fireLaser (
			lr, 
			laserHitInfo.hitPoint, 
			outDir
		);
	}
}