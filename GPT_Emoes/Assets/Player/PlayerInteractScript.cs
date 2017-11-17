using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour {

	public float InteractDistance = 3.0f;
	public Transform CameraTransform;

	private bool isLookingAtHidingSpot = false;
	private PlayerMovementScript movementScript;

	void Start() {
		movementScript = GetComponent<PlayerMovementScript> ();
	}

	void Update () {
		Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);

		Debug.DrawRay (CameraTransform.position, CameraTransform.forward * InteractDistance, Color.red);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, InteractDistance)) {
			if (hit.transform.CompareTag ("HideSpot"))
				isLookingAtHidingSpot = true;
			else
				isLookingAtHidingSpot = false;
		} else {
			isLookingAtHidingSpot = false;
		}

		if (isLookingAtHidingSpot)
			GUIManager.instance.SetInteractionText ("Press E to Hide");
		else
			GUIManager.instance.SetInteractionText ("");

		if (Input.GetKeyDown (KeyCode.E)) {
			if (isLookingAtHidingSpot) {
				GameObject hidingSpot = hit.transform.gameObject;

				if (!hidingSpot)
					return;

				HideSpotScript hideSpotScript = hidingSpot.GetComponent<HideSpotScript> ();

				if (!hideSpotScript)
					return;

				Transform playerTransform = hideSpotScript.GetPlayerLocationTransform ();

				movementScript.EnterHidingSpot (playerTransform);
			} else {
				movementScript.ExitHidingSpot ();
			}
		}
	}
}
