using UnityEngine;

public class LeverRelay : MonoBehaviour {

	public GameObject lever_8_Avoidance; // Prefab

	private GameObject instantiatedAvoidance;

	public Lever[] levers;
	private ParticleSystem[] emmiters;

	public Light redParticle;
	public Light greenParticle;

	private int leversActivated = 0;


	void Start() {
		foreach (Lever l in levers) {
			l.OnLeverSwitch += LeverActivation;
			l.OnLeverSwitch += Hazards;
		}
		emmiters = GetComponentsInChildren<ParticleSystem>();
	}

	private void Hazards(Lever sender, bool isOn) {
		switch (sender.name) {
			case "Lever_8": {
				instantiatedAvoidance = Instantiate(lever_8_Avoidance, MapData.script.GetRoom(8).background.position, Quaternion.identity);
				instantiatedAvoidance.transform.GetChild(0).GetComponent<TurretAttack>().target = M_Player.player.gameObject;
				sender.OnLeverSwitch -= Hazards;
				M_Player.OnRoomEnter += RemoveInstantiatedAvoidance;
				return;
			}
			case "Lever_9": {

				sender.OnLeverSwitch -= Hazards;
				return;
			}
			case "Lever_10": {

				sender.OnLeverSwitch -= Hazards;
				return;
			}
		}
	}

	private void RemoveInstantiatedAvoidance(M_Player sender, RectTransform background, RectTransform previous) {
		if(background == MapData.script.GetRoom(6).background) {
			Destroy(instantiatedAvoidance);
			M_Player.OnRoomEnter -= RemoveInstantiatedAvoidance;
		}
	}

	private void LeverActivation(Lever sender, bool isOn) {
		if (isOn) {
			leversActivated++;
			switch (leversActivated) {
				case 1: {
					MapData.script.SwitchMapMode(MapData.MapMode.DARK);
					ParticleSystem.LightsModule module = emmiters[0].lights;
					module.light = greenParticle;
					MusicHandler.script.TransitionMusic(MusicHandler.script.darkWorld);
					Canvas_Renderer.script.DisplayInfo(null, "The computer went to standby mode, be careful.");
					break;
				}
				case 2: {
					M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.INVERT);
					ParticleSystem.LightsModule module = emmiters[1].lights;
					module.light = greenParticle;
					Canvas_Renderer.script.DisplayInfo(".thgir leef t'nseod siht ...gnineppah si tahW !yeH","?thgirla gnihtyreve si ,egnarts leef I");
					break;
				}
				case 3: {
					M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.REVERT);
					ParticleSystem.LightsModule module = emmiters[2].lights;
					module.light = greenParticle;
					foreach (Lever l in levers) {
						l.OnLeverSwitch -= LeverActivation;
					}
					MapData.script.OpenDoor(MapData.script.GetRoomLink(6, 11));
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
		Canvas_Renderer.script.DisplayInfo("Upon inspection, I was able to figure out, that this device is not fully operational.", "New path opened");
		MapData.script.OpenDoor(MapData.script.GetRoomLink(6, 7));
		Canvas_Renderer.script.DisplayDirection(Directions.TOP);
		CameraMovement.script.RaycastForRooms();
	}

	public int currentlyActiveLevers {
		get { return leversActivated; }
	}
}
