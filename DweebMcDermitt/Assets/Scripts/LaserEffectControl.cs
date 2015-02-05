using UnityEngine;
using System.Collections;

public class LaserEffectControl : MonoBehaviour {
	
	public ParticleSystem laser;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space)){
			laser.Play();
		}
		else{
			laser.Stop();
			laser.SetParticles(null, 0);
		}
	}
}
