using UnityEngine;
using System.Collections;
using Igor.Minigames.Ships;

public class GameplayManagerSP {

	private GameObject gameOver;
	private bool gameStarted = false;

	private Field player;
	private GameObject[] playerVisual;

	private Field ai;
	private GameObject[] aiVisual;

	public GameplayManagerSP(Field player, Field ai) {
		gameOver = GameObject.Find("Canvas").transform.Find("Game_Over").gameObject;
		this.player = player;
		this.ai = ai;
		playerVisual = player.Visualize(ShipsMain.script.locationObj, Ships_UI.ViewingField.PLAYER);
		aiVisual = ai.Visualize(ShipsMain.script.locationObj,Ships_UI.ViewingField.OPPONENT);
	}

	public void PrepareForGame() {
		if (!gameStarted) {
			foreach (Location location in player.locations) {
				location.locationVisual.Unhighlight();
			}
			gameStarted = true;
		}
	}

	public void RecreateField(Vector2 dimensions, GameObject locationObj) {
		GameObject.Destroy(player.getFieldParent.gameObject);
		GameObject.Destroy(ai.getFieldParent.gameObject);

		player = new Field(dimensions);
		playerVisual = player.Visualize(locationObj, Ships_UI.ViewingField.PLAYER);
		ai = new Field(dimensions);
		aiVisual = ai.Visualize(locationObj, Ships_UI.ViewingField.OPPONENT);
		Camera.main.GetComponent<CameraAdjust>().Adjust();
	}

	public void GameOver() {
		gameOver.SetActive(true);
	}


	public Field getPlayerField {
		get { return player; }
	}

	public Field getAiField {
		get { return ai; }
	}

	public GameObject[] fieldObjectsPlayer {
		get { return playerVisual; }
		set { playerVisual = value; }
	}

	public GameObject[] fieldObjectsAi {
		get { return aiVisual; }
		set { aiVisual = value; }
	}
}
