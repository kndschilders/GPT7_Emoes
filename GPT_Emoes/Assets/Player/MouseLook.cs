using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public bool CanLoopX = true;
	public bool CanLoopY = false;
	public float minimumY = -60F;
	public float maximumY = 60F;
	public float RotationY = 0F;
	public float RotationX = 0F;

	private float defaultMinX;
	private float defaultMaxX;
	private float defaultMinY;
	private float defaultMaxY;
	private bool defaultCanLoopX;
	private bool defaultCanLoopY;

	void Update () {
        if (CursorLock.instance.CursorIsLocked == false)
            return;

		if (axes == RotationAxes.MouseXAndY) {
			RotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			RotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			RotationY = Mathf.Clamp (RotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-RotationY, RotationX, 0);
		} else if (axes == RotationAxes.MouseX) {
			RotationX += Input.GetAxis("Mouse X") * sensitivityX;

			if (!CanLoopX) RotationX = Mathf.Clamp (RotationX, minimumX, maximumX);

			transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, RotationX, 0);
		} else {
			RotationY += Input.GetAxis("Mouse Y") * sensitivityY;

			if (!CanLoopY) RotationY = Mathf.Clamp (RotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-RotationY, transform.localEulerAngles.y, 0);
		}
	}

	void Start () {
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;

		defaultMinX = minimumX;
		defaultMaxX = maximumX;
		defaultMinY = minimumY;
		defaultMaxY = maximumY;
		defaultCanLoopX = CanLoopX;
		defaultCanLoopY = CanLoopY;
	}

	public void SetCamRotation(float rotX, float rotY) {
		RotationX = rotX;
		RotationY = rotY;
	}

	public void Reset() {
		minimumX = defaultMinX;
		maximumX = defaultMaxX;
		minimumY = defaultMinY;
		maximumY = defaultMaxY;
		CanLoopX = defaultCanLoopX;
		CanLoopY = defaultCanLoopY;
	}
}
