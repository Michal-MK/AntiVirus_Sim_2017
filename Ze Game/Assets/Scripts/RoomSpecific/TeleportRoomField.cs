using UnityEngine;
using System.Collections;

public class TeleportRoomField : MonoBehaviour {
	public GameObject test;

	public Transform start; // 5,0
	public Transform end1; // 0,5
	public Transform end2; // 5,10

	//private RectTransform teleportBG;
	private Transform[,] fields = new Transform[11, 11];

	private const float xOffset = 16;
	private const float yOffset = 9;

	//5,0 start

	void Start() {
		int counter = 0;
		//teleportBG = MapData.script.GetBackground(7);
		Transform cells = transform.Find("Cells");

		for (int i = 0; i < fields.GetLength(0); i++) {
			for (int j = 0; j < fields.GetLength(1); j++) {
				fields[i, j] = cells.GetChild(counter);
				counter++;
			}
		}
		//for (int i = 0; i < 20; i++) {
		//	int x = Random.Range(0, 11);
		//	int y = Random.Range(0, 11);
		//	Instantiate(test, fields[x, y].position, Quaternion.identity, fields[x, y]);
		//}

		ConnectPoints(new Vector2(5, 0), new Vector2(10, 2));
		ConnectPoints(new Vector2(10, 2), new Vector2(2, 8));
		ConnectPoints(new Vector2(2, 8), new Vector2(5, 10));


		ConnectPoints(new Vector2(5,0), new Vector2(0, 5));
	}

	private void ConnectPoints(Vector2 from, Vector2 to) {
		int currX = (int)from.x;
		int currY = (int)from.y;
		Vector2 direction = to - from;
		Instantiate(test, fields[currX, currY].position, Quaternion.identity, fields[currX, currY]);
		while (direction != Vector2.zero) {
			int oldX = currX;
			int oldY = currY;
			if (direction.x > 0) {
				currX++;
			}
			else if (direction.x < 0) {
				currX--;
			}
			if (direction.y > 0) {

				currY++;
			}
			else if (direction.y < 0) {
				currY--;
			}

			SubTurn(oldX, oldY, currX, currY);
			Instantiate(test, fields[currX, currY].position, Quaternion.identity, fields[currX, currY]);
			direction = to - new Vector2(currX, currY);
			//print(direction);
		}
	}

	private void SubTurn(int x, int y, int toX, int toY) {
		if (x > toX) {
			if (y > toY) {
				Instantiate(test, fields[x, y - 1].position, Quaternion.identity, fields[x, y - 1]);
				//down left
			}
			else if (toY > y) {
				Instantiate(test, fields[x, y + 1].position, Quaternion.identity, fields[x, y + 1]);
				//up left
			}
		}
		else if (toX > x) {
			if (y > toY) {
				Instantiate(test, fields[x, y - 1].position, Quaternion.identity, fields[x, y - 1]);
				//down right
			}
			else if (toY > y) {
				Instantiate(test, fields[x, y + 1].position, Quaternion.identity, fields[x, y + 1]);
				//up right
			}
		}
	}
}
