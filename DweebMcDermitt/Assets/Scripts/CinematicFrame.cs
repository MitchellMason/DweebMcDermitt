using UnityEngine;
using System.Collections;

public class CinematicFrame : MonoBehaviour {
	[Tooltip("reflects the position of the player so the slides know where to look")]
	[SerializeField] private Transform playerPosition;
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
	
	GameObject background;
	GameObject midground;
	GameObject foreground;

	Vector3 playerScaledPosition;

	// Use this for initialization
	void Start () {
		if (voice != null) {
			voice.Play();
		}
		background = GameObject.Find ("bg");
		midground = GameObject.Find ("mg");
		foreground = GameObject.Find ("fg");

		//Set the textures
		foreground.GetComponent<Renderer>().material.mainTexture = foregroundTexture;
		midground.GetComponent<Renderer>().material.mainTexture = midgroundTexure;
		background.GetComponent<Renderer>().material.mainTexture = backgroundTexure;
	}
	
	// Update is called once per frame
	void Update () {
		playerScaledPosition = playerPosition.position;
		playerScaledPosition.x *= 0.5f;
		this.transform.LookAt (playerScaledPosition);

		//rotate the slides to look at the player
		foreground.transform.LookAt (playerPosition.position);
		 midground.transform.LookAt (playerPosition.position);
		background.transform.LookAt (playerPosition.position);

		foreground.transform.Rotate(0f,180f,0f);
		 midground.transform.Rotate(0f,180f,0f);
		background.transform.Rotate(0f,180f,0f);

		//update the time interval
		secondsUntilNext -= Time.deltaTime;

		//if the frame has been on the screen long enough, destroy iy and replace it
		if(secondsUntilNext < 0f){
			if(nextFramePrefab != null){
				Instantiate(nextFramePrefab, transform.position, transform.rotation);
			}
			else{
				//I'm presuming we'll want to load the next level
				Debug.Log("Out of frames");
			}
			Destroy(this.gameObject);
		}
	}
}
