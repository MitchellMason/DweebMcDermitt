using UnityEngine;
using System.Collections;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//The distance after which the laser doesn't hit anything.
	[SerializeField] private float laserClipDistance = 10.0f;
	//The origin for out shots
	[SerializeField] private Transform CenterEyeAnchor;

	//the object the laser is firing on
	private LaserShooter shooter;
	private LineRenderer lineRenderer;
	private Ray laser;

	//Raycast information
	private RaycastHit hit;

	void Start(){
		lineRenderer = this.gameObject.AddComponent<LineRenderer> ();
		laser = new Ray ();
		shooter = new LaserShooter (lineRenderer);
		lineRenderer.enabled = false;
	}

	void Update () {
		//If we're firing the laser
		if (Input.GetAxis("FireLaser") >= 0.1f) {
			laser.origin = CenterEyeAnchor.position;
			laser.direction = transform.position - CenterEyeAnchor.position;
			shooter.fireLaser(laser, laserClipDistance, false);
		}
		//if we aren't firing the laser, call laserleave on the stored object, if it exists
		else{
			shooter.endFire();
		}
	}
}