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
		
		lShooter.fireLaser(lRay);
		rShooter.fireLaser(rRay);
	}
	
	override public void onLaserLeave(){}
	
	//Get the mesh positions of the new origins for the lasers.
	private Vec3Tuple getNewOnMeshPosition(LaserHitInfo info){
		Vector3 hitPoint = info.hitPoint;
		Vec3Tuple result = new Vec3Tuple(Vector3.zero, Vector3.zero);
		
	}
	
	//Get the directions for the lasers. 
	private Vec3Tuple getNewLaserDirections(LaserHitInfo info){
		return new Vec3Tuple (new Vector3(), new Vector3());
	}
	
	//utility method that rotates a vector3 around some other point
	private static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle){
		return angle * ( point - pivot) + pivot;
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