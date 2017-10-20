using System.Collections;
using UnityEngine;

public class SignPost : MonoBehaviour {
	public SpriteRenderer sign;
	public GameObject InteractInfo;
	public GameObject E;

	private bool awaitingInput = false;
	private bool interact = true;


	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			awaitingInput = true;

			if (!InteractInfo.activeSelf) {
				InteractInfo.SetActive(true);
			}

		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Player") {
			awaitingInput = false;
			if (InteractInfo.activeSelf) {
				InteractInfo.SetActive(false);
			}
		}
	}

	private IEnumerator Fade() {
		for (float f = 255; f > 0; f -= 1) {
			sign.color = new Color32(255, 255, 255, (byte)f);
			if (f > 0) {
				yield return null;
			}
			else {
				sign.color = new Color(1, 1, 1, 0);
				Destroy(gameObject);
				print("Destroyed");
				break;
			}
		}
	}

	private void Update() {
		if (awaitingInput) {
			if (Input.GetButtonDown("Interact")) {
				if (interact && Statics.camFade.anim.GetCurrentAnimatorStateInfo(0).IsName("Wait")) {
					switch (gameObject.name) {
						case "SignPost Avoidance": {
							Statics.music.PlayMusic(Statics.music.avoidance);
							Statics.avoidance.StartAvoidance();
							Statics.avoidance.preformed = true;
							StartCoroutine(Fade());
							interact = false;
							gameObject.GetComponent<BoxCollider2D>().enabled = false;
							InteractInfo.SetActive(false);
							if (Statics.avoidance.displayAvoidInfo) {
								Statics.canvasRenderer.infoRenderer("MuHAhAHAHAHAHAHAHAHAHAHAAAAA!\n" +
																	"You fell for my genious trap, now... DIE!", "Survive, You can zoom out using the Mousewheel");
								Statics.avoidance.displayAvoidInfo = false;
							}
							interact = false;
							break;
						}
						case "SignPost Start": {
							StartCoroutine(Fade());
							interact = false;
							gameObject.GetComponent<BoxCollider2D>().enabled = false;
							InteractInfo.SetActive(false);
							Statics.canvasRenderer.infoRenderer("The virus can not be damaged while attacking.",null);
							break;
						}
						case "SignPost Room 1": {
							StartCoroutine(Fade());
							interact = false;
							gameObject.GetComponent<BoxCollider2D>().enabled = false;
							InteractInfo.SetActive(false);
							Statics.canvasRenderer.infoRenderer("All the spikes you are collecting have a purpouse, hold on to them.",null);
							break;
						}
						case "SignPost PostAvoidance": {
							StartCoroutine(Fade());
							interact = false;
							gameObject.GetComponent<BoxCollider2D>().enabled = false;
							InteractInfo.SetActive(false);
							Statics.canvasRenderer.infoRenderer("Minions of the Virus are deadly, but you have to endure!", null);
							break;
						}
						case "SignPost Maze": {
							StartCoroutine(Fade());
							interact = false;
							gameObject.GetComponent<BoxCollider2D>().enabled = false;
							InteractInfo.SetActive(false);
							Statics.canvasRenderer.infoRenderer("The coins are up to no use.", null);
							break;
						}
						case "SignPost PreBoss": {
							StartCoroutine(Fade());
							interact = false;
							gameObject.GetComponent<BoxCollider2D>().enabled = false;
							InteractInfo.SetActive(false);
							Statics.canvasRenderer.infoRenderer("Fired bullets can be picked up and reused. Handy if you miss the taget.", null);
							break;
						}
					}
				}
			}
		}
	}
}
