using UnityEngine;
using System.Collections;

public class CameraChoice : MonoBehaviour {

	public bool No_Occulus = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void set()
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			GameObject gobj = transform.GetChild(i).gameObject;
			if (gobj.name == "FirstPersonCam")
			{
				gobj.SetActive(No_Occulus);
			}
			else if (gobj.name == "OVRCam")
			{
				gobj.SetActive(!No_Occulus);
			}
		}
		
	}
}
