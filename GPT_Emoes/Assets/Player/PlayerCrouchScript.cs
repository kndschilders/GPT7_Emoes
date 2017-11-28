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

	public enum CrouchState {
		Crawl,
		Crouch,
		None
	}

	public CrouchState State;

	public bool IsCrouching {
		get { return State == CrouchState.Crouch; }
	}

	public bool IsCrawling {
		get { return State == CrouchState.Crawl; }
	}

	private Camera cam;
	private CharacterController cc;

	void Start () {
		cam = GetComponentInChildren<Camera> ();
		cc = GetComponent<CharacterController> ();

		if (!cam || !cc) return;

		normalCamY = cam.transform.localPosition.y;
		normalPlayerHeight = cc.height;
		normalPlayerCenterY = cc.center.y;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.LeftControl))
			OnCrouchChange (CrouchState.Crouch);
		if (Input.GetKeyUp (KeyCode.LeftControl))
			OnCrouchChange (CrouchState.None);

		if (Input.GetKeyDown (KeyCode.C))
			OnCrouchChange (CrouchState.Crawl);
		if (Input.GetKeyUp (KeyCode.C))
			OnCrouchChange (CrouchState.None);

		float camDestY;
		switch (State) {
			case CrouchState.Crawl:
				camDestY = CrawlCamY;
				break;
			case CrouchState.Crouch:
				camDestY = CrouchCamY;
				break;
			case CrouchState.None:
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

	public void OnCrouchChange(CrouchState state) {
		if (state != CrouchState.None && State != CrouchState.None) return;
		State = state;

		float ccHeight, ccCenterY;
		switch (State) {
		case CrouchState.Crawl:
			ccHeight = CrawlPlayerHeight;
			ccCenterY = normalPlayerCenterY - ((normalPlayerHeight - CrawlPlayerHeight) / 2);
			break;
		case CrouchState.Crouch:
			ccHeight = CrouchPlayerHeight;
			ccCenterY = normalPlayerCenterY - ((normalPlayerHeight - CrouchPlayerHeight) / 2);
			break;
		case CrouchState.None:
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
