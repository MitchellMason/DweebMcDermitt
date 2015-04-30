using UnityEngine;
using System.Collections;

public class MovieCinematic : MonoBehaviour {
	private Transform playerPosition;
	[Tooltip("The sound that plays on Start() (whenever the object is first loaded into the scene)")]
	[SerializeField] private AudioSource music;
	
	[Tooltip("The texure of the foreground (closest to the player)")]
	[SerializeField] private MovieTexture splash;
	
	GameObject foreground;
	
	/*
	 * play on awake gets set each frame that it isn't already true
	 * this is to allow the OVR health & saftey warning to 
	 * dissappear before the slides begin playing. 
	 */
	[SerializeField] bool autoPlay = true;
	
	Vector3 playerScaledPosition;
	
	void attemptPlaySound(){
		if (music != null && autoPlay) {
			music.Play();
		}
	}
	
	void Start () {
		attemptPlaySound();
		playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		foreground = GameObject.Find("fg");
		//Set the textures
		foreground.GetComponent<Renderer>().material.mainTexture = splash;
	}
	
	// Update is called once per frame
	void Update () {
		playerScaledPosition = playerPosition.position;
		playerScaledPosition.x *= 0.5f;
		this.transform.LookAt (playerScaledPosition);
		
		//rotate the slides to look at the player
		foreground.transform.LookAt (playerPosition.position);
		
		foreground.transform.Rotate (0f, 180f, 0f);
		
		if (splash.isPlaying) {
			splash.Pause();
		}
		else {
			splash.Play();
		}
	}
}
