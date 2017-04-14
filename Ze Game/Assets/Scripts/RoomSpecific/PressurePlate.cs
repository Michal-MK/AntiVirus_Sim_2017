using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public Sprite Active;
	public Sprite Inactive;
	SpriteRenderer selfSprite;

	public Spike spike;

	public AudioSource sound;

	public AudioClip On;
	public AudioClip Off;

	public GameObject wall;

	public int attempts = 0;
	public bool alreadyTriggered = false;

	public RectTransform BG;


	private void Awake() {
		Statics.pressurePlate = this;
	}

	void Start() {
		selfSprite = gameObject.GetComponent<SpriteRenderer>();
		gameObject.transform.position = GeneratePos();
	}

	public Vector3 GeneratePos() {
		Vector3 pos = gameObject.transform.position;
		Vector3 newPos = pos;

		float f = Random.value;

		if (f < 0.5) {
			while (Vector3.Distance(pos, newPos) < 10) {
				newPos = new Vector3(pos.x, Random.Range(BG.sizeDelta.y / 2 - 10,pos.y + 10),0);
			}
			return newPos;
		}
		else {
			while (Vector3.Distance(pos, newPos) < 10) {
				newPos = new Vector3(pos.x, Random.Range(-BG.sizeDelta.y / 2 + 10,pos.y - 10),0);
			}
			return newPos;
		}

	}

	void OnTriggerEnter2D(Collider2D col) {
		if (!alreadyTriggered) {
			if (col.name == "Block") {
				selfSprite.sprite = Active;
				sound.clip = On;
				sound.Play();
				spike.SetPosition();
			}
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.name == "Block") {
			attempts++;
			if (attempts == 1) {
				Statics.canvasRenderer.infoRenderer("A projectile pushed the block off of the activator...", "These projectiles sure are a nuisance.");
			}
			if (attempts == 3) {
				CreateBarrier();
			}
			sound.clip = Off;
			sound.Play();
			selfSprite.sprite = Inactive;
			spike.Hide();
		}
	}
	public void CreateBarrier() {
		Statics.canvasRenderer.infoRenderer(null, "Ok, let me help you a little.");
		GameObject protection = Instantiate(wall, gameObject.transform.position + new Vector3(10, 0, 0), Quaternion.identity,gameObject.transform.parent);
		protection.name = "Blocker";
		protection.GetComponent<BoxCollider2D>().isTrigger = true;
		protection.transform.localScale = new Vector3(0.2f, 0.1f, 1);
	}
	private void OnDestroy() {
		Statics.pressurePlate = null;
	}
}
