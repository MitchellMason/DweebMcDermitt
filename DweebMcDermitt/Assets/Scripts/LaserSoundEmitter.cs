using UnityEngine;
using System.Collections;

public class LaserSoundEmitter : MonoBehaviour {
	
	[SerializeField] private AudioSource laserSource;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("FireLaser") >= 0.1f){
			if(!laserSource.isPlaying){
				laserSource.Play();
			}
		}
		else{
			laserSource.Stop();
		}
	}
}
