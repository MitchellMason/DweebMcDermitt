using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//The distance after which the laser doesn't hit anything.
	[SerializeField] private float laserClipDistance = 10.0f;
	
	//the object the laser is firing on
	LaserTarget storedLaserTarget;
	[SerializeField] private Transform CenterEyeAnchor;
	//Raycase information
	private RaycastHit hit;

	float timer = LaserUtils.LASER_DURATION;
	bool okayToFire = false;

	void Update () {
		if (timer > 0) {
			okayToFire = true;
		} else {
			okayToFire = false;
		}

		//If we're firing the laser
		if (Input.GetAxis("FireLaser") >= 0.1f  && okayToFire) {
			timer -= Time.deltaTime;

			if (timer <= 0) {
				okayToFire = false;
			}

			//first, see if we hit anything
			GameObject justHit = getObjectHit ();
			
			//Check to be sure we hit something
			if (justHit != null) {
				LaserTarget justHitLaserTarget = justHit.GetComponent<LaserTarget> ();
				
				//is it a laserTarget? 
				if (justHitLaserTarget != null) {
					//have we hit this thing already?
					if (justHitLaserTarget == storedLaserTarget) {
						if (hit.point.Equals (CenterEyeAnchor.position))
							Debug.LogError ("hitpoint and pos the same");
						storedLaserTarget.onLaserStay (LaserUtils.toLaserHitInfo (hit, CenterEyeAnchor.position));
					} else {
						justHitLaserTarget.onLaserShot (LaserUtils.toLaserHitInfo (hit, CenterEyeAnchor.position));
						storedLaserTarget = justHitLaserTarget;
					}
				}
				//if the target isn't a laser target, call laserleave on the stored object, if it exists
				else {
					if (storedLaserTarget != null) {
						storedLaserTarget.onLaserLeave ();
						storedLaserTarget = null;
					}
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

		if (Input.GetMouseButtonUp (0)) {
			timer = LaserUtils.LASER_DURATION;
		}
	}


	//casts a ray from the center eye anchor to the crosshair and returns the game object hit
	GameObject getObjectHit(){
		if (Physics.Raycast (CenterEyeAnchor.position, transform.position - CenterEyeAnchor.position, out hit, laserClipDistance)) {
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
}