using UnityEngine;
using System.Collections;

public class Player : LaserTarget {

	int HP = 3;
	bool dead = false;
	GameObject ui;
	UIBox uibox;

	GameObject glass;
	PlayMoviePlane glassMov;

	void Start() {
		ui = GameObject.Find ("rekt");
		uibox = ui.GetComponent<UIBox>();
		glass = GameObject.Find ("GlassesPlane");
		if (glass == null)
			return;
		glassMov = glass.GetComponent<PlayMoviePlane>();
		if (uibox) {
			print ("got it");
		}
	}

	override public void onLaserShot(LaserHitInfo laserHitInfo){
		if (glass != null) {
			HP -= 1;
			print (HP);

			glassMov.HP -=1;
			glassMov.triggered = true;

			if (HP <= 0) {
				dead = true;
				uibox.dead = true;
				print ("dead");
				GetComponent<CharacterController> ().enabled = false;
			}
		}
	}

	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}