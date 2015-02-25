using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	[SerializeField] GameObject LookingAt;
	
	void Update () {
		transform.LookAt (LookingAt.transform.position);
	}
}
