using UnityEngine;
using System.Collections;

public static class LaserUtils{
	public const float LASER_WIDTH = 0.025f;
	public static Color LASER_COLOR = new Color(255f,255f,255f);
	public static void fireLaser(LineRenderer lineRenderer, Vector3 start, Vector3 end){
		lineRenderer.SetVertexCount (2);
		lineRenderer.SetWidth(LASER_WIDTH, LASER_WIDTH);
		lineRenderer.SetPosition (0, start);
		lineRenderer.SetPosition (1, end);
	}
	
	public static LaserHitInfo toLaserHitInfo(RaycastHit r, Vector3 shooterTransform){
		LaserHitInfo l = new LaserHitInfo ();
		l.hitPoint = r.point;
		l.hitSurfaceNormal = r.normal;//new Vector3(r.normal.x,-r.normal.y,r.normal.z);//r.normal;
		l.laserEmitter = shooterTransform;
		l.remainingDistance = Vector3.Distance (shooterTransform, r.collider.transform.position);
		return l;
	}
}

public struct LaserHitInfo{
	public Vector3 laserEmitter;
	public Vector3 hitPoint;
	public Vector3 hitSurfaceNormal;
	public float remainingDistance;
}