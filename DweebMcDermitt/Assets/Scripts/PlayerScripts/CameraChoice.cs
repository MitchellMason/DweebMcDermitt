using UnityEngine;
using System.Collections;

public class CameraChoice : MonoBehaviour {

	public bool No_Oculus = false;

	public void set()
	{
#if UNITY_EDITOR
		for (int i = 0; i < transform.childCount; ++i)
		{
			GameObject gobj = transform.GetChild(i).gameObject;
			if (gobj.name == "FirstPersonCam")
			{
				gobj.SetActive(No_Oculus);
			}
			else if (gobj.name == "OVRCam")
			{
				gobj.SetActive(!No_Oculus);
			}
		}
#else
		No_Oculus = false;
		for (int i = 0; i < transform.childCount; ++i)
		{
			GameObject gobj = transform.GetChild(i).gameObject;
			if (gobj.name == "FirstPersonCam")
			{
				gobj.SetActive(No_Oculus);
			}
			else if (gobj.name == "OVRCam")
			{
				gobj.SetActive(!No_Oculus);
			}
		}	
#endif
	}
}
