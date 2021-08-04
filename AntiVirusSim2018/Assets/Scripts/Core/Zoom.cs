using Igor.Constants.Strings;
using System;
using UnityEngine;

public class Zoom : MonoBehaviour {

	[SerializeField]
	private Camera cam = null;
	[SerializeField]
	private ParticleSystem matrixA = null;
	[SerializeField]
	private ParticleSystem matrixB = null;
	[SerializeField]
	private float bossMax = 107f;
	[SerializeField]
	private float bossMin = 10f;
	[SerializeField]
	private float normMax = 25;
	[SerializeField]
	private float normMin = 15;

	/// <summary>
	/// The standard zoom value
	/// </summary>
	public float NormalZoom => normMax;

	public static bool CanZoom { get; set; } = true;

	#region Lifecycle

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}
	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	#endregion

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		CanZoom = data.player.canZoom;
	}

	private void LateUpdate() {
		if (!CanZoom) return;

		float ortho = cam.orthographicSize;
		float scroll = Input.GetAxis(InputNames.MOUSEWHEEL);

		if (scroll == 0) return;

		if (CameraMovement.Instance.IsInBossRoom) {
			if (ortho < bossMax && ortho >= bossMin) {
				ortho += scroll * 0.2f;
				cam.orthographicSize = Mathf.Clamp(ortho, bossMin, bossMax);
			}
		}
		else if (ortho <= normMax && ortho >= normMin) {
			ortho += scroll * 0.08f;
			ZoomTo(ortho);
		}
	}

	public void ZoomTo(float zoom) {
		cam.orthographicSize = Mathf.Clamp(zoom, normMin, normMax);

		matrixA.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + cam.orthographicSize + 1, 0);
		matrixB.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - cam.orthographicSize - 1, 0);

		ParticleSystem.ShapeModule shapeA = matrixA.shape;
		ParticleSystem.ShapeModule shapeB = matrixB.shape;

		shapeA.radius = cam.orthographicSize * 2 + 10;
		shapeB.radius = cam.orthographicSize * 2 + 10;
	}
}
