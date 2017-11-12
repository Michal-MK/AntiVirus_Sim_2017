using System;
using UnityEngine;

public class GameProgression : MonoBehaviour {

	public GameObject[] doors;

	public GameObject block;
	public Spike spike;

	public Vector3 playerPos;
	public Vector3 boxPos;
	public Vector3 spikePos;
	public float ZRotationBlock;


	public static GameProgression script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}

		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		//Empty
	}

	public void GetValues() {
		playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		boxPos = block.transform.position;
		ZRotationBlock = block.transform.rotation.eulerAngles.z;
		spikePos = spike.transform.position;
	}

	public void Progress() {

		if (M_Player.gameProgression == 1) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);

			Canvas_Renderer.script.DisplayDirection(Directions.RIGHT);
		}

		if (M_Player.gameProgression == 2) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);
			doors[2].SetActive(false);
			doors[3].SetActive(false);

			Canvas_Renderer.script.DisplayDirection(Directions.TOP);

		}
		if (M_Player.gameProgression == 3) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);
			doors[2].SetActive(false);
			doors[3].SetActive(false);
			doors[4].SetActive(false);
			doors[5].SetActive(false);

			Canvas_Renderer.script.DisplayDirection(Directions.BOTTOM);
		}
		if (CameraMovement.script != null) {
			CameraMovement.script.RaycastForRooms();
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
