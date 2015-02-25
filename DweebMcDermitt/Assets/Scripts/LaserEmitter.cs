using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//<summary>
	//The distance after which the laser doesn't hit anything.
	//</summary>
	[SerializeField] private float laserClipDistance = 10.0f;
	[SerializeField] private Crosshair crosshair;

	private bool fireButtonStateThisFrame = false;
	private bool fireButtonStateLastFrame = false;
	
	void Update () {
		fireButtonStateThisFrame = Input.GetAxis("FireLaser") <= 0.1f;
		Debug.DrawRay (transform.position, crosshair.transform.position);
		if (fireButtonStateThisFrame && !fireButtonStateLastFrame) {
			//first, see if we hit anything
			GameObject lasered = getObjectHit();
			if(lasered != null){
				//next, see if we hit something that reacts to lasers
				LaserTarget possibleHit = lasered.GetComponent<LaserTarget>();

				if(possibleHit != null){
					Debug.Log ("Hit valid laser target " + lasered.name);
					possibleHit.onLaserShot();
				}
				else{
					Debug.Log ("Hit " + lasered.name + " but not a laser target.");
				}
			}
			else{
				Debug.Log ("Nothing hit.");
			}
		}
		fireButtonStateLastFrame = fireButtonStateThisFrame;
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
