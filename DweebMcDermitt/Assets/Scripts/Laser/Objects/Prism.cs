 using UnityEngine;
using System.Collections;

public class Prism : LaserTarget {

	LaserShooter lShooter;
	LaserShooter rShooter;
	
	override public void onLaserShot (LaserHitInfo laserHitInfo){
		RayCastHitInfo hit;
		Vector3 direction = laserHitInfo.EmitterPosition - laserHitInfo.hitPoint;
		
	}
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		
	}
	override public void onLaserLeave(){
		
	}
	
	//Get the mesh positions of the new origins for the lasers.
	private Vec3Tuple getNewOnMeshPosition(LaserHitInfo info){
		
	}
	
	//Get the directions for the lasers. 
	private Vec3Tuple getNewLaserDirections(LaserHitInfo info){
		
	}
	
	private class Vec3Tuple{
		public Vector3 left;
		public Vector3 right;
		public Vec3Tuple(Vector3 l, Vector3 r){
			this.left = l;
			this.right = r;
		}
	}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}
