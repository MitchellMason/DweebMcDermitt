using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LaserUtils{
	public const float LASER_WIDTH = 0.025f;
	public static Color LASER_COLOR = new Color(255f,255f,255f);
	
	public static LaserHitInfo toLaserHitInfo(RaycastHit r, Vector3 pos){
		LaserHitInfo l = new LaserHitInfo ();
		l.hitPoint = r.point;
		l.hitSurfaceNormal = r.normal;
		l.laserEmitter = pos;
		l.remainingDistance = Vector3.Distance (pos, r.collider.transform.position);
		return l;
	}
}

public class LaserShooter{
	LaserTarget storedObject;
	LineRenderer lineRenderer;
	
	public LaserShooter(LineRenderer renderer){
		lineRenderer = renderer;
	}
	
	public void fireLaser(Vector3 start, Vector3 dir, float distance){
		//Draw the laser shot
		lineRenderer.enabled = true;
		lineRenderer.SetVertexCount (2);
		lineRenderer.SetWidth(LaserUtils.LASER_WIDTH, LaserUtils.LASER_WIDTH);
		lineRenderer.SetPosition (0, start);
		lineRenderer.SetPosition (1, start * distance);
		
		//Perform the shot
		RaycastHit hit;
		GameObject justHit;
		if (Physics.Raycast (start, dir, out hit, distance)) {
			justHit = hit.collider.gameObject;
		}
		else{
			justHit = null;
		}
		
		//Handle the results
		if(justHit != null){
			LaserTarget justHitLaserTarget = justHit.GetComponent<LaserTarget>();
			if(justHitLaserTarget != null){
				if(justHitLaserTarget.Equals(storedObject)){
					justHitLaserTarget.onLaserStay(LaserUtils.toLaserHitInfo(hit, start));
				}
				else{
					justHitLaserTarget.onLaserShot(LaserUtils.toLaserHitInfo(hit, start));
					storedObject = justHitLaserTarget;
				}
			}
			else{
				endFire();
			}
		}
		else{
			endFire();
		}
	}
	
	public void endFire(){
		lineRenderer.enabled = false;
		if(storedObject != null){
			storedObject.onLaserLeave();
			storedObject = null;
		}
	}
	
	public LaserTarget getStoredLaserObject(){
		return storedObject;
	}
}

public class LaserHitInfo{
	public Vector3 laserEmitter;
	public Vector3 hitPoint;
	public Vector3 hitSurfaceNormal;
	public float remainingDistance;
	List<Vector3> points = new List<Vector3>();
}