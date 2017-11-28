using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLastKnownLocationScript : MonoBehaviour {

	public GameObject playerRef;

	[Range(0.0f, 30.0f)]
	public float DespawnDuration = 10.0f;

	public void OnSetPlayerMarker() {
		StopAllCoroutines ();
		StartCoroutine (setPlayerMarkerPosition ());
	}

	private void setMarkerEnabled(bool enabled, Vector3 pos) {
		transform.position = pos;
	}

	private IEnumerator setPlayerMarkerPosition() {
		setMarkerEnabled (true, playerRef.transform.position);

		yield return new WaitForSeconds (DespawnDuration);

		setMarkerEnabled (false, new Vector3(0.0f, -1000000.0f, 0.0f));
	}
}
