using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Igor.Boss.Attacks;
using System;
using Random = UnityEngine.Random;

public class NewBossBehaviour : Enemy {

	#region Outside References
	public Sprite Invincible;
	public Sprite Damageable;

	public SpriteRenderer selfRender;
	public BoxCollider2D[] selfSpikeHitboxes = new BoxCollider2D[4];
	public BossHealth selfHealth;


	private GameObject player;
	private RectTransform arenaBackground;
	private PhysicsMaterial2D bouncyMaterial;

	protected List<IAttackPattern> attacks = new List<IAttackPattern>();
	public static event EventHandler<BossEncouterEventArgs> OnBossfightBegin;
	public static event EventHandler<BossfightResultEventArgs> OnBossfightResult;
	public static float getPlayerSpeedMultiplier { get; private set; } = 5;

	private int currentAttackNumber;

	private IAttackPattern currentAttack;
	#endregion


	void Start() {
		OnBossfightBegin?.Invoke(this, new BossEncouterEventArgs(this, M_Player.player));
		getPlayerSpeedMultiplier = 5;
		arenaBackground = MapData.script.GetBackgroundBoss(1);
		player = M_Player.player.gameObject;
		attacks.Add(new BounceInArena(gameObject, arenaBackground.position));
		attacks.Add(new KillerBlockPath(gameObject, arenaBackground.transform.position - new Vector3(0, arenaBackground.sizeDelta.y / 3), arenaBackground));
		attacks.Add(new FlappyBirdWalls(gameObject, Vector3.zero, arenaBackground));
		attacks.Add(new LoopBulletSpawn(gameObject, Vector3.zero, arenaBackground.position));

		StartCoroutine(InitialAttack());
	}

	private IEnumerator InitialAttack() {
		for (int i = 0; i < selfSpikeHitboxes.Length; i++) {
			selfSpikeHitboxes[i].enabled = false;
		}
		selfRender.sprite = Invincible;

		yield return new WaitUntil(() => CameraMovement.script.isCameraDoneMoving);

		Camera.main.transform.position = arenaBackground.transform.position + new Vector3(0, 0, -10);
		Player_Movement.canMove = true;
		Zoom.canZoom = true;
		Canvas_Renderer.script.DisplayInfo("Ahh I see, you are persistent.. but you won't escape this time!\n The system is fully under my contol. You stand NO chance!", "Red = Invincible, Blue = Damageable. Aim for the things that extend from its body.");
		yield return new WaitForSeconds(1);

		StartCoroutine(Attacks(ChooseAttack()));
	}

	public IEnumerator InterPhase() {
		for (int i = 0; i < selfSpikeHitboxes.Length; i++) {
			selfSpikeHitboxes[i].enabled = true;
		}
		selfRender.sprite = Damageable;
		int choice = ChooseAttack();
		yield return new WaitForSeconds(5);
		if (gameObject == null) {
			StopAllCoroutines();
			yield break;
		}
		StartCoroutine(Attacks(choice));

		for (int i = 0; i < selfSpikeHitboxes.Length; i++) {
			selfSpikeHitboxes[i].enabled = false;
		}
	}

	public int ChooseAttack() {
		int previous = currentAttackNumber;

		while (previous == currentAttackNumber) {
			currentAttackNumber = Random.Range(0, attacks.Count);
		}
		return currentAttackNumber;
	}

	public IEnumerator Attacks(int attack) {
		selfRender.sprite = Invincible;

		//TODO Preform attack
		yield return null;
		StartCoroutine(LerpFunctions.LerpPosition(gameObject, GetStartingPosition(attack), Time.deltaTime / 2, null));

		StartCoroutine(InterPhase());
	}

	private void FixedUpdate() {
		currentAttack.Update();
	}

	private Vector3 GetStartingPosition(int selectedAttack) {
		return attacks[selectedAttack].startPosition;
	}
}