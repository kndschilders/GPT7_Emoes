using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public static GUIManager instance = null;

	public Text InteractionText;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
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
