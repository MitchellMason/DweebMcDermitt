 using UnityEngine;
using System.Collections;

public class Prism : LaserTarget {
	[SerializeField] LineRenderer first;
	[SerializeField] LineRenderer second;
	
	LaserShooter lShooter;
	LaserShooter rShooter;
	
	Vec3Tuple onMesh;
	Vec3Tuple dir;
	Ray lRay;
	Ray rRay;
	
	void Start(){
		lRay     = new Ray();
		rRay     = new Ray();
		lShooter = new LaserShooter(first.GetComponent<LineRenderer>());
		rShooter = new LaserShooter(second.GetComponent<LineRenderer>());
	}
	
	override public void onLaserShot (LaserHitInfo laserHitInfo){
		this.onLaserStay (laserHitInfo);
	}
	
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		first.enabled = true;
		second.enabled = true;
		onMesh = getNewOnMeshPosition(laserHitInfo);
		dir  = getNewLaserDirections(laserHitInfo);

		Vector3 planePosition = this.transform.position;
		planePosition.y = laserHitInfo.hitPoint.y;

		lRay.origin = onMesh.One;
		lRay.direction = onMesh.One - planePosition;

		rRay.origin = onMesh.Two;
		rRay.direction = onMesh.Two - planePosition;

		Debug.DrawRay (lRay.origin, lRay.direction, Color.yellow);
		Debug.DrawRay (rRay.origin, rRay.direction, Color.green);

		Debug.DrawLine (lRay.origin, laserHitInfo.hitPoint, Color.yellow);
		Debug.DrawLine (rRay.origin, laserHitInfo.hitPoint, Color.green);

		lShooter.fireLaser(lRay, 50.0f);
		rShooter.fireLaser(rRay, 50.0f);
	}
	
	override public void onLaserLeave(){
		if (first.enabled) {
			first.enabled = false;
			second.enabled = false;
			lShooter.endFire ();
			rShooter.endFire ();
		}
	}
	
	//Get the mesh positions of the new origins for the lasers.
	private Vec3Tuple getNewOnMeshPosition(LaserHitInfo info){
		Vector3 hitPoint = info.hitPoint;
		Vec3Tuple result = new Vec3Tuple(Vector3.zero, Vector3.zero);
		result.One = RotateAroundPoint (hitPoint, this.transform.position, Quaternion.AngleAxis ( 120.0f, Vector3.up));
		result.Two = RotateAroundPoint (hitPoint, this.transform.position, Quaternion.AngleAxis (-120.0f, Vector3.up));
		return result;
	}
	
	//Get the directions for the lasers. 
	private Vec3Tuple getNewLaserDirections(LaserHitInfo info){
		Vector3 hitNorm = info.hitSurfaceNormal;
		Vec3Tuple result = new Vec3Tuple(Vector3.zero, Vector3.zero);
		result.One = (RotateAroundPoint (hitNorm, this.transform.position, Quaternion.AngleAxis ( 120.0f, Vector3.up))).normalized;
		result.Two = (RotateAroundPoint (hitNorm, this.transform.position, Quaternion.AngleAxis (-120.0f, Vector3.up))).normalized;
		return result;
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