using UnityEngine;
using System.Collections;

public class PedestalTarget : LaserTarget {
	//<summary>
	//When hit by a laser, the color the object turns
	//</summary>
	public Color shotColor;
	public AudioSource wonderful;

	override public void onLaserShot(Transform laserEmmitter){
		Color temp = gameObject.renderer.material.color;
		this.gameObject.renderer.material.color = shotColor;
		shotColor = temp;
		
		wonderful.Play ();
	}

	override public void onLaserStay(Transform laserEmmitter){}
	
	override public void onLaserLeave(){
		Color temp = gameObject.renderer.material.color;
		this.gameObject.renderer.material.color = shotColor;
		shotColor = temp;
	}
}
