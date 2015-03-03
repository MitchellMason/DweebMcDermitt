using UnityEngine;
using System.Collections;

public class Missile : LaserTarget {
	[SerializeField] float speed;

	void Start(){}
	void Update(){
		transform.Translate (Vector3.forward * speed);
	}

	override public void onLaserShot(Transform laserEmmitter){
		Destroy (this.gameObject);
	}
	override public void onLaserStay(Transform laserEmmitter){
		Destroy (this.gameObject);
	}
	override public void onLaserLeave(){
		Destroy (this.gameObject);
	}
}