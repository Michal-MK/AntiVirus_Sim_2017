using UnityEngine;

public class LeverRelay : MonoBehaviour {

	[SerializeField]
	private GameObject room10AvoidancePrefab = null;
	[SerializeField]
	private Light redParticle = null;
	[SerializeField]
	private Light greenParticle = null;
	[SerializeField]
	private Lever[] levers = null;
	/// <summary>
	/// All levers connected with this relay
	/// </summary>
	public Lever[] Levers => levers;

	public int LeversActivated { get; private set; } = 0;

	private GameObject instantiatedAvoidance;
	private ParticleSystem[] emmiters;

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
				instantiatedAvoidance = Instantiate(room10AvoidancePrefab, MapData.Instance.GetRoom(10).Background.position, Quaternion.identity);
				instantiatedAvoidance.transform.GetChild(0).GetComponent<TurretAttack>().target = Player.Instance.gameObject;
				sender.OnLeverSwitch -= Hazards;
				Player.OnRoomEnter += RemoveInstantiatedAvoidance;
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

	private void RemoveInstantiatedAvoidance(Player sender, RectTransform background, RectTransform previous) {
		if (background == MapData.Instance.GetRoom(6).Background) {
			Destroy(instantiatedAvoidance);
			Player.OnRoomEnter -= RemoveInstantiatedAvoidance;
		}
	}

	private void LeverActivation(Lever sender, bool isOn) {
		if (isOn) {
			switch (++LeversActivated) {
				case 1: {
					MapData.Instance.SwitchMapMode(MapMode.DARK);
					ParticleSystem.LightsModule module = emmiters[0].lights;
					module.light = greenParticle;
					MusicHandler.script.TransitionMusic(MusicHandler.script.darkWorld);
					HUDisplay.Instance.DisplayInfo(null, "The computer went to standby mode, be careful.");
					break;
				}
				case 2: {
					Player.Instance.pMovement.SetMovementModifier(PlayerMovementModifiers.INVERT);
					ParticleSystem.LightsModule module = emmiters[1].lights;
					module.light = greenParticle;
					HUDisplay.Instance.DisplayInfo(".doog os leef t'nod I ,reayalp .rM ...gnineppah si tahW !yeH", "?thgirla gnihtyreve si ,egnarts si sihT");
					break;
				}
				case 3: {
					Player.Instance.pMovement.SetMovementModifier(PlayerMovementModifiers.INVERT);
					ParticleSystem.LightsModule module = emmiters[2].lights;
					module.light = greenParticle;
					foreach (Lever l in levers) {
						l.OnLeverSwitch -= LeverActivation;
					}
					MapData.Instance.GetRoomLink(6, 11).OpenDoor();
					break;
				}
			}
		}
		else {
			ParticleSystem.LightsModule module = emmiters[--LeversActivated].lights;
			module.light = redParticle;
		}
	}

	public void Interact() {
		HUDisplay.Instance.DisplayInfo("Upon inspection, I was able to figure out, that this device is not fully operational.", "New path opened");
		MapData.Instance.GetRoomLink(6, 7).OpenDoor();
		HUDisplay.Instance.DisplayDirection(Directions.TOP);
		CameraMovement.Instance.RaycastForRooms();
	}
}
