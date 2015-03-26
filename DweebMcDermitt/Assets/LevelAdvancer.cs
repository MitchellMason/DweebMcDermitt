using UnityEngine;
using System.Collections;

public class LevelAdvancer : MonoBehaviour {

	[SerializeField] int levelNumber;
	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Player")) {
			Debug.Log("Loading Level " + levelNumber);
			Application.LoadLevel(levelNumber);
		}
	}
}
