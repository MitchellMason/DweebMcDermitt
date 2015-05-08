using UnityEngine;
using System.Collections;

public class CinematicFrame : MonoBehaviour {
	private Transform playerPosition;
	[Tooltip("The sound that plays on Start() (whenever the object is first loaded into the scene)")]
	[SerializeField] private AudioSource voice;
	[Tooltip("After this time interval, the next slide is displayed or the next scene is loaded")]
	[SerializeField] private float secondsUntilNext;

	[Tooltip("The texure of the foreground (closest to the player)")]
	[SerializeField] private Texture2D foregroundTexture;
	[Tooltip("The texure of the midground")]
	[SerializeField] private Texture2D midgroundTexure;
	[Tooltip("The texure of the background (furthest to the player)")]
	[SerializeField] private Texture2D backgroundTexure;

	[Tooltip("A link to the next prefab to load when time runs out")]
	[SerializeField] private CinematicFrame nextFramePrefab;
	
	[SerializeField] GameObject foreground;
	[SerializeField] GameObject midground;
	[SerializeField] GameObject background;
	[SerializeField] MovieTexture movie;

	/*
	 * play on awake gets set each frame that it isn't already true
	 * this is to allow the OVR health & saftey warning to 
	 * dissappear before the slides begin playing. 
	 */
	 
	[SerializeField] bool autoPlay = true;
	[SerializeField] public bool isMovie = false;

	Vector3 playerScaledPosition;
	
	bool played = false;

	void attemptPlaySound(){
		if (voice != null && autoPlay) {
			voice.Play();
		}
	}

	void Start () {
		attemptPlaySound ();
		playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		//foreground = GameObject.Find("fg");
		if (isMovie) {
			foreground.GetComponent<Renderer>().material.mainTexture = movie;
		} else {
			foreground.GetComponent<Renderer>().material.mainTexture = foregroundTexture;
		}
		//Set the textures
		midground.GetComponent<Renderer>().material.mainTexture  = midgroundTexure;
		background.GetComponent<Renderer>().material.mainTexture = backgroundTexure;
	}
	
	// Update is called once per frame
	void Update () {
	if (isMovie) {
		if (!movie.isPlaying && !played) {
			movie.Play ();
			played = true;
		}
		if (!movie.isPlaying && played) {
			Instantiate (nextFramePrefab, transform.position, transform.rotation);
			Destroy (this.gameObject);
			return;
		}
	} else {
		playerScaledPosition = playerPosition.position;
		playerScaledPosition.x *= 0.5f;
		this.transform.LookAt (playerScaledPosition);
		
		//rotate the slides to look at the player
		foreground.transform.LookAt (playerPosition.position);
		midground.transform.LookAt (playerPosition.position);
		background.transform.LookAt (playerPosition.position);
		
		foreground.transform.Rotate (0f, 180f, 0f);
		midground.transform.Rotate (0f, 180f, 0f);
		background.transform.Rotate (0f, 180f, 0f);

		if (autoPlay) {
			//update the time interval
			secondsUntilNext -= Time.deltaTime;

			//if the frame has been on the screen long enough, destroy iy and replace it
			if (secondsUntilNext < 0f) {
				if (nextFramePrefab != null) {
					Instantiate (nextFramePrefab, transform.position, transform.rotation);
					Destroy (this.gameObject);
					return;
				} else {
					//I'm presuming we'll want to load the next level
					Debug.Log ("Out of frames");
					Application.LoadLevel (Application.loadedLevel + 1); //Level 1
				}
				Destroy (this.gameObject);
			}
		} else {
			autoPlay = Input.GetAxis("FireLaser") >= 0.1f;
			if(autoPlay){	
				attemptPlaySound();
			}
		}
	}
}
}