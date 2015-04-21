using UnityEngine;
using System.Collections;

//Moves and adjusts the crosshair
public class Crosshair : MonoBehaviour {

	[SerializeField] private Transform FocalPoint;
	[SerializeField] private float DistanceFromPlayer = 2.0f;
	[SerializeField] private GameObject grabbableIcon;

	private Vector3 originalScale;

	private float InteractionThreshold;

	MeshRenderer grabbablerenderer;

	void Start(){
		originalScale = transform.localScale;
		grabbablerenderer = grabbableIcon.GetComponent<MeshRenderer> ();
		Interactor interactor = this.GetComponent<Interactor> ();
		InteractionThreshold = interactor.getClipDistance ();
	}

	// Simply look at the camera
	void Update () {
		RaycastHit hit;
		float distance;
		if(Physics.Raycast( new Ray(FocalPoint.position,
		                            FocalPoint.rotation * Vector3.forward),
		                    		out hit)){
			distance = hit.distance;
			Debug.Log ("Object crosshair hit is " + hit.collider.gameObject.name);
			if(hit.collider.GetComponent<InteractionTarget> ()!= null && hit.distance < InteractionThreshold){
				grabbablerenderer.enabled = true;
			}
			else{;
				grabbablerenderer.enabled = false;
			}
		}
		else{
			distance = DistanceFromPlayer;
			grabbablerenderer.enabled = false;
		}

		this.gameObject.transform.position = FocalPoint.position + FocalPoint.rotation * Vector3.forward * distance;
		this.gameObject.transform.LookAt (FocalPoint.position);
		this.gameObject.transform.Rotate (0.0f, 180.0f, 0.0f);
		this.gameObject.transform.localScale = originalScale * distance;
	}
}
