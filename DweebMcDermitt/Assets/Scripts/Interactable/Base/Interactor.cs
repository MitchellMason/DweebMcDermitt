using UnityEngine;
using System.Collections;

public class Interactor : MonoBehaviour {

	[SerializeField] float interactionClipDistance;
	[SerializeField] Transform CenterEyeAnchor;

	private bool interactThisFrame = false;
	private bool interactLastFrame = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		interactThisFrame = Input.GetAxis ("Interact") >= 0.1f;
		//interact when the button is released
		if (!interactThisFrame && interactLastFrame) {
			Debug.Log ("Looking for interaction targets");
			RaycastHit hit;
			if (Physics.Raycast (CenterEyeAnchor.position, transform.position - CenterEyeAnchor.position, out hit, interactionClipDistance)) {
				Debug.Log ("hit something " + hit.collider.name);
				InteractionTarget justHit = hit.collider.GetComponent<InteractionTarget>();
				if(justHit != null){
					Debug.Log ("Triggering");
					justHit.onInteract();
				}
			}
			else{
				Debug.Log ("Nothing close enough");
			}
		}
		interactLastFrame = interactThisFrame;
	}

	public float getClipDistance(){
				return interactionClipDistance;
		}
}
