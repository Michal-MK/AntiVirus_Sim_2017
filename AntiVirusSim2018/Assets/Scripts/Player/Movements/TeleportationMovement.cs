using Igor.Constants.Strings;
using System;
using System.Collections;
using UnityEngine;
using static Player_Movement;

public class TeleportationMovement : MonoBehaviour, IPlayerMovement {
	public PlayerMovement movementType => PlayerMovement.TELEPORT;

	public PlayerMovementModifiers movementModifier { get; set; } = PlayerMovementModifiers.NONE;

	public AudioClip FX_Teleport;
	public Rigidbody2D body;

	private SpriteRenderer[] tpNodes;
	private Sprite ready;
	private Sprite idle;


	private bool isEnabled = false;

	public void Setup(Rigidbody2D body) {
		this.body = body;
		tpNodes = transform.Find("_Teleportation").GetComponentsInChildren<SpriteRenderer>();
		ready = tpNodes[0].sprite;
		idle = tpNodes[1].sprite;
	}

	public void Move() {
		if (!isEnabled) {
			isEnabled = true;
			StartCoroutine(Teleportation());
		}
	}

	private IEnumerator Teleportation() {
		tpNodes[0].sprite = idle;
		tpNodes[0].transform.parent.gameObject.SetActive(true);
		while (isEnabled) {
			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) > 0.5f || Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) > 0.5f);

			Directions choice = Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL) != 0 ? DetermineDirection(true, Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) : DetermineDirection(false, Input.GetAxis(InputNames.MOVEMENT_VERTICAL));

			tpNodes[(int)choice].sprite = ready;
			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) < 0.5f && Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) < 0.5f);

			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) > 0.5f || Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) > 0.5f);
			Directions secondaryChoice = Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL) != 0 ? DetermineDirection(true, Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) : DetermineDirection(false, Input.GetAxis(InputNames.MOVEMENT_VERTICAL));

			if (choice == secondaryChoice) {
				//TODO: Do something prettier
				SoundFXHandler.script.PlayFX(FX_Teleport);
				transform.position = tpNodes[(int)secondaryChoice].transform.position;
			}
			tpNodes[(int)choice].sprite = idle;
			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) < 0.5f && Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) < 0.5f);
		}
		tpNodes[0].sprite = ready;
		tpNodes[1].sprite = idle;
		transform.Find("_Teleportation").gameObject.SetActive(false);
	}


	private Directions DetermineDirection(bool isHorizontal, float value) {
		if (isHorizontal) {
			if (movementModifier != PlayerMovementModifiers.INVERT) {
				return value > 0 ? Directions.RIGHT : Directions.LEFT;
			}
			else {
				return value > 0 ? Directions.LEFT : Directions.RIGHT;
			}
		}
		else {
			if (movementModifier != PlayerMovementModifiers.INVERT) {
				return value > 0 ? Directions.TOP : Directions.BOTTOM;
			}
			else {
				return value > 0 ? Directions.BOTTOM : Directions.TOP;
			}
		}
	}

	public void Stop() {
		isEnabled = false;
		StopCoroutine(Teleportation());
		tpNodes[0].sprite = ready;
		tpNodes[1].sprite = idle;
		transform.Find("_Teleportation").gameObject.SetActive(false);
	}
}


