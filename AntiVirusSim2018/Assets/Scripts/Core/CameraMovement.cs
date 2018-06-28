using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class CameraMovement : MonoBehaviour {

	private RectTransform playerRect;

	private RectTransform bg;
	private Camera cam;
	private Vector3 camMidPoint;
	private float allowedCamWidth;
	private float allowefCamHeight;

	public List<RectTransform> accessibleBackgrounds = new List<RectTransform>();

	public bool inBossRoom = false;
	public bool inMaze = false;

	public ParticleSystem psAbove;
	public ParticleSystem psBelow;

	private bool _doneMoving = true;
	public const float defaultCamSize = 15;

	public static CameraMovement script;

	private void Awake() {
		BossBehaviour.OnBossfightBegin += BossBehaviour_OnBossfightBegin;
		MazeEscape.OnMazeEscape += MazeEscape_OnMazeEscape;
		MazeEntrance.OnMazeEnter += MazeEntrance_OnMazeEnter;
		script = this;
	}

	void Start() {
		Cursor.visible = false;
		cam = GetComponent<Camera>();
		playerRect = M_Player.player.GetComponent<RectTransform>();
		bg = M_Player.player.GetCurrentBackground();
		accessibleBackgrounds.Add(bg);
	}

	#region Events
	private void MazeEntrance_OnMazeEnter() {
		psAbove.gameObject.SetActive(false);
		psBelow.gameObject.SetActive(false);
		inMaze = true;
	}

	private void MazeEscape_OnMazeEscape() {
		ParticleSystem.ShapeModule shapeA = psAbove.shape;
		ParticleSystem.ShapeModule shapeB = psBelow.shape;

		psAbove.gameObject.SetActive(true);
		psBelow.gameObject.SetActive(true);

		shapeA.radius = cam.orthographicSize * 2;
		shapeB.radius = cam.orthographicSize * 2;

		inMaze = false;
	}

	private void BossBehaviour_OnBossfightBegin(BossBehaviour sender) {
		BossFightCam(1);
	}

	#endregion

	public void RaycastForRooms() {
		accessibleBackgrounds.Clear();

		bg = M_Player.player.GetCurrentBackground();
		allowefCamHeight = bg.sizeDelta.y / 2;
		allowedCamWidth = bg.sizeDelta.x / 2;

		LayerMask mask = LayerMask.GetMask(Layers.BACKGROUNDS, Layers.WALLS);

		RaycastHit2D[] up = Physics2D.RaycastAll(bg.position, Vector2.up, allowefCamHeight + 10, mask.value);
		RaycastHit2D[] down = Physics2D.RaycastAll(bg.position, Vector2.down, allowefCamHeight + 10, mask.value);
		RaycastHit2D[] left = Physics2D.RaycastAll(bg.position, Vector2.left, allowedCamWidth + 10, mask.value);
		RaycastHit2D[] right = Physics2D.RaycastAll(bg.position, Vector2.right, allowedCamWidth + 10, mask.value);

		foreach (RaycastHit2D[] sides in new RaycastHit2D[4][] { up, right, down, left }) {
			foreach (RaycastHit2D rects in sides) {
				if (rects.transform.gameObject.layer == LayerMask.NameToLayer(Layers.WALLS)) {
					break;
				}
				if (!accessibleBackgrounds.Contains(rects.transform.GetComponent<RectTransform>())) {
					accessibleBackgrounds.Add(rects.transform.GetComponent<RectTransform>());
				}
			}
		}

		if (accessibleBackgrounds.Count != 0) {
			CalculateArea();
		}
	}

	public void CalculateArea() {

		allowedCamWidth = 0;
		allowefCamHeight = 0;
		float maxYOffset = Mathf.NegativeInfinity;
		float maxXOffset = Mathf.NegativeInfinity;
		float minYOffset = Mathf.Infinity;
		float minXOffset = Mathf.Infinity;

		foreach (RectTransform bgRect in accessibleBackgrounds) {
			float specificTop = bgRect.position.y + bgRect.sizeDelta.y / 2;
			float specificRight = bgRect.position.x + bgRect.sizeDelta.x / 2;
			float specificBottom = bgRect.position.y - bgRect.sizeDelta.y / 2;
			float specificLeft = bgRect.position.x - bgRect.sizeDelta.x / 2;

			if (specificBottom < minYOffset) {
				minYOffset = specificBottom;
			}
			if (specificTop > maxYOffset) {
				maxYOffset = specificTop;
			}
			if (specificLeft < minXOffset) {
				minXOffset = specificLeft;
			}
			if (specificRight > maxXOffset) {
				maxXOffset = specificRight;
			}
		}
		allowedCamWidth = (-minXOffset + maxXOffset) / 2;
		allowefCamHeight = (-minYOffset + maxYOffset) / 2;
		camMidPoint.x = (minXOffset + maxXOffset) / 2;
		camMidPoint.y = (minYOffset + maxYOffset) / 2;
	}

	void LateUpdate() {
		if (!inMaze) {
			transform.position = new Vector3(camX, camY, -10);
		}
	}


	public IEnumerator LerpSize(float startSize, float finalSize, float lerpSpeedMult, Vector3 pos = default(Vector3)) {
		_doneMoving = false;
		if (pos != default(Vector3)) {
			transform.position = pos;
			yield return new WaitForSeconds(0.5f);
		}
		for (float t = 0; t < 1; t += Time.deltaTime * lerpSpeedMult) {
			Camera.main.orthographicSize = Mathf.SmoothStep(startSize, finalSize, t);
			yield return null;
		}
		Camera.main.orthographicSize = finalSize;
		_doneMoving = true;
	}

	public float camX {
		get {
			if (playerRect.position.x > allowedCamWidth + camMidPoint.x - cam.aspect * cam.orthographicSize) {
				return allowedCamWidth + camMidPoint.x - cam.aspect * cam.orthographicSize;
			}
			else if (playerRect.position.x < -allowedCamWidth + camMidPoint.x + cam.aspect * cam.orthographicSize) {
				return -allowedCamWidth + camMidPoint.x + cam.aspect * cam.orthographicSize;
			}
			else {
				return playerRect.position.x;
			}
		}
	}

	public float camY {
		get {
			if (playerRect.position.y > allowefCamHeight + camMidPoint.y - cam.orthographicSize) {
				return allowefCamHeight + camMidPoint.y - cam.orthographicSize;
			}
			else if (playerRect.position.y < -allowefCamHeight + camMidPoint.y + cam.orthographicSize) {
				return -allowefCamHeight + camMidPoint.y + cam.orthographicSize;
			}
			else {
				return playerRect.position.y;
			}
		}
	}

	public void BossFightCam(int bossNo) {
		inBossRoom = true;

		RectTransform bossRoom = MapData.script.GetBackgroundBoss(bossNo);

		float bossX = bossRoom.position.x;
		float bossY = bossRoom.position.y;

		playerRect.position = new Vector3(bossX, bossY, 0);
		gameObject.transform.position = new Vector3(bossX, bossY, -10);
		cam.orthographicSize = defaultCamSize;

		ConfigureParticles(25, bossRoom);
	}

	/// <summary>
	/// Configures particle systems dimensions and particle lifetime
	/// </summary>
	/// <param name="time">particles lifetime</param>
	/// <param name="room">room for which to configure, null = camera</param>
	public void ConfigureParticles(float time, RectTransform room) {
		ParticleSystem.ShapeModule shapeA = psAbove.shape;
		psBelow.gameObject.SetActive(false);
		shapeA.radius = room != null ? room.sizeDelta.x : cam.aspect * cam.orthographicSize;
		psAbove.time = psAbove.time * 4;
		psAbove.transform.position = room != null ? room.transform.position + new Vector3(0, room.sizeDelta.y / 2) : cam.transform.position + new Vector3(0,cam.orthographicSize + 10);
		ParticleSystem.MainModule main = psAbove.main;
		main.startLifetime = time;
	}

	private void OnDestroy() {
		BossBehaviour.OnBossfightBegin -= BossBehaviour_OnBossfightBegin;
		MazeEscape.OnMazeEscape -= MazeEscape_OnMazeEscape;
		MazeEntrance.OnMazeEnter -= MazeEntrance_OnMazeEnter;
	}

	public bool isCameraDoneMoving {
		get { return _doneMoving; }
	}

}