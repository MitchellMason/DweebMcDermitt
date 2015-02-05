using UnityEngine;
using System.Collections;

public class Mirror : LaserTarget {
	public LaserTarget Monkey;

	override public void onLaserShot(){
		Monkey.onLaserShot ();
	}
}
