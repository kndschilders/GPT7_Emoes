using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void OnGoToGameOver() {
		SceneManager.LoadScene ("game_over");
	}

	public void OnGoToMainMenu() {
		SceneManager.LoadScene ("main_menu");
	}

	public void OnGoToLabyrinth() {
		SceneManager.LoadScene ("labyrint_pretty");
	}

	public void OnGoToJacksPlayground() {
		SceneManager.LoadScene ("JacksPlayground");
	}
}
