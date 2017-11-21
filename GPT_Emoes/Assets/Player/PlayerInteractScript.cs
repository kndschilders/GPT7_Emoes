using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHidingScript))]

public class PlayerInteractScript : MonoBehaviour {

	public float InteractDistance = 3.0f;
	public Transform CameraTransform;

	private bool isLookingAtHidingSpot = false;
	private PlayerHidingScript hidingScript;

	//private MouseLook mouseLookScript;
	public MouseLook[] mouseLookScripts;

	void Start() {
		hidingScript = GetComponent<PlayerHidingScript> ();
		//mouseLookScript = GetComponent<MouseLook> ();
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

		if (GUIManager.instance) {
			if (isLookingAtHidingSpot)
				GUIManager.instance.SetInteractionText ("Press E to Hide");
			else
				GUIManager.instance.SetInteractionText ("");
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			if (isLookingAtHidingSpot) {
				GameObject hidingSpot = hit.transform.gameObject;

				if (!hidingSpot)
					return;

				HideSpotScript hideSpotScript = hidingSpot.GetComponent<HideSpotScript> ();

				if (!hideSpotScript)
					return;

				Transform playerTransform = hideSpotScript.PlayerLocationTransform;

				hidingScript.EnterHidingSpot (playerTransform);

				foreach (MouseLook mouseLookScript in mouseLookScripts) {
					mouseLookScript.minimumX = hideSpotScript.MinXAngle;
					mouseLookScript.maximumX = hideSpotScript.MaxXAngle;
					mouseLookScript.minimumY = hideSpotScript.MinYAngle;
					mouseLookScript.maximumY = hideSpotScript.MaxYAngle;
					mouseLookScript.CanLoopX = hideSpotScript.CanLookX;
					mouseLookScript.CanLoopY = hideSpotScript.CanLookY;
				}

				//mouseLookScript.minimumX = hideSpotScript.MinXAngle;
				//mouseLookScript.maximumX = hideSpotScript.MaxXAngle;
				//mouseLookScript.canLoopX = false;


				//mouseLookScript.minimumX = hideSpotScript.GetMinX ();
				//mouseLookScript.maximumX = hideSpotScript.GetMaxX ();
			} else {
				hidingScript.ExitHidingSpot ();

				foreach (MouseLook mouseLookScript in mouseLookScripts) {
					mouseLookScript.Reset ();
				}

				//mouseLookScript.minimumX = -360.0f;
				//mouseLookScript.maximumX = 360.0f;
				//mouseLookScript.canLoopX = true;
			}
		}
	}
}
