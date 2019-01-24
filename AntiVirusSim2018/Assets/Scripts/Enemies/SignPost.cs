using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Igor.Constants.Strings;

public class SignPost : MonoBehaviour {

	public delegate void SignPostInteractions();
	public static event SignPostInteractions OnAvoidanceBegin;

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
				Canvas_Renderer.script.DisplayInfo("The virus can not be damaged while it is attacking.", null);
				break;
			}
			case "_SignPost Room 1": {
				Canvas_Renderer.script.DisplayInfo("All the spikes you are collecting have a purpose, hold on to them.", null);
				break;
			}
			case "_SignPost PostAvoidance": {
				Canvas_Renderer.script.DisplayInfo("Minions of the Virus are deadly, but you have to endure!", null);
				break;
			}
			case "_SignPost Maze": {
				Canvas_Renderer.script.DisplayInfo("The coins are up to no use... yet", null);
				break;
			}
			case "_SignPost PreBossEntrance": {
				Canvas_Renderer.script.DisplayInfo("Your real challenge awaits inside this portal. Are you prepared? Sorry I can not help.", null);
				break;
			}
			case "_SignPost PreBoss": {
				Canvas_Renderer.script.DisplayInfo("Fired bullets can be picked up and reused. Handy if you miss the target. Sorry for telling you this late lel. No regrets.", null);
				break;
			}
			case "_SignPost LaserRoom": {
				Canvas_Renderer.script.DisplayInfo(readPosts.Count == 6 ? "Good job, so far you found every single post, I think you deserve a reward." :
																		  "This is the seventh sign, but you found only " + readPosts.Count + " disappointed.", null);
				if (readPosts.Count == 6) {
					Coin.coinsCollected++;
				}
				break;
			}
			case "_SignPost TeleportationRoom": {
				if (M_Player.player.pMovement.getCurrentMovementModifier == Player_Movement.PlayerMovementModifiers.INVERT) {
					Canvas_Renderer.script.DisplayInfo("The lightning in this room is very unstable, the path can disappear at any moment, and you do not want to misstep!", null);
				}
				else {
					Canvas_Renderer.script.DisplayInfo("!petssim ot tnaw ton od uoy dna ,tnemom yna ta raeppasid nac htap eht ,elbatsnu yrev si moor siht ni gninthgil ehT", null);
				}
				break;
			}
			case "_SignPost InvertingRoom": {
				Canvas_Renderer.script.DisplayInfo("!desruc si moor sihT", null);
				break;
			}
		}
		StartCoroutine(Fade());
		SoundFXHandler.script.PlayFX(interactionSound);
		readPosts.Add(this);
	}

	public void MapStanceSwitch(MapData.MapMode mode) {
		switch (mode) {
			case MapData.MapMode.LIGHT: {
				GetComponent<SpriteRenderer>().sprite = signPostNormal;
				return;
			}
			case MapData.MapMode.DARK: {
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
