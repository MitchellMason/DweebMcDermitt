using UnityEngine;
using System.Collections;

//Used in the prototype. So far, the script exists to receive laser fire and
//act on it in some way. For this example, we change the color
public abstract class InteractionTarget : Target {
	public abstract void onInteract();
}