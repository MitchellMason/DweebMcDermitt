using UnityEngine;
using System.Collections;

public class MagnifyingGlass : LaserTarget {
	public LaserTarget Target;
	
	override public void onLaserShot() {
		Target.onLaserShot ();
	}
	override public void onLaserFocusLost(){}
}