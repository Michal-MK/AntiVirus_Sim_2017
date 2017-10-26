using UnityEngine;

public class BlockScript : MonoBehaviour {
	public RectTransform BG;
	public GameObject player;

	Vector3 currentpos;
	Vector3 startingpos;
	private Quaternion startingrotation = Quaternion.Euler(0,0,0);

	float dist;

	public static bool pressurePlateTriggered = false;

	public bool showInfo = true;

	private void Awake() {
		Statics.blockScript = this;
	}

	private void Start() {
		startingpos = gameObject.transform.position;
		startingrotation = gameObject.transform.localRotation;
	}

	private void Update() {
		currentpos = gameObject.transform.position;
		dist = Vector3.Distance(player.transform.position, gameObject.transform.position);

		if (currentpos.x < BG.position.x + -BG.sizeDelta.x / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
		else if(currentpos.x > BG.position.x + BG.sizeDelta.x / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
		else if (currentpos.y < BG.position.y + -BG.sizeDelta.y / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
		else if (currentpos.y > BG.position.y + BG.sizeDelta.y / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}

		if(showInfo && dist < 10) {
			Statics.canvasRenderer.infoRenderer("Find the activator and put the block in front of you on it.", null);
			showInfo = false;
		}

	}
	private void OnDestroy() {
		Statics.blockScript = null;
	}
}
