using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public class SignPost : MonoBehaviour {
	public SpriteRenderer sign;
	public GameObject interactionInfo;

	private bool awaitingInput = false;
	private bool interacted = false;

	public delegate void SignPostInteractions();
	public static event SignPostInteractions OnAvoidanceBegin;

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player") {
			awaitingInput = true;

			if (!interactionInfo.activeSelf) {
				interactionInfo.SetActive(true);
			}

		}
	}
	private void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Player") {
			awaitingInput = false;
			if (interactionInfo.activeSelf) {
				interactionInfo.SetActive(false);
			}
		}
	}
	private IEnumerator Fade() {
		for (float f = 255; f > 0; f -= 1) {
			sign.color = new Color32(255, 255, 255, (byte)f);
			yield return null;
		}
		Destroy(gameObject);
	}

	private void Update() {
		if (awaitingInput && Input.GetButtonDown("Interact") && CamFadeOut.script.anim.GetCurrentAnimatorStateInfo(0).IsName("Wait")) {
			if (!interacted) {
				switch (gameObject.name) {
					case ObjNames.AVOIDANCE_SIGN: {
						if (OnAvoidanceBegin != null) {
							OnAvoidanceBegin();
						}
						MusicHandler.script.TrasnsitionMusic(MusicHandler.script.room1_3_avoidance);
						break;
					}
					case "SignPost Start": {
						Canvas_Renderer.script.InfoRenderer("The virus can not be damaged while it is attacking.", null);
						break;
					}
					case "SignPost Room 1": {
						Canvas_Renderer.script.InfoRenderer("All the spikes you are collecting have a purpouse, hold on to them.", null);
						break;
					}
					case "SignPost PostAvoidance": {
						Canvas_Renderer.script.InfoRenderer("Minions of the Virus are deadly, but you have to endure!", null);
						break;
					}
					case "SignPost Maze": {
						Canvas_Renderer.script.InfoRenderer("The coins are up to no use. Yet", null);
						break;
					}
					case "SignPost PreBoss": {
						Canvas_Renderer.script.InfoRenderer("Fired bullets can be picked up and reused. Handy if you miss the taget. Sorry for telling you this late lel. No regrets.", null);
						break;
					}
				}
				StartCoroutine(Fade());
				interacted = true;
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				interactionInfo.SetActive(false);
			}
		}
	}
}
