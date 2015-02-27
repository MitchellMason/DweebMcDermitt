using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//<summary>
	//The distance after which the laser doesn't hit anything.
	//</summary>
	[SerializeField] private float laserClipDistance = 10.0f;

	private bool fireButtonStateThisFrame = false;
	private bool fireButtonStateLastFrame = false;
	
	//the object the laser is firing on
	LaserTarget storedLaserTarget;
	
	void Update () {
		fireButtonStateThisFrame = Input.GetAxis("FireLaser") <= 0.1f;
		Debug.DrawRay (transform.position, crosshair.transform.position);
		if (fireButtonStateThisFrame && !fireButtonStateLastFrame) {
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
		if (Physics.Raycast (transform.position, crosshair.transform.position, out hit, laserClipDistance)) {
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
}
