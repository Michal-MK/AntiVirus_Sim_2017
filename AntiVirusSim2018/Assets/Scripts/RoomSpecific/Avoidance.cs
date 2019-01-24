using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Avoidance : MonoBehaviour {

	public bool performed = false;
	public EnemySpawner spawner;
	public Spike spike;

	public float avoidDuration = 60;

	private TurretAttack[] turrets;


	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		SignPost.OnAvoidanceBegin += SignPost_OnAvoidanceBegin;
	}

	private void SignPost_OnAvoidanceBegin() {
		if (!performed) {
			Canvas_Renderer.script.DisplayInfo("MuHAhAHAHAHAHAHAHAHAHAHAAAAA!\n" +
												"You fell for my genius trap, now... DIE!", "Survive, You can zoom out using the Mouse Wheel");
			StartAvoidance();
			turrets = spawner.SpawnAvoidance();
		}
		else {
			Canvas_Renderer.script.DisplayInfo(null, "Nope, not doing that thing again!");
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		performed = data.world.doneAvoidance;
	}

	public void StartAvoidance() {
		MapData.script.CloseDoor(MapData.script.GetRoomLink(2, 3));
		StartCoroutine(HoldAvoidance());
		Control.script.saveManager.canSave = false;
		CameraMovement.script.RaycastForRooms();
		StartCoroutine(TimeLeft());
	}

	private IEnumerator HoldAvoidance() {
		yield return new WaitForSeconds(avoidDuration - 5);
		foreach (TurretAttack turret in turrets) {
			turret.Stop();
		}
		yield return new WaitForSeconds(5);
		Control.script.saveManager.canSave = true;
		spike.SetPosition();
		Canvas_Renderer.script.DisplayInfo("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		performed = true;
		yield return new WaitForSeconds(10);
		foreach (TurretAttack turret in turrets) {
			turret.CleanUp();
			Destroy(turret.gameObject);
		}
		turrets = null;
		StopAllCoroutines();
	}

	private IEnumerator TimeLeft() {
		Text SideText = Canvas_Renderer.script.slideInText;
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
}
