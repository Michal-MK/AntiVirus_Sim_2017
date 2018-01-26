using UnityEngine;


public class Guide : MonoBehaviour {

	public delegate void GuideTarget(GameObject target, bool isImmovable);


	//Prefab
	public GameObject guidePrefab;


	private Vector3 targetPosition;
	private Vector3 playerPosition;
	private GameObject pointArrow;
	private Transform guideTransform;
	private RectTransform player;

	private GameObject targetObj;

	private Sprite guide;
	private float radius;

	private void Awake() {
		M_Player.OnCoinPickup += M_Player_OnCoinPickup;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
		M_Player.OnTargetableObjectCollision += M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget += Recalculate;
		Spike.OnNewTarget += Recalculate;
	}

	private void M_Player_OnTargetableObjectCollision(M_Player sender, GameObject other) {
		if(other.name == "Block") {
			Recalculate(GameObject.Find("Pressure_Plate"), true);
		}
	}

	void Start() {
		guideTransform = GameObject.Find("Guide").transform;
		player = M_Player.player.GetComponent<RectTransform>();
		guide = guidePrefab.GetComponent<SpriteRenderer>().sprite;
	}


	private void M_Player_OnSpikePickup(M_Player sender,GameObject other) {
		gameObject.SetActive(false);
	}

	private void M_Player_OnCoinPickup(M_Player sender, GameObject other) {
		print("Hello");
		if (Coin.coinsCollected <= 4) {
			Recalculate(other, true);
		}
		else {
			gameObject.SetActive(false);
		}
	}

	private void Coins_OnNewTarget(GameObject target) {
		Recalculate(target, true);
	}
	private void Spike_OnNewTarget(GameObject target) {
		Recalculate(target, true);
	}

	public void Recalculate(GameObject destination, bool isStatic) {
		if(destination == null) {
			Destroy(pointArrow);
			return;
		}

		targetObj = destination;

		Destroy(pointArrow);

		targetPosition = new Vector3(destination.transform.position.x, destination.transform.position.y, 0);
		playerPosition = new Vector3(player.position.x, player.position.y, 0);

		pointArrow = Instantiate(guidePrefab, Vector3.down, Quaternion.FromToRotation(Vector3.up, (targetPosition - playerPosition)));
		pointArrow.transform.SetParent(guideTransform);

		gameObject.SetActive(true);
	}

	void Update() {
		if (pointArrow != null && Timer.isRunning == true) {
			Vector2 playerMinusDestination = (Vector2)targetObj.transform.position - (Vector2)player.position;
			Vector2 playerMinusDestinationNormal = new Vector2(playerMinusDestination.y, -playerMinusDestination.x);

			float a = playerMinusDestinationNormal.x;
			float b = playerMinusDestinationNormal.y;
			float m = player.position.x;
			float n = player.position.y;
			float c = -a * m - b * n;

			if (Vector3.Distance(playerPosition, targetPosition) > 10) {
				radius = Mathf.Clamp(Vector3.Distance(playerPosition, targetPosition), 5, 10);
				SpriteRenderer sprt = pointArrow.GetComponent<SpriteRenderer>();
				sprt.sprite = guide;
			}
			else if (Vector3.Distance(playerPosition, targetPosition) <= 10) {
				SpriteRenderer sprt = pointArrow.GetComponent<SpriteRenderer>();
				sprt.sprite = null;
			}
			Debug.DrawRay(new Vector3(m, n, 0), playerMinusDestination);

			targetPosition = new Vector3(targetObj.transform.position.x, targetObj.transform.position.y, 0);
			playerPosition = new Vector3(player.position.x, player.position.y, 0);

			pointArrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - playerPosition));

			if (targetPosition.x - m > 0) {

				float X = 1 / (2 * (a * a + b * b)) * (Mathf.Sqrt(Mathf.Pow(2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * radius * radius + 2 * b * c * n + c * c)) - 2 * a * b * n - 2 * a * c + 2 * b * b * m);

				float Yline;
				float Ycirc;

				Yline = (-X * a - c) / b;

				float kinda_r = Mathf.Sqrt(Mathf.Pow((X - m), 2) + Mathf.Pow((Yline - n), 2));

				if (kinda_r > radius - 0.1f && kinda_r < radius + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Yline, 0);
					return;
				}
				else {
					if (targetPosition.y - n > 0) {
						Ycirc = n + Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(radius, 2) - Mathf.Pow(X, 2));
					}
					else if (targetPosition.y - n < 0) {
						Ycirc = n - Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(radius, 2) - Mathf.Pow(X, 2));
					}
					else {
						if (playerMinusDestination.y > 0) {
							Ycirc = radius;
						}
						else {
							Ycirc = -radius;
						}
					}
				}
				kinda_r = (Mathf.Pow((X - m), 2)) + (Mathf.Pow((Ycirc - n), 2));

				if (kinda_r > Mathf.Pow(radius, 2) - 0.1f && kinda_r < Mathf.Pow(radius, 2) + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}
				else {
					if (playerMinusDestination.y > 0) {
						Ycirc = radius + n;
					}
					else {
						Ycirc = -radius + n;
					}
					X = m;
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}
			}
			else if (targetPosition.x - m < 0) {
				float X = 1 / (2 * (a * a + b * b)) *
					(-Mathf.Sqrt(Mathf.Pow(2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * radius * radius + 2 * b * c * n + c * c))
					- 2 * a * b * n - 2 * a * c + 2 * b * b * m);

				float Yline;
				float Ycirc;

				Yline = (-X * a - c) / b;

				float kinda_r = Mathf.Sqrt(Mathf.Pow((X - m), 2) + Mathf.Pow((Yline - n), 2));

				if (kinda_r > radius - 0.1f && kinda_r < radius + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Yline, 0);
					return;
				}
				else {
					if (targetPosition.y - n > 0) {
						Ycirc = n + Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(radius, 2) - Mathf.Pow(X, 2));
					}
					else if (targetPosition.y - n < 0) {
						Ycirc = n - Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(radius, 2) - Mathf.Pow(X, 2));
					}
					else {
						if (playerMinusDestination.y > 0) {
							Ycirc = radius;
						}
						else {
							Ycirc = -radius;
						}
					}
				}
				kinda_r = (Mathf.Pow((X - m), 2)) + (Mathf.Pow((Ycirc - n), 2));

				if (kinda_r > Mathf.Pow(radius, 2) - 0.1f && kinda_r < Mathf.Pow(radius, 2) + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}
				else {
					if (playerMinusDestination.y > 0) {
						Ycirc = radius + n;
					}
					else {
						Ycirc = -radius + n;
					}
					X = m;
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}
			}
		}
	}

	private void OnDestroy() {
		M_Player.OnCoinPickup -= M_Player_OnCoinPickup;
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
		M_Player.OnTargetableObjectCollision -= M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget -= Recalculate;
		Spike.OnNewTarget -= Recalculate;
	}
}