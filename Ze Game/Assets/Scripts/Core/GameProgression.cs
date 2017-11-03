using System;
using UnityEngine;

public class GameProgression : MonoBehaviour {

	public GameObject[] doors;
	public Sprite sprtOff;
	public Sprite sprtOn;
	public Canvas_Renderer canvas_Renderer;
	public GameObject Block;

	public Vector3 playerPos;

	public Vector3 boxPos;

	public Vector3 spikePos;
	public float ZRotationBlock;

	private void Awake() {
		Statics.gameProgression = this;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		throw new NotImplementedException();
	}

	public void GetValues() {
		playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		boxPos = Block.transform.position;

		ZRotationBlock = Block.transform.rotation.eulerAngles.z;

		if (GameObject.Find("Spike") != null && GameObject.Find("Spike").activeInHierarchy) {
			spikePos = GameObject.Find("Spike").GetComponent<Transform>().position;
		}
	}

	public void Progress() {

		if (M_Player.gameProgression == 1) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);

			canvas_Renderer.DisplayDirection(Directions.RIGHT);
		}

		if (M_Player.gameProgression == 2) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);
			doors[2].SetActive(false);
			doors[3].SetActive(false);

			canvas_Renderer.DisplayDirection(Directions.TOP);

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

			canvas_Renderer.DisplayDirection(Directions.BOTTOM);
		}
		if (Statics.cameraMovement != null) {
			Statics.cameraMovement.RaycastForRooms();
		}
	}
	private void OnDestroy() {
		Statics.gameProgression = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
