using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHidingScript : MonoBehaviour {

	private bool isHiding = false;
	public bool IsHiding {
		get {
			return isHiding;
		}
		private set {
			isHiding = value;
		}
	}

	private Vector3 preHidingPosition;
	private Quaternion preHidingRotation;

	private CharacterController cc;
	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();

		preHidingPosition = transform.position;
	}

	private void setPlayerTransform(Vector3 newPosition, Quaternion newRotation) {
		transform.position = newPosition;
		transform.rotation = newRotation;
	}

	public void EnterHidingSpot(Transform playerTransform) {
		if (IsHiding) return;

		preHidingPosition = transform.position;
		preHidingRotation = transform.localRotation;

		IsHiding = true;
		setPlayerTransform (playerTransform.position, playerTransform.localRotation);
	}

	public void ExitHidingSpot() {
		if (!IsHiding) return;

		IsHiding = false;
		setPlayerTransform (preHidingPosition, preHidingRotation);
	}
}
