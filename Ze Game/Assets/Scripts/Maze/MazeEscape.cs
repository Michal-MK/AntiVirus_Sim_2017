using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MazeEscape : MonoBehaviour {
	public Animator transitionBlack;
	public GameObject player;
	public RectTransform BG;
	public Spike spike;
	public Zoom zoom;
	public MazeEntrance entrance;
	public Wrapper wrp;

	public bool pathOpen = false;

	public GameObject wall;

	private void Awake() {
		Statics.mazeEscape = this;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			StartCoroutine(FromMazeTrans());
			Statics.music.MusicTransition(Statics.music.room1);
		}
	}

	public IEnumerator FromMazeTrans() {
		entrance.gameObject.SetActive(false);
		Statics.camFade.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES);
		yield return new WaitForSeconds(1.5f);
		Camera.main.GetComponent<CameraMovement>().inMaze = false;
		zoom.canZoom = true;
		player.transform.position = new Vector3(BG.position.x, BG.position.y + BG.sizeDelta.y / 2 - 10, 0);
		Camera.main.orthographicSize = 25;
		Camera.main.transform.position = player.transform.position;
		player.transform.localScale = Vector3.one;
		spike.SetPosition();
		StartCoroutine(FadeWalls());
		pathOpen = true;

		ParticleSystem.ShapeModule shapeA = Statics.cameraMovement.psA.shape;
		ParticleSystem.ShapeModule shapeB = Statics.cameraMovement.psB.shape;

		ParticleSystem psA = Statics.cameraMovement.psA;
		ParticleSystem psB = Statics.cameraMovement.psB;

		psA.gameObject.SetActive(true);
		psB.gameObject.SetActive(true);

		shapeA.radius = Camera.main.orthographicSize * 2;
		shapeB.radius = Camera.main.orthographicSize * 2;
		
		wrp.AllowSaving(true);
	}

	private IEnumerator FadeWalls() {
		SpriteRenderer one = wall.GetComponent<SpriteRenderer>();

		Color32 newColor;

		for (float f = 255; f >= -1; f -= 0.5f) {
			if (Time.timeScale != 1) {
				f = f + 0.5f;
				yield return null;
			}
			else {
				newColor = new Color32(255, 255, 255, (byte)f);
				one.color = newColor;

				if (f > 0) {
					//print(f);
					yield return null;
				}
				else if (f <= 0) {
					wall.SetActive(false);
					break;
				}
			}
		}
	}
	private void OnDestroy() {
		Statics.mazeEscape = null;
	}
}
