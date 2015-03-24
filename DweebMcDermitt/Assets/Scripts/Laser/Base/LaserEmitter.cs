using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//The distance after which the laser doesn't hit anything.
	[SerializeField] private float laserClipDistance = 10.0f;
	//The origin for out shots
	[SerializeField] private Transform CenterEyeAnchor;
	//View for debug reasons
	[SerializeField]float timer = LaserUtils.LASER_DURATION;
	//The prefab used to visualize the laserfire
	[SerializeField] GameObject LaserCylinder;
	
	//the object the laser is firing on
	private LaserShooter shooter;
	private LineRenderer lineRenderer;
	private Ray laser;

	//Raycast information
	private RaycastHit hit;
	

	bool okayToFire = false;

	void Awake(){
		lineRenderer = this.gameObject.AddComponent<LineRenderer> ();
		shooter = new LaserShooter (lineRenderer);
	}

	void Update () {
		okayToFire = timer > 0;

		//If we're firing the laser
		if (Input.GetAxis("FireLaser") >= 0.1f && okayToFire) {
			laser.origin = CenterEyeAnchor.position;
			laser.direction = transform.position - CenterEyeAnchor.position;
			shooter.fireLaser(laser, laserClipDistance, false);

			timer -= Time.deltaTime;

			if (timer <= 0) {
				okayToFire = false;
			}
		}
		//if we aren't firing the laser, call laserleave on the stored object, if it exists
		else{
			shooter.endFire();
		}

		if (Input.GetMouseButtonUp (0)) {
			timer = LaserUtils.LASER_DURATION;
		}
	}
}