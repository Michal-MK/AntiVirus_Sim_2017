using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour {
	public SpriteRenderer sign;
	public GameObject InteractInfo;
	public GameObject E;

	private bool awaitingInput = false;
	private bool interacted = false;

	public delegate void SignPostInteractions();
	public static event SignPostInteractions OnAvoidanceBegin;

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
				break;
			}
		}
	}

	private void Update() {
		if (awaitingInput && Input.GetButtonDown("Interact") && CamFadeOut.script.anim.GetCurrentAnimatorStateInfo(0).IsName("Wait")) {
			if (!interacted) {
				switch (gameObject.name) {
					case "SignPost Avoidance": {
						if (OnAvoidanceBegin != null) {
							OnAvoidanceBegin();
						}
						MusicHandler.script.PlayMusic(MusicHandler.script.room1_3_avoidance);
						StartCoroutine(Fade());
						interacted = true;
						gameObject.GetComponent<BoxCollider2D>().enabled = false;
						InteractInfo.SetActive(false);
						break;
					}
					case "SignPost Start": {
						StartCoroutine(Fade());
						interacted = true;
						gameObject.GetComponent<BoxCollider2D>().enabled = false;
						InteractInfo.SetActive(false);
						Canvas_Renderer.script.InfoRenderer("The virus can not be damaged while it is attacking.", null);
						break;
					}
					case "SignPost Room 1": {
						StartCoroutine(Fade());
						interacted = true;
						gameObject.GetComponent<BoxCollider2D>().enabled = false;
						InteractInfo.SetActive(false);
						Canvas_Renderer.script.InfoRenderer("All the spikes you are collecting have a purpouse, hold on to them.", null);
						break;
					}
					case "SignPost PostAvoidance": {
						StartCoroutine(Fade());
						interacted = true;
						gameObject.GetComponent<BoxCollider2D>().enabled = false;
						InteractInfo.SetActive(false);
						Canvas_Renderer.script.InfoRenderer("Minions of the Virus are deadly, but you have to endure!", null);
						break;
					}
					case "SignPost Maze": {
						StartCoroutine(Fade());
						interacted = true;
						gameObject.GetComponent<BoxCollider2D>().enabled = false;
						InteractInfo.SetActive(false);
						Canvas_Renderer.script.InfoRenderer("The coins are up to no use.", null);
						break;
					}
					case "SignPost PreBoss": {
						StartCoroutine(Fade());
						interacted = true;
						gameObject.GetComponent<BoxCollider2D>().enabled = false;
						InteractInfo.SetActive(false);
						Canvas_Renderer.script.InfoRenderer("Fired bullets can be picked up and reused. Handy if you miss the taget. Sorry for telling you this late lel. No regrets.", null);
						break;
					}
				}
			}

		}
	}
}
