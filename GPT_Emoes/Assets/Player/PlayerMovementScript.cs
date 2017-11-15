using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * https://stackoverflow.com/questions/39671974/unity3d-player-movement-script
 * */
public class PlayerMovementScript : MonoBehaviour {

	public float Speed = 6.0f;
	public float JumpSpeed = 8.0f;
	public float Gravity = 20.0f;

	private Vector3 moveDirection = Vector3.forward;

	private CharacterController cc;

	private bool isHiding = false;

	private Vector3 preHidingPosition;
	private Quaternion preHidingRotation;

	void Start () {
		cc = GetComponent<CharacterController> ();

		preHidingPosition = transform.position;
	}
	
	void Update () {
		if (!isHiding) {
			if (cc.isGrounded) {
				moveDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0.0f, Input.GetAxisRaw ("Vertical"));
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= Speed;

				if (Input.GetButton ("Jump"))
					moveDirection.y = JumpSpeed;
				else
					moveDirection.y = 0.0f;
			}

			moveDirection.y -= Gravity * Time.deltaTime;

			cc.Move (moveDirection * Time.deltaTime);
		} else {
			// Player is hiding, do nothing currently...
		}
	}

	private void setPlayerTransform(Vector3 newPosition, Quaternion newRotation) {
		transform.position = newPosition;
		transform.rotation = newRotation;
	}

	public void EnterHidingSpot(Transform playerTransform) {
		if (isHiding) return;

		preHidingPosition = transform.position;
		preHidingRotation = transform.localRotation;

		isHiding = true;
		setPlayerTransform (playerTransform.position, playerTransform.localRotation);
	}

	public void ExitHidingSpot() {
		if (!isHiding) return;

		isHiding = false;
		setPlayerTransform (preHidingPosition, preHidingRotation);
	}
}
