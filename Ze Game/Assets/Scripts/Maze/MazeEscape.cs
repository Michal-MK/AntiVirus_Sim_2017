using System.Collections;
using UnityEngine;

public class MazeEscape : MonoBehaviour {
	public Animator anim;
	public GameObject player;
	public RectTransform BG;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player") {
			StartCoroutine(FromMazeTrans());
		}
	}
	public IEnumerator FromMazeTrans() {
		anim.Play("CamTransition");
		Camera.main.GetComponent<CameraMovement>().inMaze = false;
		yield return new WaitForSeconds(1.5f);
		Camera.main.orthographicSize = Camera.main.orthographicSize * 1.5f;
		player.transform.position = new Vector3(BG.position.x + (BG.sizeDelta.x / 2 - 10), BG.position.y, 0);
		player.transform.localScale = Vector3.one;
		yield return new WaitForSeconds(0.2f);
		
	}
}
