using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//For use in prototype. Not yet for actual game use
public class LaserEmitter : MonoBehaviour {
	
	//The distance after which the laser doesn't hit anything.
	[SerializeField] private float laserClipDistance = 100.0f;
	
	//the object the laser is firing on
	LaserTarget storedLaserTarget;
	[SerializeField] private Transform CenterEyeAnchor;

	//Raycase information
	private RaycastHit hit;
	public Material material;
	public AudioClip laserSound;
	private AudioSource source;
	private LineRenderer lr;
	void Start()
	{
		lr = gameObject.AddComponent<LineRenderer> ();
		lr.SetWidth (LaserUtils.LASER_WIDTH, LaserUtils.LASER_WIDTH);
		lr.enabled = false;
	}
	void Awake() {
		source = GetComponent<AudioSource> ();
		audio.Stop ();
	}

	void Update () {
		/* This is the only way I can get the laserSound to stop firing at the moment. 
		 * I know it sounds funny because the sound is starting over every frame that
		 * the fire button is held.
		 */
		audio.Stop ();
		//If we're firing the laser
		if (Input.GetAxis("FireLaser") >= 0.1f) {
			// start playing the laser sound
			source.PlayOneShot(laserSound, 1f);
			//first, see if we hit anything
			List<Vector3> points = new List<Vector3>();
			points.Add(CenterEyeAnchor.position);
			GameObject justHit = getObjectHit(CenterEyeAnchor.position, (transform.position - CenterEyeAnchor.position).normalized);
			
			//Check to be sure we hit something
			if(justHit != null){
				LaserTarget justHitLaserTarget = justHit.GetComponent<Mirror>();
				points.Add(hit.point);
				//is it a laserTarget? 
				while(justHitLaserTarget != null){
					Vector3 inDir = (points[points.Count - 2]-points[points.Count - 1]).normalized;
					Vector3 outDir = (2.0f * Vector3.Dot(inDir, hit.normal) * hit.normal - inDir).normalized;
					justHit = getObjectHit(points[points.Count-1]+outDir*0.01f, outDir);
					if (justHit == null)
					{
						points.Add (points[points.Count-1] + outDir * 100.0f);
						break;
					}
					points.Add(hit.point);
					justHitLaserTarget = justHit.GetComponent<Mirror>();

				}
				lr.SetVertexCount (points.Count-1);

				//lineRenderer.SetWidth(LASER_WIDTH, LASER_WIDTH);
				for (int i = 1; i < points.Count; ++i)
				{
					lr.SetPosition(i-1, points[i]);
				}
				lr.enabled = true;

			}
			//if we didn't hit anything, call laserleave on the stored object, if it exists
			else{
				lr.enabled = false;
			}
		}
		//if we aren't firing the laser, call laserleave on the stored object, if it exists
		else{
			lr.enabled = false;
		}
	}

	//casts a ray from the center eye anchor to the crosshair and returns the game object hit
	GameObject getObjectHit(Vector3 emitter, Vector3 direction){
		if (Physics.Raycast (emitter, direction, out hit, laserClipDistance)) {
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
}