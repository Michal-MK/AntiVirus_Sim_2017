using UnityEngine;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class TeleportRoomField : MonoBehaviour {
	public GameObject test;

	public GameObject[] warnings;

	private Transform signPost;

	public LeverRelay relay;

	private Transform[,] fields = new Transform[11, 11];
	private List<Transform> pathFields = new List<Transform>();
	private Transform dangerWarnings;

	private const int MID_AXES = 5;

	private void Awake() {
		Player.OnRoomEnter += M_Player_OnRoomEnter;
		foreach (Lever l in relay.Levers) {
			l.OnLeverSwitch += L_OnLeverSwitch;
		}
		signPost = transform.Find("_SignPost TeleportationRoom");
	}

	private void L_OnLeverSwitch(Lever sender, bool isOn) {
		if (relay.LeversActivated == 0) {
			foreach (SpriteRenderer render in dangerWarnings.GetComponentsInChildren<SpriteRenderer>()) {
				render.color = new Color(1, 0.2f, 0.2f, 1);
			}
			print(pathFields.Count);
			foreach (Transform t in pathFields) {
				t.GetComponentInChildren<Animator>().Play("Alpha Pulse Sprite");
			}
		}
		else if (relay.LeversActivated == 1) {
			foreach (SpriteRenderer render in dangerWarnings.GetComponentsInChildren<SpriteRenderer>()) {
				render.color = new Color(1, 1, 1, 0);
			}
			print(pathFields.Count);
			foreach (Transform t in pathFields) {
				t.GetChild(0).gameObject.SetActive(false);
			}
			MapData.Instance.GetRoomLink(6, 10).OpenDoor();
		}
		sender.OnLeverSwitch -= L_OnLeverSwitch;
	}

	private void M_Player_OnRoomEnter(Player sender, RectTransform background, RectTransform previous) {
		if (background == MapData.Instance.GetRoom(6).Background) {
			FillRoom();
			print("Entering LEVER RELAY");
			Player.OnRoomEnter -= M_Player_OnRoomEnter;
			Player.OnRoomEnter += OnTopRoomEnter;
		}
	}

	private void OnTopRoomEnter(Player sender, RectTransform background, RectTransform previous) {
		if (background == MapData.Instance.GetTransition(MapData.Instance.GetRoomLink(7, 9))) {
			print("Entering Transition to top room");
			Player.Instance.pMovement.SetMovementMode(PlayerMovementType.TELEPORT);
			MapData.Instance.GetRoom(9).Background.parent.Find("Cherries_Apples").gameObject.SetActive(true);
			Player.OnRoomEnter -= OnTopRoomEnter;
		}
	}

	void Start() {
		int counter = 0;
		Transform cells = transform.Find("Cells");
		dangerWarnings = transform.Find("_DangerWarnings");

		for (int i = 0; i < fields.GetLength(0); i++) {
			for (int j = 0; j < fields.GetLength(1); j++) {
				fields[i, j] = cells.GetChild(counter);
				counter++;
			}
		}
	}

	private void FillRoom() {
		Vector2 start = new Vector2(5, 0);
		bool isSharingPossible = Chance.Half();

		Vector2 point1 = new Vector2(Random.Range(MID_AXES + 3, MID_AXES * 2), Random.Range(1, MID_AXES - 1));
		Vector2 pointA = new Vector2(MID_AXES, MID_AXES);
		Vector2 pointB = isSharingPossible ? new Vector2(Random.Range(1, MID_AXES - 1), Random.Range(MID_AXES + 1, MID_AXES * 2)) : new Vector2(Random.Range(MID_AXES + 2, MID_AXES * 2 - 1), Random.Range(MID_AXES + 2, MID_AXES * 2 - 1));
		Vector2 pointC = isSharingPossible ? pointB : new Vector2(Random.Range(1, MID_AXES - 1), Random.Range(1, MID_AXES - 1));

		Vector2 deadEnd = isSharingPossible ? new Vector2(Random.Range(MID_AXES + 2, MID_AXES * 2), Random.Range(MID_AXES + 1, MID_AXES * 2)) : new Vector2(Random.Range(0, MID_AXES), Random.Range(MID_AXES + 3, MID_AXES * 2));

		Vector2 leftExit = new Vector2(0, 5);
		Vector2 upperExit = new Vector2(5, 10);

		ConnectPoints(start, point1);
		ConnectPoints(isSharingPossible ? point1 : pointA, deadEnd);
		ConnectPoints(point1, pointA);
		ConnectPoints(pointA, pointB);
		ConnectPoints(isSharingPossible ? pointB : pointA, pointC);
		ConnectPoints(pointB, upperExit);
		ConnectPoints(pointC, leftExit);

		signPost.position = fields[(int)deadEnd.x, (int)deadEnd.y].position;

		FillHazards();
	}

	private void FillHazards() {
		foreach (Transform t in fields) {
			if (!pathFields.Contains(t)) {
				if (Chance.Quarter()) {
					Instantiate(warnings[Random.Range(0, warnings.Length)], t.position, Quaternion.identity, dangerWarnings);
				}
			}
		}
	}

	private void ConnectPoints(Vector2 from, Vector2 to) {
		int currX = (int)from.x;
		int currY = (int)from.y;
		Vector2 direction = to - from;
		if (fields[currX, currY].childCount == 0) {
			Instantiate(test, fields[currX, currY].position, Quaternion.identity, fields[currX, currY]);
		}
		fields[currX, currY].tag = Tags.UNTAGGED;
		pathFields.Add(fields[currX, currY]);
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
			pathFields.Add(fields[currX, currY]);
			fields[currX, currY].tag = Tags.UNTAGGED;
			if (fields[currX, currY].childCount == 0) {
				Instantiate(test, fields[currX, currY].position, Quaternion.identity, fields[currX, currY]);
			}
			direction = to - new Vector2(currX, currY);
		}
	}

	private void SubTurn(int x, int y, int toX, int toY) {
		Vector2Int modified = new Vector2Int(-1, -1);
		if (x > toX) {
			if (y > toY) {
				if (Chance.Half()) {
					modified = new Vector2Int(x, y - 1);
				}
				else {
					modified = new Vector2Int(x - 1, y);
				}
			}
			else if (toY > y) {
				if (Chance.Half()) {
					modified = new Vector2Int(x, y + 1);
				}
				else {
					modified = new Vector2Int(x - 1, y);
				}
			}
		}
		else if (toX > x) {
			if (y > toY) {
				if (Chance.Half()) {
					modified = new Vector2Int(x, y - 1);
				}
				else {
					modified = new Vector2Int(x + 1, y);
				}
			}
			else if (toY > y) {
				if (Chance.Half()) {
					modified = new Vector2Int(x, y + 1);
				}
				else {
					modified = new Vector2Int(x + 1, y);
				}
			}
		}
		if (modified != -Vector2.one) {
			fields[modified.x, modified.y].tag = Tags.UNTAGGED;
			pathFields.Add(fields[modified.x, modified.y]);
			if (fields[modified.x, modified.y].childCount == 0) {
				Instantiate(test, fields[modified.x, modified.y].position, Quaternion.identity, fields[modified.x, modified.y]);
			}
		}
	}
}