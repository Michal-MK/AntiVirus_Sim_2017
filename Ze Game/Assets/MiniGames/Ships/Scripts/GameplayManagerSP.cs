using UnityEngine;
using System.Collections;
using Igor.Minigames.Ships;

public class GameplayManagerSP {

	private GameObject gameOver;
	private bool gameStarted = false;

	public GameplayManagerSP() {
		gameOver = GameObject.Find("Canvas").transform.Find("Game_Over").gameObject;

	}

	public void PrepareForGame() {
		if (!gameStarted) {
			foreach (Location location in Field.self.locations) {
				location.locationVisual.Unhighlight();
			}
			gameStarted = true;
		}
	}

	public void GameOver() {
		gameOver.SetActive(true);
	}
}
