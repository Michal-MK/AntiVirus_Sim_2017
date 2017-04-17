using UnityEngine;

public class Zoom : MonoBehaviour {
	public Camera cam;
	public ParticleSystem matrixA;
	public ParticleSystem matrixB;

	public bool canZoom = true;

	float BossMax = 108f;
	float BossMin = 10f;

	float NormMax = 25;
	float NormMin = 15;


	private void Awake() {
		Statics.zoom = this;
	}

	private void LateUpdate() {
		//print(Input.GetAxis("Mouse Scroll Wheel"));
		if (Statics.cameraMovement.inBossRoom && canZoom) {
			float roll = Input.GetAxis("Mouse Scroll Wheel");
			
			if (roll > 0) {
				if (cam.orthographicSize < BossMax) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.2f;
				}
			}
			else if (roll < 0) {
				if (cam.orthographicSize > BossMin) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.2f;
				}
			}
			Vector3 cam_pos = new Vector3(Statics.cameraMovement.camX(), Statics.cameraMovement.camY(), -10);
			//
			cam.transform.position = cam_pos;
			//
		}
		else if (!Statics.cameraMovement.inBossRoom && canZoom) {
			float roll = Input.GetAxis("Mouse Scroll Wheel");
			//print("Heeeeere");
			if (roll > 0) {
				if (cam.orthographicSize < NormMax) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.08f;
				}
			}
			else if (roll < 0) {
				
				if (cam.orthographicSize > NormMin) {
					cam.orthographicSize += Input.GetAxis("Mouse Scroll Wheel") * 0.08f;
				}
			}
			Vector3 cam_pos = new Vector3(Statics.cameraMovement.camX(), Statics.cameraMovement.camY(), -10);
			cam.transform.position = cam_pos;
		}
		if (matrixA.shape.radius != Camera.main.orthographicSize * 2 + 10 && !Statics.cameraMovement.inBossRoom) {

			matrixA.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + Camera.main.orthographicSize,0);
			matrixB.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - Camera.main.orthographicSize, 0);

			ParticleSystem.ShapeModule shapeA = matrixA.shape;
			ParticleSystem.ShapeModule shapeB = matrixB.shape;

			shapeA.radius = Camera.main.orthographicSize * 2 + 10;
			shapeB.radius = Camera.main.orthographicSize * 2 + 10;			
		}
	}
	private void OnDestroy() {
		Statics.zoom = null;
	}
}
