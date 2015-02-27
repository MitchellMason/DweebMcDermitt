using UnityEngine;
using System.Collections;

//Used in the prototype. So far, the script exists to receive laser fire and
//act on it in some way. For this example, we change the color
public abstract class LaserTarget : MonoBehaviour {
	public abstract void onLaserShot(Transform laserEmmitter);
	public abstract void onLaserStay(Transform laserEmmitter);
	public abstract void onLaserLeave();
}