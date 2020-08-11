using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Igor.Constants.Strings;
using System;

public class CameraMovement : MonoBehaviour {

	private RectTransform player;
	private RectTransform background;
	private Camera cam;

	private Vector3 camMidPoint;
	private float allowedCamWidth;
	private float allowefCamHeight;

	[SerializeField]
	private List<RectTransform> accessibleBackgrounds = new List<RectTransform>();
	[SerializeField]
	private ParticleSystem above = null;
	[SerializeField]
	private ParticleSystem below = null;

	[SerializeField]
	private bool inBossRoom = false;
	public bool IsInBossRoom { get => inBossRoom; set { inBossRoom = value; } }
	[SerializeField]
	private bool inMaze = false;
	public bool IsInMaze { get => inMaze; set { inMaze = value; } }

	public bool CameraStill { get; private set; } = true;

	public const float DEFAULT_CAM_SIZE = 15;

	public static CameraMovement Instance { get; private set; }

	#region Lifecycle

	private void Awake() {
		BossBehaviour.OnBossfightBegin += BossBehaviour_OnBossfightBegin;
		MazeEscape.OnMazeEscape += MazeEscape_OnMazeEscape;
		MazeEntrance.OnMazeEnter += MazeEntrance_OnMazeEnter;
		Instance = this;
	}

	void Start() {
		Cursor.visible = false;
		cam = GetComponent<Camera>();
		player = Player.Instance.GetComponent<RectTransform>();
		background = Player.Instance.GetCurrentBackground();
		accessibleBackgrounds.Add(background);
	}

	private void OnDestroy() {
		BossBehaviour.OnBossfightBegin -= BossBehaviour_OnBossfightBegin;
		MazeEscape.OnMazeEscape -= MazeEscape_OnMazeEscape;
		MazeEntrance.OnMazeEnter -= MazeEntrance_OnMazeEnter;
	}

	#endregion

	#region Events

	private void MazeEntrance_OnMazeEnter(object sender, EventArgs e) {
		above.gameObject.SetActive(false);
		below.gameObject.SetActive(false);
		inMaze = true;
	}

	private void MazeEscape_OnMazeEscape(object sender, EventArgs e) {
		ParticleSystem.ShapeModule shapeA = above.shape;
		ParticleSystem.ShapeModule shapeB = below.shape;

		above.gameObject.SetActive(true);
		below.gameObject.SetActive(true);

		shapeA.radius = cam.orthographicSize * 2;
		shapeB.radius = cam.orthographicSize * 2;

		inMaze = false;
	}

	private void BossBehaviour_OnBossfightBegin(object sender, BossEncouterEventArgs e) {
		BossFightCam(e.BossID);
	}
	
	#endregion

	public void RaycastForRooms() {
		accessibleBackgrounds.Clear();

		background = Player.Instance.GetCurrentBackground();
		allowefCamHeight = background.sizeDelta.y / 2;
		allowedCamWidth = background.sizeDelta.x / 2;

		LayerMask mask = LayerMask.GetMask(Layers.BACKGROUNDS, Layers.WALLS);

		RaycastHit2D[] up = Physics2D.RaycastAll(background.position, Vector2.up, allowefCamHeight + 10, mask.value);
		RaycastHit2D[] down = Physics2D.RaycastAll(background.position, Vector2.down, allowefCamHeight + 10, mask.value);
		RaycastHit2D[] left = Physics2D.RaycastAll(background.position, Vector2.left, allowedCamWidth + 10, mask.value);
		RaycastHit2D[] right = Physics2D.RaycastAll(background.position, Vector2.right, allowedCamWidth + 10, mask.value);

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


	public IEnumerator LerpSize(float startSize, float finalSize, float lerpSpeedMult, Vector3 pos = default) {
		CameraStill = false;
		if (pos != default) {
			transform.position = pos;
			yield return new WaitForSeconds(0.5f);
		}
		for (float t = 0; t < 1; t += Time.deltaTime * lerpSpeedMult) {
			Camera.main.orthographicSize = Mathf.SmoothStep(startSize, finalSize, t);
			yield return null;
		}
		Camera.main.orthographicSize = finalSize;
		CameraStill = true;
	}

	public float camX {
		get {
			if (player.position.x > allowedCamWidth + camMidPoint.x - cam.aspect * cam.orthographicSize) {
				return allowedCamWidth + camMidPoint.x - cam.aspect * cam.orthographicSize;
			}
			else if (player.position.x < -allowedCamWidth + camMidPoint.x + cam.aspect * cam.orthographicSize) {
				return -allowedCamWidth + camMidPoint.x + cam.aspect * cam.orthographicSize;
			}
			else {
				return player.position.x;
			}
		}
	}

	public float camY {
		get {
			if (player.position.y > allowefCamHeight + camMidPoint.y - cam.orthographicSize) {
				return allowefCamHeight + camMidPoint.y - cam.orthographicSize;
			}
			else if (player.position.y < -allowefCamHeight + camMidPoint.y + cam.orthographicSize) {
				return -allowefCamHeight + camMidPoint.y + cam.orthographicSize;
			}
			else {
				return player.position.y;
			}
		}
	}

	public void BossFightCam(int bossNo) {
		inBossRoom = true;

		RectTransform bossRoom = MapData.Instance.GetBackgroundBoss(bossNo);

		float bossX = bossRoom.position.x;
		float bossY = bossRoom.position.y;

		player.position = new Vector3(bossX, bossY, 0);
		gameObject.transform.position = new Vector3(bossX, bossY, -10);
		cam.orthographicSize = DEFAULT_CAM_SIZE;

		ConfigureParticles(25, bossRoom);
	}

	/// <summary>
	/// Configures particle systems dimensions and particle lifetime
	/// </summary>
	public void ConfigureParticles(float time, RectTransform room) {
		ParticleSystem.ShapeModule shapeA = above.shape;
		below.gameObject.SetActive(false);
		shapeA.radius = room != null ? room.sizeDelta.x : cam.aspect * cam.orthographicSize;
		above.time = above.time * 4;
		above.transform.position = room != null ? room.transform.position + new Vector3(0, room.sizeDelta.y / 2) : cam.transform.position + new Vector3(0,cam.orthographicSize + 10);
		ParticleSystem.MainModule main = above.main;
		main.startLifetime = time;
	}
}