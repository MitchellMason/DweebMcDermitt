using UnityEngine;
using System.Collections;

public class LaserButton : LaserTarget {
	
	[SerializeField] private TriggerTarget target;
	[SerializeField] private Material changeMaterial;
	
	private bool beingShot;
	private Color originalColor;
	
	void Start(){
		beingShot = false;
		originalColor = changeMaterial.color;
	}
	
	override public void onLaserShot(LaserHitInfo laserHitInfot){
		beingShot = true;
		if(target != null) target.onTrigger(this);
		changeMaterial.color = Color.blue;
		Debug.Log ("LaserButton shot");
	}
	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){
		beingShot = false;
		changeMaterial.color = originalColor;
	}
	
	override public bool isTriggered(){
		return beingShot;
	}
}