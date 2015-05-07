using UnityEngine;
using System.Collections;

public class LevelAdvancer : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.tag.Equals ("Player")) {
			//one for each eye
			OVRScreenFade[] fade = col.gameObject.GetComponentsInChildren<OVRScreenFade>();
			for(int i=0; i<fade.Length; i++){
				fade[i].BeginFadeOut();
			}
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
