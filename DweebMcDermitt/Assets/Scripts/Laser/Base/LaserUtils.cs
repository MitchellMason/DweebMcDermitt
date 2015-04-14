using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LaserUtils{
	public const float LASER_WIDTH = 0.25f;
	public static Color LASER_COLOR = new Color(255f,255f,255f);
	public static float LASER_DURATION = 6;

	public static float LASER_DISTANCE = 50.0f;

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

//	GameObject burnMarks;
	
	public LaserShooter(LineRenderer renderer){
		renderer.enabled = true;
		lineRenderer = renderer;
		Material mat = lineRenderer.material;
		if (mat.name != "Custom/LaserMat"){
			Shader ShaderToUse = Shader.Find ("Custom/LaserShad");
			mat = new Material(ShaderToUse);
			lineRenderer.material = mat;
		}
		lineRenderer.useWorldSpace = true;
		lineRenderer.castShadows = true;
		lineRenderer.receiveShadows = false;
		lineRenderer.SetVertexCount (2);
		lineRenderer.SetWidth(LaserUtils.LASER_WIDTH, LaserUtils.LASER_WIDTH);
	}

	public void fireLaser(Ray ray, float distance){
		fireLaser (ray, distance, true);
	}
	
	//fire two laser beams, useful for prisms
	public void splitLaser(Ray first, Ray second, float distance1, float distance2){
		//let the logic do it's thing
		fireLaser(first, distance1, true);
		fireLaser(second, distance2, true);
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

			// make a burn mark on burnable stuff
			if (hit.transform.tag == "burnable") {
				Debug.Log ("Firing at a burnable object");
				GameObject burnClone = (GameObject)GameObject.Instantiate(Resources.Load("burnMarks"), hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
//				GameObject.Instantiate(Resources.Load("burnMarks"), hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
				GameObject.Destroy(burnClone.gameObject, 5);
			}
			//Draw the laser shot
			if(draw){
				Material mat = lineRenderer.material;
				mat.SetFloat ("_Width", LaserUtils.LASER_WIDTH/2.0f);
				mat.SetVector ("_Start", ray.origin);
				mat.SetVector ("_End", (ray.direction * hit.distance) + ray.origin);
				lineRenderer.SetVertexCount (2);
				lineRenderer.SetPosition (0, ray.origin);
				lineRenderer.SetPosition (1, (ray.direction * hit.distance) + ray.origin);
			}
		}
		else{
			//Debug.Log("Mirror: Nothing hit.");
			justHit = null;
			//Draw the laser shot
			if(draw){
				
				Material mat = lineRenderer.material;
				mat.SetFloat ("_Width", LaserUtils.LASER_WIDTH/2.0f);
				mat.SetVector ("_Start", ray.origin);
				mat.SetVector ("_End", (ray.direction * distance) + ray.origin);

				lineRenderer.SetVertexCount (2);
				lineRenderer.SetPosition(0, ray.origin);
				lineRenderer.SetPosition(1, (ray.direction * distance) + ray.origin);

			}
		}
		
		//Handle the results
		if(justHit != null){
			LaserTarget justHitLaserTarget = justHit.GetComponent<LaserTarget>();
			if(justHitLaserTarget != null){
				LaserHitInfo hitInfo = LaserUtils.toLaserHitInfo(hit, ray.origin);
				if(justHitLaserTarget == storedObject){
					storedObject.onLaserStay(hitInfo);
				}
				else{
					justHitLaserTarget.onLaserShot(hitInfo);
					if(storedObject != null){
						storedObject.onLaserLeave();
					}
					storedObject = justHitLaserTarget;
				}
//				Decal burn = GameObject.Instantiate(burnMarks, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as Decal;
//				burn.init(hitInfo.hitPoint, hitInfo.hitSurfaceNormal);
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