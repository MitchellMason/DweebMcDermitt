using UnityEngine;
using System.Collections;

public class TurretHorizontalRotate : MonoBehaviour {

	// fast rotation
	public float rotSpeed = 360f;

	public Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {

		
		// distance between target and the actual rotating object
		Vector3 D = target.position - transform.position;  
		
		
		// calculate the Quaternion for the rotation
		Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotSpeed * Time.deltaTime);
		
		//Apply the rotation 
		transform.rotation = rot; 

		// put 0 on the axys you do not want for the rotation object to rotate
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	
	}
}
