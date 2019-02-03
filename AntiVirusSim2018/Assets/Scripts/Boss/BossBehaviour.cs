using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Igor.Boss.Attacks;
using System;
using Random = UnityEngine.Random;

public class BossBehaviour : Enemy {

	#region Outside References
	public Sprite Invincible;
	public Sprite Damageable;

	public SpriteRenderer selfRender;
	public BossHealth selfHealth;


	private GameObject player;

	protected List<IAttackPattern> attacks = new List<IAttackPattern>();
	public static event EventHandler<BossEncouterEventArgs> OnBossfightBegin;
	public static event EventHandler<BossfightResultEventArgs> OnBossfightResult;

	private int currentAttackNumber;

	private IAttackPattern currentAttack;
	#endregion


	void Start() {
		OnBossfightBegin?.Invoke(this, new BossEncouterEventArgs(this, M_Player.player));
		RectTransform arenaBackground = MapData.script.GetBackgroundBoss(1);
		player = M_Player.player.gameObject;
		attacks.Add(new LoopBulletSpawn(gameObject, SpriteOffsets.GetPoint(arenaBackground.GetComponent<SpriteRenderer>(), 12, 88), arenaBackground.position, arenaBackground));
		attacks.Add(new KillerBlockPath(gameObject, arenaBackground.transform.position - new Vector3(0, arenaBackground.sizeDelta.y / 3), arenaBackground));
		attacks.Add(new FlappyBirdWalls(gameObject, new Vector3(arenaBackground.position.x + arenaBackground.sizeDelta.x / 4, arenaBackground.position.y + arenaBackground.sizeDelta.y / 2), arenaBackground));
		attacks.Add(new LaserSpin(gameObject, arenaBackground.position, arenaBackground));

		StartCoroutine(InitialAttack());
	}

	private IEnumerator InitialAttack() {
		selfHealth.SetDamageable(false);
		selfRender.sprite = Invincible;

		yield return new WaitUntil(() => CameraMovement.script.isCameraDoneMoving);

		Player_Movement.canMove = true;
		Zoom.canZoom = true;
		Canvas_Renderer.script.DisplayInfo("Ahh I see, you are persistent.. but you won't escape this time!\n The system is fully under my control. You stand NO chance!", "Red = Invincible, Blue = Damageable. Aim for the things that extend from its body.");
		yield return new WaitForSeconds(1);

		StartCoroutine(Attack(Random.Range(0, attacks.Count)));
	}

	public IEnumerator InterPhase() {
		selfHealth.SetDamageable(true);

		selfRender.sprite = Damageable;
		int choice = ChooseAttack();
		yield return new WaitForSeconds(5);

		StartCoroutine(Attack(choice));

		selfHealth.SetDamageable(false);
	}

	public int ChooseAttack() {
		int previous = currentAttackNumber;

		while (previous == currentAttackNumber) {
			currentAttackNumber = Random.Range(0, attacks.Count);
		}
		return currentAttackNumber;
	}

	public IEnumerator Attack(int attack) {
		selfRender.sprite = Invincible;
		currentAttack = attacks[attack];
		yield return LerpFunctions.LerpPosition(gameObject, currentAttack.startPosition, Time.deltaTime / 2, null);
		yield return currentAttack.Attack();
		StartCoroutine(InterPhase());
	}

	private void FixedUpdate() {
		if (currentAttack != null && currentAttack.isAttackInProgress) {
			currentAttack.Update();
		}
	}
}