using UnityEngine;
using System.Collections;

public class AxialRotation : InteractionTarget {
	[SerializeField] private Transform rotateAround;
	[SerializeField] bool rotationEnabled;
	
	public override void onInteract ()
	{
		if(rotationEnabled)
			this.transform.RotateAround(rotateAround.position, Vector3.up, 90.0f);
	}
	
	public override bool isTriggered ()
	{
		return false;
	}
}
