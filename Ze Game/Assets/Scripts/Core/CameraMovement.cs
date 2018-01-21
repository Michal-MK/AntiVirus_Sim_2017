using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class CameraMovement : MonoBehaviour {

	public static event Zoom.Zooming OnZoomModeSwitch;

	public RectTransform bg;
	public RectTransform bossRoom;
	public RectTransform player;

	private Vector3 cam_pos;
	private Camera cam;
	private float camWidht;
	private float camHeight;
	private Vector3 middle;
	private float currentBGX;
	private float currentBGY;

	public List<GameObject> BackGroundS = new List<GameObject>();

	public bool inBossRoom = false;
	public bool inMaze = false;

	public ParticleSystem psA;
	public ParticleSystem psB;

	public static bool doneMoving = true;
	public const float defaultCamSize = 15;

	public static CameraMovement script;

	private void Awake() {
		BossBehaviour.OnBossfightBegin += BossBehaviour_OnBossfightBegin;
		MazeEscape.OnMazeEscape += MazeEscape_OnMazeEscape;
		MazeEntrance.OnMazeEnter += MazeEntrance_OnMazeEnter;

		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		Cursor.visible = false;
		cam = GetComponent<Camera>();
		BackGroundS.Add(bg.gameObject);
		camWidht = cam.aspect * cam.orthographicSize;
		camHeight = cam.orthographicSize;
	}

	#region Events
	private void MazeEntrance_OnMazeEnter() {
		psA.gameObject.SetActive(false);
		psB.gameObject.SetActive(false);
		inMaze = true;
	}

	private void MazeEscape_OnMazeEscape() {
		ParticleSystem.ShapeModule shapeA = psA.shape;
		ParticleSystem.ShapeModule shapeB = psB.shape;

		psA.gameObject.SetActive(true);
		psB.gameObject.SetActive(true);

		shapeA.radius = cam.orthographicSize * 2;
		shapeB.radius = cam.orthographicSize * 2;

		inMaze = false;
	}

	private void BossBehaviour_OnBossfightBegin(BossBehaviour sender) {
		SetParticleLifetime(25);
	}

	#endregion

	public void RaycastForRooms() {
		BackGroundS.Clear();

		bg = M_Player.player.GetCurrentBackground();

		currentBGY = bg.sizeDelta.y / 2;
		currentBGX = bg.sizeDelta.x / 2;


		RaycastHit2D[] up = Physics2D.RaycastAll(new Vector2(bg.position.x, bg.position.y), Vector2.up, currentBGY + 10);
		RaycastHit2D[] down = Physics2D.RaycastAll(new Vector2(bg.position.x, bg.position.y), Vector2.down, Mathf.Abs(-currentBGY - 10));
		RaycastHit2D[] left = Physics2D.RaycastAll(new Vector2(bg.position.x, bg.position.y), Vector2.left, Mathf.Abs(-currentBGX - 10));
		RaycastHit2D[] right = Physics2D.RaycastAll(new Vector2(bg.position.x, bg.position.y), Vector2.right, currentBGX + 10);

		//Debug.DrawRay(new Vector2(bg.position.x, bg.position.y), Vector2.up * 100, Color.blue, 5);
		//Debug.DrawRay(new Vector2(bg.position.x, bg.position.y), Vector2.down * 100, Color.green, 5);
		//Debug.DrawRay(new Vector2(bg.position.x, bg.position.y), Vector2.left * 100, Color.red, 5);
		//Debug.DrawRay(new Vector2(bg.position.x, bg.position.y), Vector2.right * 100, Color.yellow, 5);


		foreach (RaycastHit2D hits in up) {
			if (hits.transform.gameObject.activeInHierarchy == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				break;
			}
			if (hits.transform.tag == "BG" && BackGroundS.Contains(hits.transform.gameObject) == false) {
				GameObject hitObj = hits.transform.gameObject;
				BackGroundS.Add(hitObj);
			}
		}
		foreach (RaycastHit2D hits in down) {
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				break;
			}
			if (hits.transform.tag == "BG" && BackGroundS.Contains(hits.transform.gameObject) == false) {
				GameObject hitObj = hits.transform.gameObject;
				BackGroundS.Add(hitObj);
			}
		}
		foreach (RaycastHit2D hits in left) {
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				break;
			}
			if (hits.transform.tag == "BG" && BackGroundS.Contains(hits.transform.gameObject) == false) {
				GameObject hitObj = hits.transform.gameObject;
				BackGroundS.Add(hitObj);
			}
		}
		foreach (RaycastHit2D hits in right) {
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				break;
			}
			if (hits.transform.tag == "BG" && BackGroundS.Contains(hits.transform.gameObject) == false) {
				GameObject hitObj = hits.transform.gameObject;
				BackGroundS.Add(hitObj);
			}
		}
		if (BackGroundS.Count != 0) {
			CalculateArea();
		}
	}

	public void CalculateArea() {

		currentBGX = 0;
		currentBGY = 0;
		int i = 0;
		bool vertical = true;
		bool horizontal = true;

		float xForAll = 0;
		float yForAll = 0;
		foreach (GameObject backgroundObj in BackGroundS) {
			if (i == 0) {
				xForAll = backgroundObj.transform.position.x;
				yForAll = backgroundObj.transform.position.y;
			}
			if (backgroundObj.transform.position.x != xForAll) {
				vertical = false;
			}
			if (backgroundObj.transform.position.y != yForAll) {
				horizontal = false;
			}
		}

		if (horizontal && vertical) {
			horizontal = false;
			vertical = false;
		}

		if (vertical == true) {
			float TopBorder = -Mathf.Infinity;
			float BottomBorder = Mathf.Infinity;
			foreach (GameObject BackGroundRect in BackGroundS) {
				if (BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2 > currentBGX) {
					currentBGX = BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2;
				}
				currentBGY += BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2;
				float specificTop = BackGroundRect.transform.position.y + BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2;
				float specificBottom = BackGroundRect.transform.position.y - BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2;
				if (specificBottom < BottomBorder) {
					BottomBorder = specificBottom;
				}
				if (specificTop > TopBorder) {
					TopBorder = specificTop;
				}
			}
			middle.x = bg.position.x;
			middle.y = (BottomBorder + TopBorder) / 2;
		}
		else if (horizontal == true) {
			float LeftBorder = Mathf.Infinity;
			float RightBorder = -Mathf.Infinity;
			foreach (GameObject BackGroundRect in BackGroundS) {
				if (BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2 > currentBGY) {
					currentBGY = BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2;
				}
				currentBGX += BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2;
				float specificLeft = BackGroundRect.transform.position.x - BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2;
				float specificRight = BackGroundRect.transform.position.x + BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2;

				if (specificLeft < LeftBorder) {
					LeftBorder = specificLeft;
				}
				if (specificRight > RightBorder) {
					RightBorder = specificRight;
				}
			}
			middle.y = bg.position.y;
			middle.x = (LeftBorder + RightBorder) / 2;
		}
		else {
			float TopBorder = -Mathf.Infinity;
			float BottomBorder = Mathf.Infinity;
			foreach (GameObject BackGroundRect in BackGroundS) {
				float specificTop = BackGroundRect.transform.position.y + BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2;
				float specificBottom = BackGroundRect.transform.position.y - BackGroundRect.GetComponent<RectTransform>().sizeDelta.y / 2;
				if (specificBottom < BottomBorder) {
					BottomBorder = specificBottom;
				}
				if (specificTop > TopBorder) {
					TopBorder = specificTop;
				}
			}
			currentBGY = (-BottomBorder + TopBorder) / 2;
			middle.y = (BottomBorder + TopBorder) / 2;
			float LeftBorder = Mathf.Infinity;
			float RightBorder = -Mathf.Infinity;
			foreach (GameObject BackGroundRect in BackGroundS) {

				float specificLeft = BackGroundRect.transform.position.x - BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2;
				float specificRight = BackGroundRect.transform.position.x + BackGroundRect.GetComponent<RectTransform>().sizeDelta.x / 2;
				if (specificLeft < LeftBorder) {
					LeftBorder = specificLeft;
				}
				if (specificRight > RightBorder) {
					RightBorder = specificRight;
				}
			}
			middle.x = (LeftBorder + RightBorder) / 2;
			currentBGX = (-LeftBorder + RightBorder) / 2;
		}
		if (!inMaze && OnZoomModeSwitch != null) {
			OnZoomModeSwitch(true);
		}
	}

	void LateUpdate() {
		camWidht = cam.aspect * cam.orthographicSize;
		camHeight = cam.orthographicSize;

		if (!inBossRoom && !inMaze) {
			cam_pos = new Vector3(camX, camY, -10);
			gameObject.transform.position = cam_pos;
		}
	}

	public IEnumerator LerpSize(float startSize, float finalSize, float lerpSpeedMult, Vector3 pos = default(Vector3)) {
		doneMoving = false;
		if (pos != default(Vector3)) {
			transform.position = pos;
			yield return new WaitForSeconds(0.5f);
		}
		for (float t = 0; t < 1; t += Time.deltaTime * lerpSpeedMult) {
			Camera.main.orthographicSize = Mathf.SmoothStep(startSize, finalSize, t);
			yield return null;
		}
		Camera.main.orthographicSize = finalSize;
		doneMoving = true;
	}

	public float camX {
		get {
			if (player.position.x > currentBGX + middle.x - camWidht) {

				return currentBGX + middle.x - camWidht;

			}
			else if (player.position.x < -currentBGX + middle.x + camWidht) {

				return -currentBGX + middle.x + camWidht;

			}
			else {

				return player.position.x;
			}
		}
	}

	public float camY {
		get {
			if (player.position.y > currentBGY + middle.y - camHeight) {

				return currentBGY + middle.y - camHeight;

			}
			else if (player.position.y < -currentBGY + middle.y + camHeight) {

				return -currentBGY + middle.y + camHeight;

			}
			else {

				return player.position.y;
			}
		}
	}

	public void BossFightCam(int bossNo) {
		inBossRoom = true;

		bossRoom = GameObject.Find(BackgroundNames.BACKGROUND_BOSS_ + bossNo).GetComponent<RectTransform>();

		float bossX = bossRoom.position.x;
		float bossY = bossRoom.position.y;


		player.position = new Vector3(bossX, bossY, 0);
		gameObject.transform.position = new Vector3(bossX, bossY, -10);
		cam.orthographicSize = defaultCamSize;
		ParticleSystem.ShapeModule shapeA = psA.shape;
		psB.gameObject.SetActive(false);

		shapeA.radius = 108 * 2;
		psA.transform.position = bossRoom.transform.position + new Vector3(0, bossRoom.sizeDelta.y / 2, 0);
		ParticleSystem.MainModule main = psA.main;
		main.startLifetime = 25;
	}

	public void SetParticleLifetime(float time) {
		ParticleSystem.ShapeModule shapeA = psA.shape;
		psB.gameObject.SetActive(false);
		print("Hardocded values... ");
		shapeA.radius = 108 * 2;
		psA.transform.position = bossRoom.transform.position + new Vector3(0, bossRoom.sizeDelta.y / 2, 0);
		ParticleSystem.MainModule main = psA.main;
		main.startLifetime = time;
	}

	private void OnDestroy() {
		BossBehaviour.OnBossfightBegin -= BossBehaviour_OnBossfightBegin;
		MazeEscape.OnMazeEscape -= MazeEscape_OnMazeEscape;
		MazeEntrance.OnMazeEnter -= MazeEntrance_OnMazeEnter;
	}


}

