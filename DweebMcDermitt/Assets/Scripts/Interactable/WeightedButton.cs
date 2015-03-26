using UnityEngine;
using System.Collections;

public class WeightedButton : PositionTarget {
	[SerializeField] private AudioSource buttonAudio;
	[SerializeField] private Color engadgedColor;
	[SerializeField] GameObject topPiece;
	[SerializeField] TriggerTarget optionalTarget;
	private bool pressed = false;
	private float weightThreshold = 10.0f;
	private Color swap;

	void Start(){
		swap = topPiece.GetComponent<MeshRenderer> ().material.color;
	}

	void OnTriggerEnter(Collider col){
		if ((col.GetComponent<Rigidbody> () != null && col.GetComponent<Rigidbody> ().mass > weightThreshold) || col.gameObject.tag.Equals ("Player")) {
			pressed = true;
			buttonAudio.Play ();
			if (optionalTarget != null) 
				optionalTarget.onTrigger (this);
			topPiece.GetComponent<MeshRenderer> ().material.color = engadgedColor;
			return;
		}
	}
	
	void OnTriggerExit(Collider col){
		topPiece.GetComponent<MeshRenderer>().material.color = swap;
		pressed = false;
	}
	
	override public bool isTriggered(){
		//Debug.Log (this.gameObject.name + " is " + pressed);
		return pressed;
	}
	
	override public void onPosition() {
		// Get off of me fatty I'm trying to live my life.
	}
}
