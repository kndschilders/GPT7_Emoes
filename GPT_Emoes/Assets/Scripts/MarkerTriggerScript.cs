using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerTriggerScript : MonoBehaviour {

	public GameEvent ge;

	public void OnSetMarker() {
		ge.Raise ();
	}
}
