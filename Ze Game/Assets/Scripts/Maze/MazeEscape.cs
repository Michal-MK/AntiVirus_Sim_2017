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

	public GameObject wall;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			StartCoroutine(FromMazeTrans());
			AudioHandler.script.MusicTransition(AudioHandler.script.room1);
		}
	}
	public IEnumerator FromMazeTrans() {
		transitionBlack.Play("CamTransition");
		yield return new WaitForSeconds(2.5f);
		Camera.main.GetComponent<CameraMovement>().inMaze = false;
		zoom.canZoom = true;
		player.transform.position = new Vector3(BG.position.x, BG.position.y + BG.sizeDelta.y / 2 - 10, 0);
		Camera.main.orthographicSize = 25;
		Camera.main.transform.position = player.transform.position;
		player.transform.localScale = Vector3.one;
		spike.SetPosition();
		yield return new WaitForSeconds(0.2f);
		StartCoroutine(FadeWalls());

	}

	private IEnumerator FadeWalls() {

		Image one = wall.GetComponent<Image>();

		Color32 newColor;

		for (float f = 255; f >= -1; f -= Time.deltaTime * 40) {
			if(Time.timeScale != 1) {
				yield return new WaitForSeconds(0.1f);
				yield return null;
			}
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
