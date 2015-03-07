using UnityEngine;
using System.Collections;

public class Door : TriggerTarget {

	MonoBehaviour trigger;
	
	override public void onTrigger(MonoBehaviour t) {
		trigger = t;


	}
}