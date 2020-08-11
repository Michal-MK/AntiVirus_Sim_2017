using UnityEngine;

public class Zoom : MonoBehaviour {

	public Camera cam;
	public ParticleSystem matrixA;
	public ParticleSystem matrixB;
	public float BossMax = 107f;
	public float BossMin = 15f;

	public float NormMax = 25;
	public float NormMin = 15;

	public static bool CanZoom { get; set; } = true;


	private float intermediateValue = 0;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		CanZoom = data.player.canZoom;
	}

	private void LateUpdate() {
		if (CameraMovement.Instance.IsInBossRoom && CanZoom) {
			float roll = Input.GetAxis("Mouse Scroll Wheel");
			intermediateValue = cam.orthographicSize;
			if (roll > 0) {
				if (intermediateValue < BossMax) {
					intermediateValue += Input.GetAxis("Mouse Scroll Wheel") * 0.2f;
					cam.orthographicSize = Mathf.Clamp(intermediateValue, BossMin, BossMax);
					print(cam.orthographicSize);
				}
			}
			else if (roll < 0) {
				if (intermediateValue > BossMin) {
					intermediateValue += Input.GetAxis("Mouse Scroll Wheel") * 0.2f;
					cam.orthographicSize = Mathf.Clamp(intermediateValue, BossMin, BossMax);
					print(cam.orthographicSize);
				}
			}
		}
		else if (!CameraMovement.Instance.IsInBossRoom && CanZoom) {
			float roll = Input.GetAxis("Mouse Scroll Wheel");
			intermediateValue = cam.orthographicSize;
			if (roll > 0) {
				if (intermediateValue < NormMax) {
					intermediateValue += Input.GetAxis("Mouse Scroll Wheel") * 0.08f;
					cam.orthographicSize = Mathf.Clamp(intermediateValue, NormMin, NormMax);
				}
			}
			else if (roll < 0) {

				if (intermediateValue > NormMin) {
					intermediateValue += Input.GetAxis("Mouse Scroll Wheel") * 0.08f;
					cam.orthographicSize = Mathf.Clamp(intermediateValue, NormMin, NormMax);
				}
			}
		}
		if (matrixA.shape.radius != Camera.main.orthographicSize * 2 + 10 && !CameraMovement.Instance.IsInBossRoom) {

			matrixA.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + Camera.main.orthographicSize, 0);
			matrixB.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - Camera.main.orthographicSize, 0);

			ParticleSystem.ShapeModule shapeA = matrixA.shape;
			ParticleSystem.ShapeModule shapeB = matrixB.shape;

			shapeA.radius = Camera.main.orthographicSize * 2 + 10;
			shapeB.radius = Camera.main.orthographicSize * 2 + 10;
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
