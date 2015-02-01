using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//<summary>
	//The distance after which the laser doesn't hit anything.
	//</summary>
	[SerializeField] private float laserClipDistance;

	//The direction of fire
	private Vector3 forward;

	private bool fireButtonStateThisFrame = false;
	private bool fireButtonStateLastFrame = false;

	void Update () {
		fireButtonStateThisFrame = Input.GetAxis("FireLaser") <= 0.1f;
		forward = transform.TransformVector (Vector3.forward);
		Debug.DrawRay (transform.position, forward * 10);
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
		if (Physics.Raycast (transform.position, forward, out hit, laserClipDistance)) {
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
}
