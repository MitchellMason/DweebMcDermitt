using UnityEngine;
using System.Collections;

public class Mirror : LaserTarget {
	private LineRenderer lr;
	
	Vector3 newDir;
	Vector3 laserEndPoint;
	
	void Start(){
		//Set the line renderer
		if (gameObject.GetComponents<LineRenderer>().Length == 0)
		{
			gameObject.AddComponent<LineRenderer>();
		}
		lr = gameObject.GetComponent<LineRenderer> ();
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

	void drawLaser(LaserHitInfo laserHitInfo){
		Vector3 inDir = (laserHitInfo.laserEmitter-laserHitInfo.hitPoint).normalized;
		newDir = -Vector3.Reflect(laserHitInfo.laserEmitter - laserHitInfo.hitPoint, laserHitInfo.hitSurfaceNormal).normalized;
		laserEndPoint = laserHitInfo.hitPoint + (newDir * laserHitInfo.remainingDistance);
		Debug.Log ("in: " + laserHitInfo.laserEmitter + " hit: " + laserHitInfo.hitPoint + " normal: " + laserHitInfo.hitSurfaceNormal + " outDir: " + newDir + " end: " + laserEndPoint);
		
		//fire the laser
		LaserUtils.fireLaser (
			lr, 
			laserHitInfo.hitPoint, 
			laserEndPoint
		);
	}
	
	/*
					Vector3 inDir = (points[points.Count - 2]-points[points.Count - 1]).normalized;
					Vector3 outDir = (2.0f * Vector3.Dot(inDir, hit.normal) * hit.normal - inDir).normalized;
					justHit = getObjectHit(points[points.Count-1]+outDir*0.01f, outDir);
					if (justHit == null)
					{
						points.Add (points[points.Count-1] + outDir * 100.0f);
						break;
					}
					points.Add(hit.point);
					if (points.Count >= maxbounces)
						break;
					justHitLaserTarget = justHit.GetComponent<Mirror>();

				}
				lr.SetVertexCount (points.Count-1);

				//lineRenderer.SetWidth(LASER_WIDTH, LASER_WIDTH);
				for (int i = 1; i < points.Count; ++i)
				{
					lr.SetPosition(i-1, points[i]);
				}
				lr.enabled = true;
				*/
	
	override public bool isTriggered(){
		return false;
	}
}