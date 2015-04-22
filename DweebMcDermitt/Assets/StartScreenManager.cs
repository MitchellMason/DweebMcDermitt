using UnityEngine;
using Ovr;
using System.Collections;

public class StartScreenManager : MonoBehaviour {
	
	[SerializeField] MovieTexture startScreenMovie;
	[SerializeField] string nextLevel;
	[SerializeField] OVRManager ovrManager;
	float HSWonScreenTime = 0.1f;
	
	// Use this for initialization
	void Start () {
		if(startScreenMovie != null){
			this.gameObject.GetComponent<MeshRenderer>().material.mainTexture = startScreenMovie;
		}
		else{
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
			Debug.LogError("start screen movie can't be found on object " + this.gameObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!OVRManager.isHSWDisplayed){
			if(!startScreenMovie.isPlaying){
				startScreenMovie.Play();
			}
			HSWonScreenTime -= Time.deltaTime;
			if(HSWonScreenTime <= 0 && Input.anyKey){
				Application.LoadLevel(nextLevel);
			}
		}
	}
}
