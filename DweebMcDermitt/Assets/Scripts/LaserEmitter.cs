using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//The distance after which the laser doesn't hit anything.
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
				LaserTarget justHitLaserTarget = justHit.GetComponent<LaserTarget>();
				
				//is it a laserTarget? 
				if(justHitLaserTarget != null){
					//have we hit this thing already?
					if(justHitLaserTarget == storedLaserTarget){
						storedLaserTarget.onLaserStay(this.transform);
					} 
					else{
						justHitLaserTarget.onLaserShot(this.transform);
						storedLaserTarget = justHitLaserTarget;
					}
				}
				//if the target isn't a laser target, call laserleave on the stored object, if it exists
				else{
					if(storedLaserTarget != null){
						storedLaserTarget.onLaserLeave();
						storedLaserTarget = null;
					}
				}
			}
			//if we didn't hit anything, call laserleave on the stored object, if it exists
			else{
				if(storedLaserTarget != null){
					storedLaserTarget.onLaserLeave();
					storedLaserTarget = null;
				}
			}
		}
		//if we aren't firing the laser, call laserleave on the stored object, if it exists
		else{
			if(storedLaserTarget != null){
				storedLaserTarget.onLaserLeave();
				storedLaserTarget = null;
			}
		}
	}

	//casts a ray from the center eye anchor to the crosshair and returns the game object hit
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
