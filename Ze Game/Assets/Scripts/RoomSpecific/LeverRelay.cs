using UnityEngine;
using System.Collections;

public class LeverRelay : MonoBehaviour {
	public Lever[] levers;
	private ParticleSystem[] emmiters;

	public Light redParticle;
	public Light greenParticle;

	private int leversActivated = 0;


	void Start() {
		foreach (Lever l in levers) {
			l.OnLeverSwitch += L_OnLeverSwitch;
		}
		emmiters = GetComponentsInChildren<ParticleSystem>();
	}

	private void L_OnLeverSwitch(Lever sender, bool isOn) {
		if (isOn) {
			leversActivated++;
			switch (leversActivated) {
				case 1: {
					MapData.script.SwitchMapMode(MapData.MapMode.DARK);
					ParticleSystem.LightsModule module = emmiters[0].lights;
					module.light = greenParticle;
					break;
				}
				case 2: {
					M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.INVERT);
					ParticleSystem.LightsModule module = emmiters[1].lights;
					module.light = greenParticle;
					break;
				}
				case 3: {
					M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.REVERT);
					ParticleSystem.LightsModule module = emmiters[2].lights;
					module.light = greenParticle;
					foreach (Lever l in levers) {
						l.OnLeverSwitch -= L_OnLeverSwitch;
					}
					MapData.script.OpenDoor(new RoomLink(6, 10));
					break;
				}
			}
		}
		else {
			ParticleSystem.LightsModule module = emmiters[leversActivated - 1].lights;
			module.light = redParticle;
			leversActivated--;
		}
	}

	public void Interact() {
		Canvas_Renderer.script.DisplayInfo("Upon inspection, I was able to figure out, that this device is not fully operational.", "New paths were opened");
		MapData.script.OpenDoor(new RoomLink(6, 7));
		MapData.script.OpenDoor(new RoomLink(6,"8a"));
		CameraMovement.script.RaycastForRooms();
	}
}
