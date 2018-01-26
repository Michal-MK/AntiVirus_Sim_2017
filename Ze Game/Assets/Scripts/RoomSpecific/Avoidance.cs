using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Avoidance : MonoBehaviour {

	public bool performed = false;
	public EnemySpawner spawner;
	public Spike spike;

	public float avoidDuration = 60;

	private bool displayAvoidInfo = true;

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
		MapData.script.CloseDoor(new RoomLink(2, 3));
		StartCoroutine(HoldAvoidance());
		SaveManager.canSave = false;
		CameraMovement.script.RaycastForRooms();
		StartCoroutine(TimeLeft());
	}

	private IEnumerator HoldAvoidance() {
		yield return new WaitForSeconds(avoidDuration - 5);
		spawner.DespawnAvoidance();
		yield return new WaitForSeconds(5);
		SaveManager.canSave = true;
		MapData.script.OpenDoor(new RoomLink(2, 3));
		spike.SetPosition();
		CameraMovement.script.RaycastForRooms();
		Canvas_Renderer.script.InfoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		performed = true;
		StopAllCoroutines();
	}

	private IEnumerator TimeLeft() {
		Text SideText = Canvas_Renderer.script.slideInInfo;
		int timeLeft = (int)avoidDuration - 1;
		bool show = false;

		while (true) {
			yield return new WaitForSeconds(1);
			timeLeft--;
			if (timeLeft <= 50) {
				show = true;
			}
			if (show) {
				SideText.text = string.Format("{0:00} seconds left!", timeLeft);
			}
			if (timeLeft <= 0) {
				StopCoroutine(TimeLeft());
				break;
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		SignPost.OnAvoidanceBegin -= SignPost_OnAvoidanceBegin;
	}

	public bool save_displayAvoidInfo {
		get { return displayAvoidInfo; }
	}
}
