using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEntrance : MonoBehaviour {
	public Maze maze;
	public GameObject player;
	public Spike spike;
	public CameraMovement cam;
	public Animator mazeEntrance;


	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {

			M_Player.gameProgression = 3;
			StartCoroutine(TransToPos());


		}
	}
	public IEnumerator TransToPos() {

		mazeEntrance.Play("CamTransition");
		yield return new WaitForSeconds(2);
		spike.SetPosition();
		cam.inMaze = true;
		cam.mazeCam();
		player.transform.position = maze.grid[GetRandomGridPos(true), GetRandomGridPos(false)].transform.position;
		player.transform.localScale = new Vector3(2, 2, 0);
		yield return new WaitForSeconds(3);
		Canvas_Renderer.script.infoRenderer("What do we have here..?");
		StopAllCoroutines();
	}


	int reference = 0;



	public int GetRandomGridPos(bool xAxis) {

		int side = Random.Range(0, 4);

		if (xAxis) {
			if (side == 0) {
				reference = side;
				int x = Random.Range(0, maze.rowcollCount - 1);
				return x;
			}
			else if (side == 1) {
				reference = side;
				int x = Random.Range(0, 2);
				if (x == 0) {
					return 0;
				}
				else {
					return maze.rowcollCount - 1;
				}
			}
			else if (side == 2) {
				reference = side;
				int x = Random.Range(0, maze.rowcollCount - 1);
				return x;
			}
			else {
				reference = side;
				int x = Random.Range(0, 2);
				if (x == 0) {
					return 0;
				}
				else {
					return maze.rowcollCount - 1;
				}
			}
		}

		else{
			if (reference == 0) {
				int y = Random.Range(0, 2);
				if (y == 0) {
					return 0;
				}
				else {
					return maze.rowcollCount - 1;
				}
			}
			else if (reference == 1) {
				int y = Random.Range(0, maze.rowcollCount - 1);
				return y;
			}
			else if (reference == 2) {
				int y = Random.Range(0, 2);
				if (y == 0) {
					return 0;
				}
				else {
					return maze.rowcollCount - 1;
				}
			}
			else{
				int y = Random.Range(0, maze.rowcollCount - 1);
				return y;
			}
		}
	}
}
