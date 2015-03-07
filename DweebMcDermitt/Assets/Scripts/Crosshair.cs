using UnityEngine;
using System.Collections;

//Moves and adjusts the crosshair
public class Crosshair : MonoBehaviour {

	[SerializeField] private Transform FocalPoint;
	[SerializeField] private float DistanceFromPlayer = 2.0f;
	private Vector3 originalScale;

	public AudioClip backgroundMusic;
	private AudioSource source;

	void Start(){
		originalScale = transform.localScale;
		source = GetComponent<AudioSource> ();
		source.PlayOneShot(backgroundMusic, .5f);
	}

	// Simply look at the camera
	void Update () {
		RaycastHit hit;
		float distance;
		if(Physics.Raycast( new Ray(FocalPoint.position,
		                            FocalPoint.rotation * Vector3.forward),
		                    		out hit)){
			distance = hit.distance;
		}
		else{
			distance = DistanceFromPlayer;
		}

		this.gameObject.transform.position = FocalPoint.position + FocalPoint.rotation * Vector3.forward * distance;
		this.gameObject.transform.LookAt (FocalPoint.position);
		this.gameObject.transform.Rotate (0.0f, 180.0f, 0.0f);
		this.gameObject.transform.localScale = originalScale * distance;
	}
}
