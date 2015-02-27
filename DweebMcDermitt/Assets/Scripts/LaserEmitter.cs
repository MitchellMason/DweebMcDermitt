using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//<summary>
	//The distance after which the laser doesn't hit anything.
	//</summary>
	[SerializeField] private float laserClipDistance = 10.0f;

	//The direction of fire
	private Vector3 forward;
	
	//the object the laser is firing on
	LaserTarget storedLaserTarget;
	
	void Update () {
		forward = transform.TransformVector (Vector3.forward);
		Debug.DrawRay (transform.position, forward * 10);
		
		if (Input.GetAxis("FireLaser") >= 0.1f) {
			Debug.Log ("Firing.");
			
			//first, see if we hit anything
			GameObject justHit = getObjectHit();
			
			//Check to be sure we hit something
			if(justHit != null){
				Debug.Log ("Hit " + justHit.name);
				LaserTarget justHitLaserTarget = justHit.GetComponent<LaserTarget>();
				
				//is it a laserTarget? 
				if(justHitLaserTarget != null){
					Debug.Log (justHit.name + " is a laser Target.");
					//have we hit this thing already?
					if(justHitLaserTarget == storedLaserTarget){
						storedLaserTarget.onLaserStay(this.transform);
					} 
					else{
						justHitLaserTarget.onLaserShot(this.transform);
						storedLaserTarget = justHitLaserTarget;
					}
				}
			}
			else{
				if(storedLaserTarget != null){
					storedLaserTarget.onLaserLeave();
					storedLaserTarget = null;
				}
			}
		}
		else{
			if(storedLaserTarget != null){
				storedLaserTarget.onLaserLeave();
				storedLaserTarget = null;
			}
		}
	}

	GameObject getObjectHit(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, forward, out hit, laserClipDistance)) {
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
}
