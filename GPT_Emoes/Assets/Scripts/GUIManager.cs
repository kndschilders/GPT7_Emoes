using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public static GUIManager instance = null;

	public Text InteractionText;

	public RectTransform LocationIndicatorRectTransform;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	public void UpdateEnemyLocationIndicator(Vector3 enemyPos, Vector3 playerPos, float playerRotation) {
		Vector2 ePos = new Vector2 (enemyPos.x, enemyPos.z);
		Vector2 pPos = new Vector2 (playerPos.x, playerPos.z);

		Vector2 diff = ePos - pPos;
		float sign = (ePos.y < pPos.y) ? -1.0f : 1.0f;

		float angle = ((Vector2.Angle (Vector2.right, diff) * sign) + playerRotation) % 360.0f;

		LocationIndicatorRectTransform.eulerAngles = new Vector3 (
			LocationIndicatorRectTransform.rotation.x,
			LocationIndicatorRectTransform.rotation.y,
			angle
		);
	}
	
	public void SetInteractionText(string text) {
		InteractionText.text = text;
	}

	public void SetHiding(bool enabled) {
	}

	public void UpdateHeartbeat(int heartbeat) {
	}

	public void SetAlertMode(AlertMode alertMode) {
	}

	public void SetMonsterState(MonsterState monsterState) {
	}
}
