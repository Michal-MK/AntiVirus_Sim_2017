using UnityEngine;

public class Zoom : MonoBehaviour {
	public Camera cam;
	CameraMovement camScript;
	float max = 108f;
	float min = 10f;

	private void Start() {
		camScript = cam.gameObject.GetComponent<CameraMovement>();
	}


	void Update() {
		if (camScript.inBossRoom) {
			float roll= Input.GetAxis("Mouse ScrollWheel");
			if (roll < 0) {
				if (cam.orthographicSize < max) {
					camScript.camWidht = cam.aspect * cam.orthographicSize;
					camScript.camHeight = cam.orthographicSize;
					cam.orthographicSize += 0.5f;
					print(cam.orthographicSize);

				}
			}
			else if (roll > 0) {
				if (cam.orthographicSize > min) {
					camScript.camWidht = cam.aspect * cam.orthographicSize;
					camScript.camHeight = cam.orthographicSize;
					cam.orthographicSize -= 0.5f;
					print(cam.orthographicSize);

				}
			}
		}
	}
	private void LateUpdate() {
		if(cam.orthographicSize > min && cam.orthographicSize < max && camScript.inBossRoom) {
			print(true);
			Vector3 cam_pos = new Vector3(camScript.camX(), camScript.camY(), -10);
			cam.transform.position = cam_pos;
		}
	}
}
