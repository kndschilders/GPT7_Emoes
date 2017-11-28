using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationIndicatorTest : MonoBehaviour {

	public GameObject Player;

	public GameObject Enemy;

	private MouseLook playerMouseLookScript;

	void Start() {
		playerMouseLookScript = Player.GetComponent<MouseLook> ();
	}

	void Update () {
		GUIManager.instance.UpdateEnemyLocationIndicator (Enemy.transform.position, Player.transform.position, playerMouseLookScript.RotationX + 270.0f);
	}
}
