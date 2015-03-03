using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	[SerializeField] private GameObject CenterEyeAnchor;
	[SerializeField] private float DistanceFromPlayer = 2.0f;
	private Vector3 originalScale;

	void Start(){
		originalScale = transform.localScale;
	}

	// Simply look at the camera
	void Update () {
		RaycastHit hit;
		float distance;
		if(Physics.Raycast( new Ray(CenterEyeAnchor.transform.position,
		                            CenterEyeAnchor.transform.rotation * Vector3.forward),
		                    		out hit)){
			distance = hit.distance;
		}
		else{
			distance = DistanceFromPlayer;
		}

		this.gameObject.transform.position = CenterEyeAnchor.transform.position + CenterEyeAnchor.transform.rotation * Vector3.forward * distance;
		this.gameObject.transform.LookAt (CenterEyeAnchor.transform.position);
		this.gameObject.transform.Rotate (0.0f, 180.0f, 0.0f);
		this.gameObject.transform.localScale = originalScale * distance;
	}
}
