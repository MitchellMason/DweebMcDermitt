using UnityEngine;
using System.Collections;

public class TargetEnabledMultiplexor : MonoBehaviour{

	[SerializeField] private TriggerTarget target;
	[SerializeField] private Target[] conditions;
	[SerializeField] private AudioSource successSound;
	[SerializeField] private bool needSuccessSound;

	private bool soundHasPlayed = false;

	
	// Update is called once per frame
	void Update () {
		for(int i=0; i<conditions.Length; i++){
			if(!conditions[i].isTriggered()) return;
		}
		Debug.Log("Firing event for " + target.name);
		if (needSuccessSound) {
			if (!successSound.isPlaying && !soundHasPlayed) {
				successSound.Play ();
			}
			soundHasPlayed = true;
		}
		target.onTrigger (this);
	}
}
