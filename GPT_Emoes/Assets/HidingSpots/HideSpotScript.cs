using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpotScript : MonoBehaviour {

	public Transform playerLocationTransform;
	public Transform PlayerLocationTransform {
		get {
			return playerLocationTransform;
		}
		private set {
			PlayerLocationTransform = value;
		}
	}

	public float minXAngleOffset = 20.0f;
	public float MinXAngle {
		get {
			return PlayerLocationTransform.eulerAngles.y - minXAngleOffset;
		}
		private set {
			minXAngleOffset = value;
		}
	}

	public float maxXAngleOffset = 20.0f;
	public float MaxXAngle {
		get {
			return PlayerLocationTransform.eulerAngles.y + maxXAngleOffset;
		}
		private set {
			maxXAngleOffset = value;
		}
	}

	public float minYAngleOffset = 20.0f;
	public float MinYAngle {
		get {
			return PlayerLocationTransform.eulerAngles.x - minYAngleOffset;
		}
		private set {
			minYAngleOffset = value;
		}
	}

	public float maxYAngleOffset = 20.0f;
	public float MaxYAngle {
		get {
			return PlayerLocationTransform.eulerAngles.x + maxYAngleOffset;
		}
		private set {
			maxYAngleOffset = value;
		}
	}

	public bool canLoopX = false;
	public bool CanLoopX {
		get {
			return canLoopX;
		}
		private set {
			canLoopX = value;
		}
	}

	public bool canLoopY = false;
	public bool CanLoopY {
		get {
			return canLoopY;
		}
		private set {
			canLoopY = value;
		}
	}
}
