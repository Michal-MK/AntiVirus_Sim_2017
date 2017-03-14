using UnityEngine;

public class BlockScript : MonoBehaviour {
	public RectTransform BG;
	public GameObject player;

	Vector3 currentpos;
	Vector3 startingpos;
	Quaternion startingrotation;
	float dist;

	private bool once = true;

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

		if(once && dist < 10) {
			Canvas_Renderer.script.infoRenderer("Find a pressure plate and put that block on it.", null);
			once = false;
		}

	}
}
