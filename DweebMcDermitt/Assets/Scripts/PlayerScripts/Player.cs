﻿using UnityEngine;
using System.Collections;

public class Player : LaserTarget {

	float HP = 3;
	bool dead = false;
	GameObject ui;
	UIBox uibox;

	void Start() {
		ui = GameObject.Find ("GUI");
		uibox = ui.GetComponent<UIBox>();
		if (uibox) {
			print ("got it");
		}
	}

	override public void onLaserShot(LaserHitInfo laserHitInfo){
		HP -= 1;
		print (HP);
		if (HP <= 0) {
			dead = true;
			uibox.dead = true;
			print ("dead");
			GetComponent<CharacterController>().enabled = false;
		}
	}

	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}