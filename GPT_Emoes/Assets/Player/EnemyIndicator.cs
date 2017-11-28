using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class EnemyIndicator : MonoBehaviour {

	/// <summary>
	/// The player.
	/// </summary>
	public GameObject Player;

	/// <summary>
	/// The enemy.
	/// </summary>
	public GameObject Enemy;

	/// <summary>
	/// The max distance to the enemy to show the indicator.
	/// </summary>
	public float MaxDistance = 20.0f;

	/// <summary>
	/// The minimum distance before making the indicator completely opaque.
	/// </summary>
	public float MinDistance = 5.0f;

	private MouseLook pMouseLookScript;
	private RectTransform rectTransform;
	private CanvasRenderer canvasRenderer;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		rectTransform = GetComponent<RectTransform> ();
		canvasRenderer = GetComponent<CanvasRenderer> ();
		pMouseLookScript = Player.GetComponent<MouseLook> ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		updateEnemyLocationIndicator ();
	}

	/// <summary>
	/// Updates the indicator by setting it's rotation and opacity.
	/// </summary>
	private void updateEnemyLocationIndicator() {
		// Create vector2's of player and enemy position.
		Vector2 pPos = new Vector2 (Player.transform.position.x, Player.transform.position.z);
		Vector2 ePos = new Vector2 (Enemy.transform.position.x, Enemy.transform.position.z);

		// Calculate the vector difference.
		Vector2 diff = ePos - pPos;

		// Calculate the distance between the enemy and player position.
		float diffDistance = Mathf.Sqrt((diff.x * diff.x) + (diff.y * diff.y));

		// Calculate the sign, which is needed to flip the angle if needed.
		float sign = (ePos.y < pPos.y) ? -1.0f : 1.0f;

		// Calculate and set indicator angle.
		float angle = ((Vector2.Angle (Vector2.right, diff) * sign) + pMouseLookScript.RotationX + 270.0f) % 360.0f;
		rectTransform.eulerAngles = new Vector3 (
			rectTransform.rotation.x,
			rectTransform.rotation.y,
			angle
		);

		// Calculate and set indicator opacity.
		float newAlpha = Mathf.Clamp01(1.0f - (float)((diffDistance - MinDistance) / MaxDistance));
		canvasRenderer.SetAlpha (newAlpha);
	}
}
