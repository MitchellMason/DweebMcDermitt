using UnityEngine;
using System.Collections;

//Moves the camera so that the mirror effect is accurate
public class MirrorCameraTargeting : MonoBehaviour {

	[SerializeField] private Transform Player;
	
	// Update is called once per frame
	void Update () {
		transform.rotation.SetLookRotation (Player.transform.position);
	}
}
