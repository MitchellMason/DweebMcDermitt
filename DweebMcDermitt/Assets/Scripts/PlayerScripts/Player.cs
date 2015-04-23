using UnityEngine;
using System.Collections;

public class Player : LaserTarget {

	[SerializeField] int HP = 3;
	bool dead = false;

	[SerializeField] PlayMoviePlane glassMov;
	[SerializeField] OVRScreenFade lEye;
	[SerializeField] OVRScreenFade rEye;
	[SerializeField] float fadeTime;

	override public void onLaserShot(LaserHitInfo laserHitInfo){
		HP--;
		Debug.Log ("Player hit. HP is now " + HP);

		glassMov.HP--;
		glassMov.triggered = true;

		if (HP <= 0) {
			dead = true;
			Debug.Log ("Player is dead. Restarting.");
			GetComponent<CharacterController> ().enabled = false;
			
			//Set the amount of time needed to fade in and out
			lEye.fadeTime = fadeTime;
			rEye.fadeTime = fadeTime;
			
			//Fade the screen out
			lEye.BeginFadeOut();
			rEye.BeginFadeOut();
			
			StartCoroutine(RestartLevel());
		}
	}

	override public void onLaserStay(LaserHitInfo laserHitInfo){}
	override public void onLaserLeave(){}
	
	IEnumerator RestartLevel() {
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel (1);
	}
	
	//TODO
	override public bool isTriggered(){
		return false;
	}
}