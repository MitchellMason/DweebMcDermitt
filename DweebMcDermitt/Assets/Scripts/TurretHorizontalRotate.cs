using UnityEngine;
using System.Collections;

public class TurretHorizontalRotate : TriggerTarget {
	
	// fast rotation
	[SerializeField] float rotSpeed;
	[SerializeField] LaserShooter gun;
	[SerializeField] LineRenderer lineRenderer;
	
	[SerializeField] Transform target;
	[SerializeField] Transform laserStartPoint;
	[SerializeField] Transform laserEndPoint;
	[SerializeField] float range;
	RaycastHit hit;
	bool dead;
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		gun = new LaserShooter (lineRenderer);
		dead = false;
		//laserStartPoint = 
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
			gun.endFire();
			return;
		}
		Physics.Raycast(transform.position, target.position - transform.position, out hit, range);
		if (!hit.collider.gameObject.name.Equals("Player") && !hit.collider.gameObject.name.Equals("PlayerToggle")) {
			gun.endFire();
			return;
		}
		// distance between target and the actual rotating object
		Vector3 D = target.position - transform.position;  

		// calculate the Quaternion for the rotation
		Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);

		//Apply the rotation 
		transform.rotation = rot; 
		
		// put 0 on the axys you do not want for the rotation object to rotate
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		
		gun.fireLaser (new Ray (laserStartPoint.position, laserEndPoint.position - laserStartPoint.position), range);
		
		////gun.fireLaser (new Ray (new Vector3 (transform.position.x + 0f, transform.position.y - 0.1f, transform.position.z + 0f), LaserDirection), 20f);
	}

	public void die() {
		dead = true;
	}

	public override void onTrigger(MonoBehaviour trigger) {
		die ();
	}
}
