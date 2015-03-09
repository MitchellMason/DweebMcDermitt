using UnityEngine;
using System.Collections;

public class LaserEffectControl : MonoBehaviour {

	[SerializeField] private bool enableFiring = true;
	private ParticleSystem laser;

	float timer = LaserUtils.LASER_DURATION;
	bool okayToFire = false;
	
	void Start(){
		laser = this.gameObject.GetComponent<ParticleSystem>();
	}
	void Update () {

		if (timer > 0) {
			okayToFire = true;
		} else {
			okayToFire = false;
		}

		if(enableFiring && Input.GetAxis("FireLaser") >= 0.1f && okayToFire){
			timer -= Time.deltaTime;
			
			if (timer <= 0) {
				okayToFire = false;
			}

			laser.Play();
		}
		else{
			laser.Stop();
			laser.SetParticles(null, 0);
		}

		if (Input.GetMouseButtonUp (0)) {
			timer = LaserUtils.LASER_DURATION;
		}
	}
}
