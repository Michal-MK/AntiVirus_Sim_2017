using Igor.Constants.Strings;
using UnityEngine;

public class Guide : MonoBehaviour {

	[SerializeField]
	private GameObject guidePrefab = null;
	[SerializeField]
	private float radius = 5;
	[SerializeField]
	private RectTransform player = null;
	[SerializeField]
	private Vector3 targetPosition = default;


	private GameObject guide;
	private SpriteRenderer guideSpr;


	private void Awake() {
		Player.OnTargetableObjectCollision += M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget += Recalculate;
		Spike.OnNewTarget += Recalculate;
		Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	#region Event Handling

	private void M_Player_OnTargetableObjectCollision(Player sender, GameObject other) {
		if (other.name == ObjNames.BLOCK) {
			Recalculate(GameObject.Find(ObjNames.PRESSURE_PALTE));
		}
	}

	private void M_Player_OnSpikePickup(Player _, Spike __) {
		Recalculate(null);
	}

	#endregion

	private void Recalculate(GameObject destination) {
		if (destination == null) {
			Destroy(guide);
		}
		else {
			SetupGuiding(destination.transform.position);
		}
	}

	private void Recalculate(Vector3 targetPos) {
		if (targetPos == default) {
			Destroy(guide);
		}
		else {
			SetupGuiding(targetPos);
		}
	}

	private void SetupGuiding(Vector3 pos) {
		targetPosition = pos;

		if (guide == null) {
			guide = Instantiate(guidePrefab, Vector3.down, Quaternion.FromToRotation(Vector3.up, (targetPosition - player.position)), transform);
			guideSpr = guide.GetComponent<SpriteRenderer>();
		}
		gameObject.SetActive(true);
	}

	private void Update() {
		if (guide != null && Timer.Instance.IsRunning) {
			Vector3 direction = targetPosition - player.position;
			guideSpr.enabled = direction.magnitude >= radius;
			guide.transform.position = player.position + direction.normalized * radius;
			guide.transform.rotation = Quaternion.FromToRotation(Vector3.up, (targetPosition - player.position));
		}
	}

	private void OnDestroy() {
		Player.OnSpikePickup -= M_Player_OnSpikePickup;
		Player.OnTargetableObjectCollision -= M_Player_OnTargetableObjectCollision;
		Coin.OnNewTarget -= Recalculate;
		Spike.OnNewTarget -= Recalculate;
	}
}