using UnityEngine;
using System.Collections;

//Used in the prototype. So far, the script exists to receive laser fire and
//act on it in some way. For this example, we change the color
public class LaserTarget : MonoBehaviour {

	//<summary>
	//When hit by a laser, the color the object turns
	//</summary>
	public Color shotColor;

	public void onLaserShot(){
		this.gameObject.renderer.material.color = shotColor;
		//TODO play sound
	}
}
