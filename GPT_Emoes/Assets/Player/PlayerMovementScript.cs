using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * https://stackoverflow.com/questions/39671974/unity3d-player-movement-script
 * */
[System.Obsolete("This script is obsolete, use FPSInputController instead!")]
public class PlayerMovementScript : MonoBehaviour {

	public float Speed = 6.0f;
	public float SprintSpeed = 10.0f;
	public float CrouchSpeed = 3.0f;
	public float JumpSpeed = 8.0f;
	public float Gravity = 20.0f;

	public enum PlayerMovementState {
		Moving,
		Sprinting,
		Crouching
	}

	public PlayerMovementState MovementState = PlayerMovementState.Moving;

	public enum FogDistance {
		Close,
		Normal,
		Far
	}

	public Kino.Fog Fog;

	public float FogNormalDistance = 4.0f;
	public float FogCloseDistance = 1.0f;
	public float FogFarDistance = 8.0f;

	private Vector3 moveDirection = Vector3.forward;

	private CharacterController cc;

	private bool isHiding = false;

	private Vector3 preHidingPosition;
	private Quaternion preHidingRotation;

	private FogDistance fogDistance = FogDistance.Normal;

	void Start () {
		cc = GetComponent<CharacterController> ();

		preHidingPosition = transform.position;
	}
	
	void Update () {
		if (!isHiding) {
			if (cc.isGrounded) {
				if (Input.GetKey (KeyCode.LeftShift))
					MovementState = PlayerMovementState.Sprinting;
				else if (Input.GetKey (KeyCode.LeftControl))
					MovementState = PlayerMovementState.Crouching;
				else
					MovementState = PlayerMovementState.Moving;


				float speed;
				switch (MovementState) {
					case PlayerMovementState.Crouching:
						speed = CrouchSpeed;	
						break;
					case PlayerMovementState.Sprinting:
						speed = SprintSpeed;	
						break;
					case PlayerMovementState.Moving:
					default:
						speed = Speed;
						break;
				}
				
				moveDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0.0f, Input.GetAxisRaw ("Vertical"));
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= speed;

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

		if (Fog) {
			switch (fogDistance) {
				case FogDistance.Close:
					Fog.startDistance = FogCloseDistance;
					break;
				case FogDistance.Far:
					Fog.startDistance = FogFarDistance;
					break;
				case FogDistance.Normal:
				default:
					Fog.startDistance = FogNormalDistance;
					break;
			}
		}
	}

	private void setPlayerTransform(Vector3 newPosition, Quaternion newRotation) {
		transform.position = newPosition;
		transform.rotation = newRotation;
	}

	public void EnterHidingSpot(Transform playerTransform) {
		if (isHiding) return;

		fogDistance = FogDistance.Far;

		preHidingPosition = transform.position;
		preHidingRotation = transform.localRotation;

		isHiding = true;
		setPlayerTransform (playerTransform.position, playerTransform.localRotation);
	}

	public void ExitHidingSpot() {
		if (!isHiding) return;

		fogDistance = FogDistance.Close;

		isHiding = false;
		setPlayerTransform (preHidingPosition, preHidingRotation);
	}
}
