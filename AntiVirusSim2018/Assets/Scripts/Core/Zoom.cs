using UnityEngine;

public class Zoom : MonoBehaviour {

	public Camera cam;
	public ParticleSystem matrixA;
	public ParticleSystem matrixB;

	private static bool _canZoom = true;

	public float BossMax = 107f;
	public float BossMin = 15f;

	public float NormMax = 25;
	public float NormMin = 15;

	private float intermediateValue = 0;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		_canZoom = data.player.canZoom;
	}

	private void LateUpdate() {
		if (CameraMovement.script.inBossRoom && canZoom) {
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
		else if (!CameraMovement.script.inBossRoom && canZoom) {
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
		if (matrixA.shape.radius != Camera.main.orthographicSize * 2 + 10 && !CameraMovement.script.inBossRoom) {

			matrixA.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + Camera.main.orthographicSize, 0);
			matrixB.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - Camera.main.orthographicSize, 0);

			ParticleSystem.ShapeModule shapeA = matrixA.shape;
			ParticleSystem.ShapeModule shapeB = matrixB.shape;

			shapeA.radius = Camera.main.orthographicSize * 2 + 10;
			shapeB.radius = Camera.main.orthographicSize * 2 + 10;
		}
	}

	public static bool canZoom {
		get { return _canZoom; }
		set { _canZoom = value; }
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
