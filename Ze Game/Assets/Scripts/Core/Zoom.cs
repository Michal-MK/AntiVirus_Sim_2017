using UnityEngine;

public class Zoom : MonoBehaviour {
	public Camera cam;
	CameraMovement camScript;
	float BossMax = 108f;
	float BossMin = 10f;

	float NormMax = 25;
	float NormMin = 15;

	private void Start() {
		camScript = cam.gameObject.GetComponent<CameraMovement>();
	}


	void Update() {
		if (camScript.inBossRoom) {
			float roll = Input.GetAxis("Mouse ScrollWheel");
			if (roll < 0) {
				if (cam.orthographicSize < BossMax) {
					camScript.camWidht = cam.aspect * cam.orthographicSize;
					camScript.camHeight = cam.orthographicSize;
					cam.orthographicSize += 0.8f;


				}
			}
			else if (roll > 0) {
				if (cam.orthographicSize > BossMin) {
					camScript.camWidht = cam.aspect * cam.orthographicSize;
					camScript.camHeight = cam.orthographicSize;
					cam.orthographicSize -= 0.8f;


				}
			}
		}
		else {
			float roll = Input.GetAxis("Mouse ScrollWheel");
			if (roll < 0) {
				if (cam.orthographicSize < NormMax) {
					camScript.camWidht = cam.aspect * cam.orthographicSize;
					camScript.camHeight = cam.orthographicSize;
					cam.orthographicSize += 0.8f;


				}
			}
			else if (roll > 0) {
				if (cam.orthographicSize > NormMin) {
					camScript.camWidht = cam.aspect * cam.orthographicSize;
					camScript.camHeight = cam.orthographicSize;
					cam.orthographicSize -= 0.8f;


				}
			}
		}
	}
	private void LateUpdate() {
		if (camScript.inBossRoom && cam.orthographicSize > BossMin && cam.orthographicSize < BossMax ) {
			Vector3 cam_pos = new Vector3(camScript.camX(), camScript.camY(), -10);
			cam.transform.position = cam_pos;
		}
		else if (!camScript.inBossRoom && cam.orthographicSize > NormMin && cam.orthographicSize < NormMax) {
			Vector3 cam_pos = new Vector3(camScript.camX(), camScript.camY(), -10);
			cam.transform.position = cam_pos;
		}
	}
}
