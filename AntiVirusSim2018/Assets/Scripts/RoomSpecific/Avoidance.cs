using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Avoidance : MonoBehaviour {

	[SerializeField]
	private EnemySpawner spawner = null;

	[SerializeField]
	private Spike spike = null;

	[SerializeField]
	private float avoidDuration = 60;
	/// <summary>
	/// Time in seconds how log should this <see cref="Avoidance"/> last
	/// </summary>
	public float AvoidanceDuration { get => avoidDuration; set => avoidDuration = value; }

	private TurretAttack[] turrets;

	/// <summary>
	/// <see cref="Avoidance"/> completion state, prevents re-activation
	/// </summary>
	public bool AvoidanceFinished { get; private set; }

	#region Lifecycle

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		SignPost.OnAvoidanceBegin += SignPost_OnAvoidanceBegin;
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		SignPost.OnAvoidanceBegin -= SignPost_OnAvoidanceBegin;
	}

	#endregion

	private void SignPost_OnAvoidanceBegin() {
		if (!AvoidanceFinished) {
			HUDisplay.Instance.DisplayInfo("MuHAhAHAHAHAHAHAHAHAHAHAAAAA!\n" +
										 "You fell for my genius trap, now... DIE!", "Survive, You can zoom out using the Mouse Wheel");
			StartAvoidance();
		}
		else {
			HUDisplay.Instance.DisplayInfo(null, "Nope, not doing that thing again!");
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		AvoidanceFinished = data.world.doneAvoidance;
	}

	public void StartAvoidance() {
		MapData.Instance.GetRoomLink(2, 3).CloseDoor();
		turrets = spawner.SpawnAvoidance();
		StartCoroutine(HoldAvoidance());
		Control.Instance.saveManager.canSave = false;
		CameraMovement.Instance.RaycastForRooms();
		StartCoroutine(TimeLeft());
	}

	private IEnumerator HoldAvoidance() {
		yield return new WaitForSeconds(avoidDuration - 5);
		foreach (TurretAttack turret in turrets) {
			turret.Stop();
		}
		yield return new WaitForSeconds(5);
		Control.Instance.saveManager.canSave = true;
		spike.SetPosition();
		HUDisplay.Instance.DisplayInfo("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		AvoidanceFinished = true;
		yield return new WaitForSeconds(10);
		foreach (TurretAttack turret in turrets) {
			turret.CleanUp();
			Destroy(turret.gameObject);
		}
		turrets = null;
		StopAllCoroutines();
	}

	private IEnumerator TimeLeft() {
		int timeLeft = (int)avoidDuration - 1;
		bool show = false;

		while (true) {
			yield return new WaitForSeconds(1);
			timeLeft--;
			if (timeLeft <= 50) {
				show = true;
			}
			if (show) {
				HUDisplay.Instance.UpdateSlideTextDirect(string.Format("{0:00} seconds left!", timeLeft));
			}
			if (timeLeft <= 0) {
				StopCoroutine(TimeLeft());
				break;
			}
		}
	}
}
