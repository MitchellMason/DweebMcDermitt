using UnityEngine;
using System.Collections;

public class TargetEnabledMultiplexor : MonoBehaviour{

	[SerializeField] private InteractionTarget target;
	[SerializeField] private Target[] conditions;
	
	// Update is called once per frame
	void Update () {
		for(int i=0; i<conditions.Length; i++){
			if(!conditions[i].isTriggered()) return;
		}
		if(!target.isTriggered()){ 
			target.onInteract();
			Debug.Log("Firing event for " + target.name);
		};
	}
}
