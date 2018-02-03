using UnityEngine;

public class Guide : MonoBehaviour {

	public delegate void GuideTargetDynamic(GameObject target);
	public delegate void GuideTargetStatic(Vector3 target);

	//Prefab
	public GameObject guidePrefab;
	private GameObject guideObj;
	private SpriteRenderer guideObjSprRender;

	private Vector3 targetPosition;
	private RectTransform playerTransform;

	public float radius;
	private float defaultRadius;

	private void Awake() {
		M_Player.OnTargetableObjectCollision += M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget += Recalculate;
		Spike.OnNewTarget += Recalculate;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	void Start() {
		playerTransform = M_Player.player.GetComponent<RectTransform>();
		defaultRadius = radius;
	}

	#region Event Handling

	private void M_Player_OnTargetableObjectCollision(M_Player sender, GameObject other) {
		if (other.name == "Block") {
			Recalculate(GameObject.Find("Pressure_Plate"));
		}
	}

	private void M_Player_OnSpikePickup(M_Player sender, GameObject other) {
		Recalculate(null);
	}

	private void Coins_OnNewTarget(GameObject target) {
		Recalculate(target);
	}
	private void Spike_OnNewTarget(GameObject target) {
		Recalculate(target);
	}

	#endregion

	private void Recalculate(GameObject destination) {
		if (destination == null) {
			Destroy(guideObj);
			return;
		}

		targetPosition = destination.transform.position;

		if (guideObj == null) {
			guideObj = Instantiate(guidePrefab, Vector3.down, Quaternion.FromToRotation(Vector3.up, (targetPosition - playerTransform.position)), transform);
			guideObjSprRender = guideObj.GetComponent<SpriteRenderer>();
		}
		else {
			guideObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - playerTransform.position));
		}
		gameObject.SetActive(true);
	}

	private void Recalculate(Vector3 targetPosition) {
		if (targetPosition == default(Vector3)) {
			Destroy(guideObj);
			return;
		}

		this.targetPosition = targetPosition;

		if (guideObj == null) {
			guideObj = Instantiate(guidePrefab, Vector3.down, Quaternion.FromToRotation(Vector3.up, (targetPosition - playerTransform.position)), transform);
			guideObjSprRender = guideObj.GetComponent<SpriteRenderer>();
		}
		else {
			guideObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - playerTransform.position));
		}
		gameObject.SetActive(true);
	}

	private void Update() {
		radius = defaultRadius;
		if (guideObj != null && Timer.isRunning == true) {
			Vector3 direction = targetPosition - playerTransform.position;
			if (direction.magnitude < radius) {
				radius = direction.magnitude;
			}
			if (radius < 10) {
				guideObjSprRender.enabled = false;
			}
			else {
				guideObjSprRender.enabled = true;
			}
			guideObj.transform.position = playerTransform.position + direction.normalized * radius;
			guideObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - playerTransform.position));
		}
	}

	private void OnDestroy() {
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
		M_Player.OnTargetableObjectCollision -= M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget -= Recalculate;
		Spike.OnNewTarget -= Recalculate;
	}
}