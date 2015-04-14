using UnityEngine;
using System.Collections;

public class Decal : MonoBehaviour {
	[SerializeField] private float lifeTime;

	// Update is called once per frame
	void Update () {
		if (lifeTime < 0) {
			DestroyImmediate (this.gameObject);
		} else {
			lifeTime -= Time.deltaTime;
		}
	}

	public void init(Vector3 where, Vector3 normal){
		this.transform.position = where;
		this.transform.rotation.SetLookRotation (normal);
		this.transform.Rotate (Vector3.up, 90.0f);
	}
}