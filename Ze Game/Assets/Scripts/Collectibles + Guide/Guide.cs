using UnityEngine;


public class Guide : MonoBehaviour {

	public delegate void GuideTarget(GameObject target, bool isImmovable);

	public M_Player player;

	//Prefab
	public GameObject Arrow;


	private Vector3 destinationpos;
	private Vector3 playerpos;
	private GameObject pointArrow;
	private  Transform GuideObj;

	public GameObject destinationGlobal;

	public Sprite guide;
	public float r;

	private void Awake() {
		M_Player.OnCoinPickup += M_Player_OnCoinPickup;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
		M_Player.OnTargetableObjectCollision += M_Player_OnTargetableObjectCollision;
		Coins.OnNewTarget += Recalculate;
		Spike.OnNewTarget += Recalculate;
	}

	private void M_Player_OnTargetableObjectCollision(M_Player sender, GameObject other) {
		if(other.name == "Block") {
			Recalculate(GameObject.Find("Pressure_Plate"), true);
		}
	}

	void Start() {
		GuideObj = GameObject.Find("Guide").transform;
	}


	private void M_Player_OnSpikePickup(M_Player sender,GameObject other) {
		gameObject.SetActive(false);
	}

	private void M_Player_OnCoinPickup(M_Player sender, GameObject other) {
		if (Coins.coinsCollected <= 4) {
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

		destinationGlobal = destination;

		Destroy(pointArrow);

		destinationpos = new Vector3(destination.transform.position.x, destination.transform.position.y, 0);
		playerpos = new Vector3(player.transform.position.x, player.transform.position.y, 0);

		pointArrow = Instantiate(Arrow, Vector3.down, Quaternion.FromToRotation(Vector3.up, (destinationpos - playerpos)));
		pointArrow.transform.SetParent(GuideObj);

		gameObject.SetActive(true);
	}

	void Update() {
		if (pointArrow != null && Timer.isRunning == true) {
			Vector2 PlayToDestination = (Vector2)destinationGlobal.transform.position - (Vector2)player.transform.position;
			Vector2 normVec = new Vector2(PlayToDestination.y, -PlayToDestination.x);

			float a = normVec.x;
			float b = normVec.y;
			float m = player.transform.position.x;
			float n = player.transform.position.y;
			float c = -a * m - b * n;
			if (Vector3.Distance(playerpos, destinationpos) > 10) {
				r = Mathf.Clamp(Vector3.Distance(playerpos, destinationpos), 5, 10);
				SpriteRenderer sprt = pointArrow.GetComponent<SpriteRenderer>();
				sprt.sprite = guide;
			}
			else if (Vector3.Distance(playerpos, destinationpos) <= 10) {
				SpriteRenderer sprt = pointArrow.GetComponent<SpriteRenderer>();
				sprt.sprite = null;
			}


			Debug.DrawRay(new Vector3(m, n, 0), PlayToDestination);

			destinationpos = new Vector3(destinationGlobal.transform.position.x, destinationGlobal.transform.position.y, 0);
			playerpos = new Vector3(player.transform.position.x, player.transform.position.y, 0);

			pointArrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, (destinationpos - playerpos));

			if (destinationpos.x - m > 0) {

				float X = 1 / (2 * (a * a + b * b)) * (Mathf.Sqrt(Mathf.Pow(2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * r * r + 2 * b * c * n + c * c)) - 2 * a * b * n - 2 * a * c + 2 * b * b * m);

				float Yline;
				float Ycirc;

				Yline = (-X * a - c) / b;

				float kinda_r = Mathf.Sqrt(Mathf.Pow((X - m), 2) + Mathf.Pow((Yline - n), 2));

				if (kinda_r > r - 0.1f && kinda_r < r + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Yline, 0);
					return;
				}
				else {
					if (destinationpos.y - n > 0) {
						Ycirc = n + Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(r, 2) - Mathf.Pow(X, 2));
					}
					else if (destinationpos.y - n < 0) {
						Ycirc = n - Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(r, 2) - Mathf.Pow(X, 2));
					}
					else {
						if (PlayToDestination.y > 0) {
							Ycirc = r;
						}
						else {
							Ycirc = -r;
						}
					}
				}
				kinda_r = (Mathf.Pow((X - m), 2)) + (Mathf.Pow((Ycirc - n), 2));

				if (kinda_r > Mathf.Pow(r, 2) - 0.1f && kinda_r < Mathf.Pow(r, 2) + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}
				else {
					if (PlayToDestination.y > 0) {
						Ycirc = r + n;
					}
					else {
						Ycirc = -r + n;
					}
					X = m;
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}




			}
			else if (destinationpos.x - m < 0) {
				float X = 1 / (2 * (a * a + b * b)) *
					(-Mathf.Sqrt(Mathf.Pow(2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * r * r + 2 * b * c * n + c * c))
					- 2 * a * b * n - 2 * a * c + 2 * b * b * m);


				float Yline;
				float Ycirc;

				Yline = (-X * a - c) / b;

				float kinda_r = Mathf.Sqrt(Mathf.Pow((X - m), 2) + Mathf.Pow((Yline - n), 2));

				if (kinda_r > r - 0.1f && kinda_r < r + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Yline, 0);
					return;
				}
				else {
					if (destinationpos.y - n > 0) {
						Ycirc = n + Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(r, 2) - Mathf.Pow(X, 2));
					}
					else if (destinationpos.y - n < 0) {
						Ycirc = n - Mathf.Sqrt(-Mathf.Pow(m, 2) + 2 * m * X + Mathf.Pow(r, 2) - Mathf.Pow(X, 2));
					}
					else {
						if (PlayToDestination.y > 0) {
							Ycirc = r;
						}
						else {
							Ycirc = -r;
						}
					}
				}
				kinda_r = (Mathf.Pow((X - m), 2)) + (Mathf.Pow((Ycirc - n), 2));

				if (kinda_r > Mathf.Pow(r, 2) - 0.1f && kinda_r < Mathf.Pow(r, 2) + 0.1f) {
					pointArrow.transform.position = new Vector3(X, Ycirc, 0);
				}
				else {
					if (PlayToDestination.y > 0) {
						Ycirc = r + n;
					}
					else {
						Ycirc = -r + n;
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
		Coins.OnNewTarget -= Recalculate;
		Spike.OnNewTarget -= Recalculate;
	}
}