using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LaserUtils{
	public const float LASER_WIDTH = 0.025f;
	public static Color LASER_COLOR = new Color(255f,255f,255f);
	public static float LASER_DURATION = 6;
	
	public static LaserHitInfo toLaserHitInfo(RaycastHit r, Vector3 pos){
		LaserHitInfo l = new LaserHitInfo ();
		l.hitPoint = r.point;
		l.hitSurfaceNormal = r.normal;
		l.EmitterPosition = pos;
		l.remainingDistance = Vector3.Distance (pos, r.collider.transform.position);
		return l;
	}
}

public class LaserShooter{
	LaserTarget storedObject; //The laser target we're in the process of shooting
	LineRenderer lineRenderer;
	
	public LaserShooter(LineRenderer renderer){
		lineRenderer = renderer;
		lineRenderer.castShadows = false;
		lineRenderer.receiveShadows = false;
		lineRenderer.SetVertexCount (2);
		lineRenderer.SetWidth(LaserUtils.LASER_WIDTH, LaserUtils.LASER_WIDTH);
	}
	public void fireLaser(Ray ray, float distance){
		fireLaser (ray, distance, true);
	}

	public void fireLaser(Ray ray, float distance, bool draw){
		//Debug.Log ("Firing with origin: " + ray.origin + " direction: " + ray.direction + " distance: " + distance);
		lineRenderer.SetWidth(LaserUtils.LASER_WIDTH, LaserUtils.LASER_WIDTH);
		//Perform the shot
		RaycastHit hit;
		GameObject justHit;
		if (Physics.Raycast (ray.origin, ray.direction, out hit, distance)) {
			//Debug.Log ("Mirror: Hit " + hit.collider.gameObject.name);
			justHit = hit.collider.gameObject;

			//Draw the laser shot
			if(draw){
				lineRenderer.SetVertexCount (2);
				lineRenderer.SetPosition (0, ray.origin);
				lineRenderer.SetPosition (1, hit.point);
			}
		}
		else{
			//Debug.Log("Mirror: Nothing hit.");
			justHit = null;
			//Draw the laser shot
			if(draw){
				lineRenderer.SetVertexCount (2);
				lineRenderer.SetPosition (0, ray.origin);
				lineRenderer.SetPosition(1, (ray.direction * distance) + ray.origin);
			}
		}
		
		//Handle the results
		if(justHit != null){
			LaserTarget justHitLaserTarget = justHit.GetComponent<LaserTarget>();
			if(justHitLaserTarget != null){
				if(justHitLaserTarget == storedObject){
					storedObject.onLaserStay(LaserUtils.toLaserHitInfo(hit, ray.origin));
				}
				else{
					justHitLaserTarget.onLaserShot(LaserUtils.toLaserHitInfo(hit, ray.origin));
					if(storedObject != null){
						//Debug.Log ("not the same object.");
						storedObject.onLaserLeave();
					}
					storedObject = justHitLaserTarget;
				}
			}
			else{
				//Debug.Log("Not a laser target");
				endFire();
			}
		}
		else{
			//Debug.Log ("Didn't hit anything");
			endFire();
		}
	}
	
	public void endFire(){
		if(storedObject != null){
			//Debug.Log ("End fire called for " + storedObject.name);
			storedObject.onLaserLeave();
			storedObject = null;
		}
		else{
			//Debug.Log ("End fire called. No impact");
		}
	}
	
	public LaserTarget getStoredLaserObject(){
		return storedObject;
	}
}

public class LaserHitInfo{
	public Vector3 EmitterPosition;
	public Vector3 hitPoint;
	public Vector3 hitSurfaceNormal;
	public float remainingDistance;
	List<Vector3> points = new List<Vector3>();
}