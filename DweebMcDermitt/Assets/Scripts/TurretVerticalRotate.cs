using UnityEngine;
using System.Collections;

public class TurretVerticalRotate : MonoBehaviour {
	
	// fast rotation
	[SerializeField] float rotSpeed;
	[SerializeField] LaserShooter gun;
	[SerializeField] LineRenderer lineRenderer;
	
	[SerializeField] Transform target;
	[SerializeField] Transform laserStartPoint;
	[SerializeField] Transform laserEndPoint;
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		gun = new LaserShooter (lineRenderer);
		//laserStartPoint = 
	}
	
	// Update is called once per frame
	void Update () {
		float yRotation = transform.eulerAngles.y;
		
		// distance between target and the actual rotating object
		Vector3 D = target.position - transform.position;  
		
		
		
		// calculate the Quaternion for the rotation
		Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);
		
		
		
		//Apply the rotation 
		transform.rotation = rot; 
		
		// put 0 on the axys you do not want for the rotation object to rotate
		transform.eulerAngles = new Vector3(-13, yRotation, 0);
		
		Vector3 LaserDirection = transform.eulerAngles;
		LaserDirection.y += 1;
		
		//gun.fireLaser (new Ray (laserStartPoint.position, laserEndPoint.position - laserStartPoint.position), 50f);
		
		////gun.fireLaser (new Ray (new Vector3 (transform.position.x + 0f, transform.position.y - 0.1f, transform.position.z + 0f), LaserDirection), 20f);
	}
}
