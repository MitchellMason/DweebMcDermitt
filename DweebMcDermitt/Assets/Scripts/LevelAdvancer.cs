using UnityEngine;
using System.Collections;

public class LevelAdvancer : MonoBehaviour {

	[SerializeField] int levelNumber;
	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Player")) {
			//one for each eye
			OVRScreenFade[] fade = col.gameObject.GetComponentsInChildren<OVRScreenFade>();
			for(int i=0; i<fade.Length; i++){
				fade[i].BeginFadeOut();
			}
			Debug.Log("Loading Level " + levelNumber);
			Application.LoadLevel(levelNumber);
		}
	}
}
