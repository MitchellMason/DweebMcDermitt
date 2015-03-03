using UnityEngine;
using System.Collections;

public class LaserEffectControl : MonoBehaviour {

	[SerializeField] private bool enableFiring = true;
	private ParticleSystem laser;
	
	void Start(){
		laser = this.gameObject.particleSystem;
	}
	void Update () {
		if(enableFiring && Input.GetAxis("FireLaser") >= 0.1f){
			laser.Play();
		}
		else{
			laser.Stop();
			laser.SetParticles(null, 0);
		}
	}
}
