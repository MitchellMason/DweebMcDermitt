using UnityEngine;
using System.Collections;

public class TargetEnabledMultiplexor : MonoBehaviour{

	[SerializeField] private TriggerTarget target;
	[SerializeField] private Target[] conditions;
	
	// Update is called once per frame
	void Update () {
		for(int i=0; i<conditions.Length; i++){
			if(!conditions[i].isTriggered()) return;
		}
		Debug.Log("Firing event for " + target.name);
		target.onTrigger (this);
	}
}
