using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchScript : MonoBehaviour {
	[Range(-2.0f, 2.0f)]
	public float CrouchCamY;
	[Range(0.1f, 2.0f)]
	public float CrouchPlayerHeight;
	[Range(-2.0f, 2.0f)]
	public float CrawlCamY;
	[Range(0.1f, 2.0f)]
	public float CrawlPlayerHeight;
	[Range(0.01f, 1.0f)]
	public float CamTransformSpeed;

	private float normalCamY;
	private float normalPlayerHeight;
	private float normalPlayerCenterY;

	public PlayerStateVariable PlayerStateEnum;

	private Camera cam;
	private CharacterController cc;
	private bool keepCheckingForCrouchState = false;
	public BoolReference IsHiding;

	void Start () {
		cam = GetComponentInChildren<Camera> ();
		cc = GetComponent<CharacterController> ();

		if (!cam || !cc) return;

		normalCamY = cam.transform.localPosition.y;
		normalPlayerHeight = cc.height;
		normalPlayerCenterY = cc.center.y;
	}

	private bool canStand() {
		Ray ray = new Ray (transform.position, Vector3.up);

		Debug.DrawRay (transform.position, Vector3.up, Color.red);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 1.5f)) {
			if (hit.collider != null) return false;
		}

		return true;
	}

	private void standUp() {
		if (canStand ()) {
			keepCheckingForCrouchState = false;
			OnCrouchChange (PlayerState.Stand);
		} else {
			keepCheckingForCrouchState = true;
		}
	}

	void Update() {
		if (IsHiding.Value) {
			if (PlayerStateEnum.Value != PlayerState.Stand)
				OnCrouchChange (PlayerState.Stand);
		} else {
			if (keepCheckingForCrouchState)
				standUp ();

			if (Input.GetKeyDown (KeyCode.LeftControl))
				OnCrouchChange (PlayerState.Crouch);
			if (Input.GetKeyUp (KeyCode.LeftControl))
				standUp ();

			if (Input.GetKeyDown (KeyCode.C))
				OnCrouchChange (PlayerState.Crawl);
			if (Input.GetKeyUp (KeyCode.C))
				standUp ();
		}

		float camDestY;
		switch (PlayerStateEnum.Value) {
			case PlayerState.Crawl:
				camDestY = CrawlCamY;
				break;
			case PlayerState.Crouch:
				camDestY = CrouchCamY;
				break;
			case PlayerState.Stand:
			default:
				camDestY = normalCamY;
				break;
		}

		if (cam.transform.localPosition.y != camDestY) {
			cam.transform.localPosition = Vector3.Lerp (
				cam.transform.localPosition,
				new Vector3(
					cam.transform.localPosition.x,
					camDestY,
					cam.transform.localPosition.z
				),
				CamTransformSpeed
			);
		}
	}

	public void OnCrouchChange(PlayerState state) {
		if (state != PlayerState.Stand && PlayerStateEnum.Value!= PlayerState.Stand) return;
		PlayerStateEnum.Value = state;

		float ccHeight, ccCenterY;
		switch (PlayerStateEnum.Value) {
		case PlayerState.Crawl:
			ccHeight = CrawlPlayerHeight;
			ccCenterY = normalPlayerCenterY - ((normalPlayerHeight - CrawlPlayerHeight) / 2);
			break;
		case PlayerState.Crouch:
			ccHeight = CrouchPlayerHeight;
			ccCenterY = normalPlayerCenterY - ((normalPlayerHeight - CrouchPlayerHeight) / 2);
			break;
		case PlayerState.Stand:
		default:
			ccHeight = normalPlayerHeight;
			ccCenterY = normalPlayerCenterY;
			break;
		}

		cc.height = ccHeight;
		cc.center = new Vector3(
			cc.center.x,
			ccCenterY,
			cc.center.z
		);
	}
}
