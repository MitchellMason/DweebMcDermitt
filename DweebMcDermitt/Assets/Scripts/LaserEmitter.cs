using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//<summary>
	//The distance after which the laser doesn't hit anything.
	//</summary>
	[SerializeField] private float laserClipDistance = 10.0f;
	
	//the object the laser is firing on
	LaserTarget storedLaserTarget;
	[SerializeField] private Transform CenterEyeAnchor;
	
	void Update () {
		
		//If we're firing the laser
		if (Input.GetAxis("FireLaser") >= 0.1f) {
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
		else{
			if(storedLaserTarget != null){
				storedLaserTarget.onLaserLeave();
				storedLaserTarget = null;
			}
		}
	}

	GameObject getObjectHit(){
		RaycastHit hit;
		Debug.DrawLine (CenterEyeAnchor.position, transform.position);
		if (Physics.Raycast (CenterEyeAnchor.position, transform.position - CenterEyeAnchor.position, out hit, laserClipDistance)) {
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
}
