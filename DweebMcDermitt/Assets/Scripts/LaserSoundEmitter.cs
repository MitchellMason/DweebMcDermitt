using UnityEngine;
using System.Collections;

public class LaserSoundEmitter : MonoBehaviour {
	
	[SerializeField] private AudioSource laserSource;

	float timer = LaserUtils.LASER_DURATION;
	bool okayToFire = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (timer > 0) {
			okayToFire = true;
		} else {
			okayToFire = false;
		}

		if(Input.GetAxis("FireLaser") >= 0.1f && okayToFire){
			timer -= Time.deltaTime;
			
			if (timer <= 0) {
				okayToFire = false;
			}

			if(!laserSource.isPlaying){
				laserSource.Play();
			}
		}
		else{
			laserSource.Stop();
		}

		if (Input.GetMouseButtonUp (0)) {
			timer = LaserUtils.LASER_DURATION;
		}
	}
}
