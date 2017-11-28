using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * http://wiki.unity3d.com/index.php/SmoothMouseLook
 * */
[System.Obsolete("This script is obsolete, use MouseLook instead!")]
public class PlayerCameraScript : MonoBehaviour {

	public enum RotationAxes {
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	public float RotationX {
		get {
			return rotationX;
		}
		private set {
			rotationX = value;
		}
	}
	float rotationY = 0F;
	public float RotationY {
		get {
			return rotationY;
		}
		private set {
			rotationY = value;
		}
	}

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	

	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;

	public float frameCounter = 10;

	private Quaternion originalRotation;

	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		if (rb)
			rb.freezeRotation = true;

		originalRotation = transform.localRotation;
	}

	void Update () {
		if (axes == RotationAxes.MouseXAndY)
		{			
			rotAverageY = 0f;
			rotAverageX = 0f;

			RotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			RotationX += Input.GetAxis("Mouse X") * sensitivityX;

			rotArrayY.Add(RotationY);
			rotArrayX.Add(RotationX);

			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt(0);
			}
			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt(0);
			}

			for(int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			for(int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}

			rotAverageY /= rotArrayY.Count;
			rotAverageX /= rotArrayX.Count;

			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);

			transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		}
		else if (axes == RotationAxes.MouseX)
		{			
			rotAverageX = 0f;

			RotationX += Input.GetAxis("Mouse X") * sensitivityX;

			rotArrayX.Add(RotationX);

			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt(0);
			}
			for(int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}
			rotAverageX /= rotArrayX.Count;

			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;			
		}
		else
		{
			rotAverageY = 0f;

			RotationY += Input.GetAxis("Mouse Y") * sensitivityY;

			rotArrayY.Add(RotationY);

			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt(0);
			}
			for(int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			rotAverageY /= rotArrayY.Count;

			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);

			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
			transform.localRotation = originalRotation * yQuaternion;
		}
	}

	public void SetRotation(float rotX, float rotY) {
		RotationX = rotX;
		RotationY = rotY;
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}
