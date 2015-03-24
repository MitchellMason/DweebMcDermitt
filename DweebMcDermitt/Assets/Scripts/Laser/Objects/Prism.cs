 using UnityEngine;
using System.Collections;

public class Prism : LaserTarget {
	LineRenderer first;
	LineRenderer second;
	
	LaserShooter lShooter;
	LaserShooter rShooter;
	
	Vec3Tuple start;
	Vec3Tuple dir;
	Ray lRay;
	Ray rRay;
	
	void Start(){
		first    = this.GetComponent<LineRenderer>();
		second   = this.GetComponent<LineRenderer>();
		lRay     = new Ray();
		rRay     = new Ray();
		lShooter = new LaserShooter(first);
		rShooter = new LaserShooter(second);
	}
	
	override public void onLaserShot (LaserHitInfo laserHitInfo){
		//RayCastHitInfo hit;
		Vector3 direction = laserHitInfo.EmitterPosition - laserHitInfo.hitPoint;
		
	}
	
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		start = getNewOnMeshPosition(laserHitInfo);
		dir  = getNewLaserDirections(laserHitInfo);
		
		lRay.origin = start.One;
		lRay.direction = dir.One;
		
		rRay.origin = start.Two;
		lRay.direction = dir.Two;
		
//		lShooter.fireLaser(lRay);
//		rShooter.fireLaser(rRay);
	}
	
	override public void onLaserLeave(){}
	
	//Get the mesh positions of the new origins for the lasers.
	private Vec3Tuple getNewOnMeshPosition(LaserHitInfo info){
		return new Vec3Tuple (new Vector3(), new Vector3());
	}
	
	//Get the directions for the lasers. 
	private Vec3Tuple getNewLaserDirections(LaserHitInfo info){
		return new Vec3Tuple (new Vector3(), new Vector3());
	}
	
	private class Vec3Tuple{
		public Vector3 One;
		public Vector3 Two;
		public Vec3Tuple(Vector3 l, Vector3 r){
			this.One  = l;
			this.Two = r;
		}
	}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}
