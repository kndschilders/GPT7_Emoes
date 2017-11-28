using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void GoToGameWon() {
		SceneManager.LoadScene ("game_won");
	}

	public void GoToGameOver() {
		SceneManager.LoadScene ("game_over");
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene ("main_menu");
	}

	public void GoToLabyrinth() {
		SceneManager.LoadScene ("labyrint_pretty");
	}

	public void GoToJacksPlayground() {
		SceneManager.LoadScene ("JacksPlayground");
	}
}
