using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Avoidance : MonoBehaviour {

	public CameraMovement camMovement;

	public bool performed = false;
	public RectTransform player;
	public GameObject door1;
	public EnemySpawner spawner;
	public Spike spike;
	public float avoidDuration = 60;
	public bool displayAvoidInfo = true;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		SignPost.OnAvoidanceBegin += SignPost_OnAvoidanceBegin;
	}

	private void SignPost_OnAvoidanceBegin() {
		if (displayAvoidInfo) {
			Canvas_Renderer.script.InfoRenderer("MuHAhAHAHAHAHAHAHAHAHAHAAAAA!\n" +
												"You fell for my genious trap, now... DIE!", "Survive, You can zoom out using the Mousewheel");
			displayAvoidInfo = false;
		}
		StartAvoidance();
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		displayAvoidInfo = data.shownHints.shownAvoidanceInfo;
		performed = data.world.doneAvoidance;
	}

	public void StartAvoidance() {
		door1.SetActive(true);
		StartCoroutine(HoldAvoidance());
		SaveManager.canSave = false;
		Projectile.spawnedByAvoidance = true;
		Projectile.spawnedByKillerWall = false;
		camMovement.RaycastForRooms();
		StartCoroutine(TimeLeft());
	}

	private IEnumerator HoldAvoidance() {
		yield return new WaitForSeconds(avoidDuration - 5);
		spawner.DespawnAvoidance();
		yield return new WaitForSeconds(5);
		SaveManager.canSave = true;
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		camMovement.RaycastForRooms();
		Canvas_Renderer.script.InfoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		performed = true;
		StopAllCoroutines();
	}

	private IEnumerator TimeLeft() {
		Text SideText = Canvas_Renderer.script.info_S;
		int i = (int)avoidDuration - 1;
		bool show = false;

		while (true) {
			yield return new WaitForSeconds(1);
			i--;
			if (i <= 50) {
				show = true;
			}
			if (show) {
				SideText.text = string.Format("{0:00} seconds left!", i);
			}
			if (i <= 0) {
				StopCoroutine(TimeLeft());
				break;
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		SignPost.OnAvoidanceBegin -= SignPost_OnAvoidanceBegin;
	}
}
