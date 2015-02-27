 using UnityEngine;
using System.Collections;

public class Prism : LaserTarget {

	[SerializeField] private LaserTarget t1;
	[SerializeField] private LaserTarget t2;
	public int blah;

	override public void onLaserShot (){
		t1.onLaserShot ();
		t2.onLaserShot ();
	}
	override public void onLaserFocusLost(){}
}
