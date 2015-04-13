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

	[SerializeField] private GameObject bulletHole;

	//Raycast information
	private RaycastHit hit;


	bool okayToFire = false;
	void startUp()
	{
		if (gameObject.GetComponents<LineRenderer> ().Length <= 0) {
			lineRenderer = gameObject.AddComponent<LineRenderer> ();
			lineRenderer.material = new Material (Shader.Find ("Custom/LaserShad"));
		}
		else
		{
			lineRenderer = GetComponent<LineRenderer> ();
		}
		shooter = new LaserShooter (lineRenderer);
	}
	void Start()
	{
		startUp ();
	}
	void Awake()
	{
		startUp ();
	}
	void Update () {
		okayToFire = timer > 0;
		//If we're firing the laser
		if (Input.GetAxis("FireLaser") >= 0.1f && okayToFire) {
			laser.origin = CenterEyeAnchor.position;
			laser.direction = transform.position - CenterEyeAnchor.position;
			shooter.fireLaser(laser, laserClipDistance, false);
			Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

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