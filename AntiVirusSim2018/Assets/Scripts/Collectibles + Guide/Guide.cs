using UnityEngine;

public class Guide : MonoBehaviour {

	public GameObject guidePrefab;


	private GameObject guideObj;
	private SpriteRenderer guideSpr;

	private Vector3 targetPosition;
	private RectTransform player;

	private float defaultRadius;
	[SerializeField]
	private float radius = 10;


	private void Awake() {
		Player.OnTargetableObjectCollision += M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget += Recalculate;
		Spike.OnNewTarget += Recalculate;
		Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	void Start() {
		player = Player.Instance.GetComponent<RectTransform>();
		defaultRadius = radius;
	}

	#region Event Handling

	private void M_Player_OnTargetableObjectCollision(Player sender, GameObject other) {
		if (other.name == "Block") {
			Recalculate(GameObject.Find("Pressure_Plate"));
		}
	}

	private void M_Player_OnSpikePickup(Player sender, GameObject other) {
		Recalculate(null);
	}

	#endregion

	private void Recalculate(GameObject destination) {
		if (destination == null) {
			Destroy(guideObj);
		}
		else {
			SetupGuiding(destination.transform.position);
		}
	}

	private void Recalculate(Vector3 targetPos) {
		if (targetPos == default) {
			Destroy(guideObj);
		}
		else {
			SetupGuiding(targetPos);
		}
	}

	private void SetupGuiding(Vector3 pos) {
		targetPosition = pos;

		if (guideObj == null) {
			guideObj = Instantiate(guidePrefab, Vector3.down, Quaternion.FromToRotation(Vector3.up, (targetPosition - player.position)), transform);
			guideSpr = guideObj.GetComponent<SpriteRenderer>();
		}
		else {
			guideObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - player.position));
		}
		gameObject.SetActive(true);
	}

	private void Update() {
		radius = defaultRadius;
		if (guideObj != null && Timer.script.isRunning == true) {
			Vector3 direction = targetPosition - player.position;
			if (direction.magnitude < radius) {
				radius = direction.magnitude;
			}
			guideSpr.enabled = radius >= defaultRadius;
			guideObj.transform.position = player.position + direction.normalized * radius;
			guideObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - player.position));
		}
	}

	private void OnDestroy() {
		Player.OnSpikePickup -= M_Player_OnSpikePickup;
		Player.OnTargetableObjectCollision -= M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget -= Recalculate;
		Spike.OnNewTarget -= Recalculate;
	}
}