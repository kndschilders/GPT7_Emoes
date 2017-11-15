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

	void Start () {
		cc = GetComponent<CharacterController> ();
	}
	
	void Update () {
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
	}
}
