using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Igor.Constants.Strings;

public class SignPost : MonoBehaviour {

	public static event EmptyEventHandler OnAvoidanceBegin;

	private static List<SignPost> readPosts = new List<SignPost>();


	public Sprite signPostNormal;
	public Sprite signPostDark;

	public AudioClip interactionSound;

	private void Interact() {
		switch (gameObject.name) {
			case ObjNames.AVOIDANCE_SIGN: {
				OnAvoidanceBegin?.Invoke();
				MusicHandler.script.TransitionMusic(MusicHandler.script.room1_3_avoidance);
				break;
			}
			case "_SignPost Start": {
				HUDisplay.Instance.DisplayInfo("The virus can not be damaged while it is attacking.", null);
				break;
			}
			case "_SignPost Room 1": {
				HUDisplay.Instance.DisplayInfo("All the spikes you are collecting have a purpose, hold on to them.", null);
				break;
			}
			case "_SignPost PostAvoidance": {
				HUDisplay.Instance.DisplayInfo("Minions of the Virus are deadly, but you have to endure!", null);
				break;
			}
			case "_SignPost Maze": {
				HUDisplay.Instance.DisplayInfo("The coins are up to no use... yet", null);
				break;
			}
			case "_SignPost PreBossEntrance": {
				HUDisplay.Instance.DisplayInfo("Your real challenge awaits inside this portal. Are you prepared? Sorry I can not help.", null);
				break;
			}
			case "_SignPost PreBoss": {
				HUDisplay.Instance.DisplayInfo("Fired bullets can be picked up and reused. Handy if you miss the target. Sorry for telling you this late lel. No regrets.", null);
				break;
			}
			case "_SignPost LaserRoom": {
				HUDisplay.Instance.DisplayInfo(readPosts.Count == 6 ? "Good job, so far you found every single post, I think you deserve a reward." :
																	"This is the seventh sign, but you found only " + readPosts.Count + " disappointed.", null);
				if (readPosts.Count == 6) {
					FindObjectOfType<Coin>().CoinsCollected++;
				}
				break;
			}
			case "_SignPost TeleportationRoom": {
				if (Player.Instance.pMovement.CurrentMovementModifier == PlayerMovementModifiers.INVERT) {
					HUDisplay.Instance.DisplayInfo("The lightning in this room is very unstable, the path can disappear at any moment, and you do not want to misstep!", null);
				}
				else {
					HUDisplay.Instance.DisplayInfo("!petssim ot tnaw ton od uoy dna ,tnemom yna ta raeppasid nac htap eht ,elbatsnu yrev si moor siht ni gninthgil ehT", null);
				}
				break;
			}
			case "_SignPost InvertingRoom": {
				HUDisplay.Instance.DisplayInfo("!desruc si moor sihT", null);
				break;
			}
		}
		StartCoroutine(Fade());
		SoundFXHandler.script.PlayFX(interactionSound);
		readPosts.Add(this);
	}

	public void MapStanceSwitch(MapMode mode) {
		switch (mode) {
			case MapMode.LIGHT: {
				GetComponent<SpriteRenderer>().sprite = signPostNormal;
				return;
			}
			case MapMode.DARK: {
				GetComponent<SpriteRenderer>().sprite = signPostDark;
				return;
			}
		}
	}

	private IEnumerator Fade() {
		SpriteRenderer selfRender = GetComponent<SpriteRenderer>();
		for (int i = 255; i > 0; i--) {
			selfRender.color = new Color32(255, 255, 255, (byte)i);
			yield return null;
		}
		Destroy(gameObject);
	}
}
