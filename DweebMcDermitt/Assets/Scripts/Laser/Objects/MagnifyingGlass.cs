using UnityEngine;
using System.Collections;

public class MagnifyingGlass : LaserTarget {
	
	Vector3 newDir;
	
	public LineRenderer lineRenderer;
	public LaserShooter shooter;
	public void startUp()
	{
		if (gameObject.GetComponents<LineRenderer> ().Length <= 0)
			lineRenderer = gameObject.AddComponent<LineRenderer> ();
		else {
			lineRenderer = GetComponent<LineRenderer> ();
		}
		shooter = new LaserShooter (lineRenderer);
	}
	void Awake(){
		startUp ();
	}
	
	void Start(){
		startUp ();
	}
	override public void onLaserShot(LaserHitInfo laserHitInfo){
		lineRenderer.enabled = true;
		shoot (laserHitInfo);
	}
	
	override public void onLaserStay(LaserHitInfo laserHitInfo){
		lineRenderer.enabled = true;
		shoot (laserHitInfo);
	}
	
	override public void onLaserLeave(){
		lineRenderer.enabled = false;
		shooter.endFire ();
	}
	
	void shoot(LaserHitInfo laserHitInfo){
		//Debug.DrawRay (laserHitInfo.hitPoint, laserHitInfo.hitSurfaceNormal, Color.green);
		Vector3 inDir = -(laserHitInfo.EmitterPosition-laserHitInfo.hitPoint).normalized;
		//Debug.DrawRay (laserHitInfo.EmitterPosition, inDir * laserHitInfo.remainingDistance, Color.red);
		//newDir = Vector3.Reflect(inDir, laserHitInfo.hitSurfaceNormal).normalized;
		//Debug.Log ("in: " + laserHitInfo.EmitterPosition + " hit: " + laserHitInfo.hitPoint + " normal: " + laserHitInfo.hitSurfaceNormal + " outDir: " + newDir + " end: " + laserEndPoint);
		float angle = Vector3.Dot (inDir, transform.forward);

		{
			float ratio = Mathf.Min((Mathf.Abs (angle)),1.0f);
			Vector3 outdir = Mathf.Sign(angle)*transform.forward*ratio + inDir * (1.0f-ratio);
			Ray ray = new Ray (laserHitInfo.hitPoint * (1.0f-ratio) + transform.position * ratio, outdir);
			//Debug.DrawRay (ray.origin, ray.direction, Color.blue);
			//shooter.fireLaser(ray, laserHitInfo.remainingDistance);
			shooter.fireLaser (ray, laserHitInfo.remainingDistance);
		}
	}
	
	override public bool isTriggered(){
		return false;
	}
}