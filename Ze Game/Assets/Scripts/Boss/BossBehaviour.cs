using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Igor.Constants.Strings;

public class BossBehaviour : MonoBehaviour {

	#region Outside References
	public Sprite Invincible;
	public Sprite Damageable;

	public GameObject cageObj;
	public GameObject Brimstone;

	public SpriteRenderer selfRender;
	private Animator selfAnim;
	public Rigidbody2D selfRigid;
	public RectTransform spikeHitbox;

	private GameObject player;
	private RectTransform BG;
	private PhysicsMaterial2D bouncyMaterial;

	#endregion

	#region Inside References


	private RectTransform topBrim;
	private RectTransform rightBrim;
	private RectTransform bottomBrim;
	private RectTransform leftBrim;

	private ObjectPool pool_EnemyProjectile;
	private ObjectPool pool_KillerBlock;

	private int changes = 0;
	private int bounces = 0;

	private List<GameObject> bullets = new List<GameObject>();

	private float cageSize;

	private float zRotation = 0;
	private float rotationDelta = 0.1f;

	private bool doneBouncing = false;

	private Vector3 attack1StartPos;
	private Vector3 attack2StartPos;
	private Vector3 attack3StartPos;
	private Vector3 attack4StartPos;
	private Vector3 attack5StartPos;

	private Coroutine currentAttack;

	private bool Attack1 = false;
	private bool Attack2 = false;
	private bool Attack3 = false;
	private bool Attack4 = false;
	private bool Attack5 = false;


	private bool informOnce = true;

	public BoxCollider2D[] spikeHitboxes = new BoxCollider2D[4];

	private static float playerSpeedMultiplier = 5;

	public delegate void BossBehavior(BossBehaviour sender);
	public static event BossBehavior OnBossfightBegin;

	private int attackNo;
	private int totalCircles;

	private BossHealth health;
	#endregion


	void Start() {
		if (OnBossfightBegin != null) {
			OnBossfightBegin(this);
		}
		playerSpeedMultiplier = 5;
		BG = MapData.script.GetBackgroundBoss(1);
		player = M_Player.player.gameObject;
		pool_EnemyProjectile = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_INACCUARATE) as GameObject);
		pool_KillerBlock = new ObjectPool(Resources.Load(PrefabNames.ENEMY_KILLERBLOCK_BOSS) as GameObject);
		selfAnim = GetComponent<Animator>();
		selfRigid = GetComponent<Rigidbody2D>();
		health = spikeHitbox.GetComponent<BossHealth>();
		attack1StartPos = BG.position;
		attack2StartPos = new Vector3(-530, -70, 1);
		attack3StartPos = BG.transform.position - new Vector3(0, BG.sizeDelta.y / 3);
		attack4StartPos = BG.position;
		attack5StartPos = new Vector3(BG.position.x + BG.sizeDelta.x / 4, BG.position.y + BG.sizeDelta.y / 2);
		bouncyMaterial = selfRigid.sharedMaterial;
		selfRigid.sharedMaterial = null;
		totalCircles = 2;

		StartCoroutine(InitialAttack());
	}

	private IEnumerator InitialAttack() {
		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = false;
		}
		selfRender.sprite = Invincible;

		yield return new WaitUntil(() => CameraMovement.script.isCamereDoneMoving);

		Camera.main.transform.position = BG.transform.position + new Vector3(0, 0, -10);
		Player_Movement.canMove = true;
		Zoom.canZoom = true;
		Canvas_Renderer.script.DisplayInfo("Ahh I see, you are persistent.. but you won't escape this time!\n The system is fully under my contol. You stande NO chance!", "Red = Invincible, Blue = Damageable. Aim for the things that extend from his body.");
		yield return new WaitForSeconds(1);

		//StartCoroutine(Attacks(ChooseAttack()));
		//1 bounce, 2 caged, 3 killerblocks, 4 laser, 5 flappy
		int debugAttack = 5;
		Debug.Log("DEBUG MODE first attack " + debugAttack);
		StartCoroutine(Attacks(debugAttack));
	}

	public IEnumerator InterPhase() {
		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = true;
		}
		selfRender.sprite = Damageable;
		health.CheckShields();
		int choice = ChooseAttack();
		yield return new WaitForSeconds(5);
		if (gameObject == null) {
			StopAllCoroutines();
			yield break;
		}
		currentAttack = StartCoroutine(Attacks(choice));

		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = false;
		}
	}

	//Attack Selector
	public int ChooseAttack() {
		int previous = attackNo;

		while (previous == attackNo) {
			attackNo = Random.Range(1, 6);
		}
		return attackNo;
	}
	//

	//Attack handler
	public IEnumerator Attacks(int attack) {
		selfRender.sprite = Invincible;
		StartCoroutine(LerpFunctions.LerpPosition(gameObject, GetStartingPosition(attack), Time.deltaTime / 2));

		switch (attack) {
			//Bouncing Attack
			case 1: {
				selfAnim.enabled = false;
				selfRigid.isKinematic = false;
				selfRigid.sharedMaterial = bouncyMaterial;
				yield return new WaitForSeconds(3);
				selfRender.GetComponent<CircleCollider2D>().isTrigger = true;


				//Actual Attack --
				Attack1 = true;
				selfRigid.velocity = Random.insideUnitCircle;
				while (selfRigid.velocity.magnitude < 250) {
					selfRigid.velocity += selfRigid.velocity.normalized * 5;
					yield return null;
				}

				yield return new WaitUntil(() => bounces >= 20);
				selfRigid.drag = 3;
				yield return new WaitUntil(() => selfRigid.velocity == Vector2.zero);
				selfRigid.drag = 0;
				bounces = 0;
				//--//

				selfAnim.SetTrigger("Attack" + attack);
				Attack1 = false;
				selfAnim.enabled = true;
				selfRigid.isKinematic = true;
				selfRigid.sharedMaterial = null;
				selfRender.GetComponent<CircleCollider2D>().isTrigger = false;
				break;
			}

			//Caged Attack
			case 2: {
				Attack2 = true;
				MoveScript positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity).GetComponent<MoveScript>();

				yield return new WaitForSeconds(2);

				//Actual Attack
				playerSpeedMultiplier = 1;

				StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, BG.transform.position, Time.deltaTime / 2));
				StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, BG.transform.position, Time.deltaTime / 2));
				positioningCage.destroy = true;
				yield return new WaitForSeconds(3);
				Canvas_Renderer.script.DisplayInfo(null, "Don't forget about the zooming feature :]");

				StartCoroutine(Caged(positioningCage.gameObject, 1.1f));
				for (int i = 0; i <= totalCircles; i++) {
					Debug.Log("Preforming " + (i + 1) + ". circle.");

					selfAnim.Play("Attack" + attack);

					yield return new WaitForSeconds(15f);
				}
				//--//

				Attack2 = false;

				yield return new WaitForSeconds(2f);
				playerSpeedMultiplier = 5;
				Destroy(positioningCage.gameObject);
				ClearBullets();
				break;
			}

			//Avoid KillerBlocks
			case 3: {
				MoveScript positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity).GetComponent<MoveScript>();

				yield return new WaitForSeconds(2);

				StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, gameObject.transform.position + new Vector3(0, 50, 0), Time.deltaTime / 2));
				StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, gameObject.transform.position + new Vector3(0, 50, 0), Time.deltaTime / 2));
				positioningCage.destroy = true;
				yield return new WaitForSeconds(2);

				//Actual Attack
				Attack3 = true;
				StartCoroutine(KillerBlockPath(positioningCage.GetComponent<MoveScript>()));

				while (Attack3) {

					MoveScript BlockL = pool_KillerBlock.getNext.GetComponent<MoveScript>();
					BlockL.gameObject.SetActive(true);
					BlockL.transform.position = new Vector3(transform.position.x - spikeHitbox.sizeDelta.x / 2, transform.position.y, 1);
					BlockL.transform.localScale = new Vector3(3, 3, 1);
					BlockL.Move();

					MoveScript BlockR = pool_KillerBlock.getNext.GetComponent<MoveScript>();
					BlockR.gameObject.SetActive(true);
					BlockR.transform.position = new Vector3(transform.position.x + spikeHitbox.sizeDelta.x / 2, transform.position.y, 1);
					BlockR.transform.localScale = new Vector3(3, 3, 1);
					BlockR.Move();

					yield return new WaitForSeconds(0.2f);

					if (changes >= 10) {
						selfAnim.SetTrigger("Reset");
						changes = 0;
						break;
					}
				}

				yield return new WaitForSeconds(1);

				Attack3 = false;
				selfRigid.velocity = Vector3.zero;
				break;
			}

			//Brimstone like Attack
			case 4: {

				selfAnim.Play("Attack" + attack);
				yield return new WaitForSeconds(2f);
				gameObject.GetComponent<ParticleSystem>().Emit(100);
				yield return new WaitForSeconds(2f);
				Attack4 = true;
				topBrim = Instantiate(Brimstone, transform).GetComponent<RectTransform>();
				topBrim.transform.localPosition = Vector3.zero;
				topBrim.name = "Top";

				rightBrim = Instantiate(Brimstone, transform).GetComponent<RectTransform>();
				rightBrim.transform.localPosition = Vector3.zero;
				rightBrim.name = "Right";

				bottomBrim = Instantiate(Brimstone, transform).GetComponent<RectTransform>();
				bottomBrim.transform.localPosition = Vector3.zero;
				bottomBrim.name = "Bottom";

				leftBrim = Instantiate(Brimstone, transform).GetComponent<RectTransform>();
				leftBrim.transform.localPosition = Vector3.zero;
				leftBrim.name = "Left";

				StartCoroutine(VariedRotation());

				yield return new WaitForSeconds(35);
				//--//

				topBrim.gameObject.SetActive(false);
				rightBrim.gameObject.SetActive(false);
				bottomBrim.gameObject.SetActive(false);
				leftBrim.gameObject.SetActive(false);
				transform.rotation = Quaternion.identity;
				Attack4 = false;

				StopCoroutine(VariedRotation());
				break;
			}

			//Flappybird like Attack
			case 5: {
				MoveScript positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity).GetComponent<MoveScript>();
				Attack5 = true;
				yield return new WaitForSeconds(2);
				if (informOnce) {
					informOnce = false;
					Canvas_Renderer.script.DisplayInfo("Flappy Bird!!! (Press \"UpArrow\" or \"W\") to flap. ", "Press \"Up or W\" to flap.");
				}
				StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, (Vector2)BG.transform.position - BG.sizeDelta / 2 + new Vector2(40, 20), Time.deltaTime / 2));
				StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, (Vector2)BG.transform.position - BG.sizeDelta / 2 + new Vector2(40, 20), Time.deltaTime / 2));
				positioningCage.destroy = true;

				yield return new WaitForSeconds(2);
				Destroy(positioningCage.gameObject);
				M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.FLAPPY);
				StartCoroutine(PipeGeneration());

				yield return new WaitUntil(() => doneBouncing);

				M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.ARROW);
				Attack5 = false;
				doneBouncing = false;
				StartCoroutine(LerpFunctions.LerpPosition(gameObject, BG.transform.position + new Vector3(BG.sizeDelta.x / 2 - 140, 0, 0), Time.deltaTime / 2));
				break;
			}
		}
		StartCoroutine(InterPhase());
	}

	//Caged Attack Code
	public IEnumerator Caged(GameObject cage, float waitTime) {

		while (Attack2) {
			cageSize = cage.GetComponent<RectTransform>().sizeDelta.x / 2;
			Vector3 target = GetPosInCage(cage);
			print("New wait time: " + waitTime);
			yield return new WaitForSeconds(waitTime);
			Projectile bullet = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, target - gameObject.transform.position);
			bullet.transform.position = gameObject.transform.position;
			bullets.Add(bullet.gameObject);
			bullet.gameObject.SetActive(true);
			bullet.projectileSpeed = 15;
			bullet.Fire(1);
			waitTime -= waitTime * 0.005f;
		}
	}

	public Vector3 GetPosInCage(GameObject positioningCage) {
		float x = Random.Range(positioningCage.transform.position.x - cageSize, positioningCage.transform.position.x + cageSize);
		float y = Random.Range(positioningCage.transform.position.y - cageSize, positioningCage.transform.position.y + cageSize);
		return new Vector3(x, y, 1);
	}

	public void ClearBullets() {
		foreach (GameObject bullet in bullets) {
			bullet.SetActive(false);
		}
	}
	//


	//Dodge KillerBlocks Attack Code
	public IEnumerator KillerBlockPath(MoveScript positioningCage) {
		Directions current = Directions.RIGHT;
		yield return new WaitForSeconds(1);
		selfAnim.Play("SpeedUp");
		positioningCage.Move();

		while (Attack3) {
			LayerMask mask = LayerMask.GetMask(Layers.WALLS);
			RaycastHit2D right = Physics2D.Raycast(transform.position, Vector3.right, BG.sizeDelta.x, mask.value);
			RaycastHit2D left = Physics2D.Raycast(transform.position, Vector3.left, BG.sizeDelta.x, mask.value);

			float distR = Vector3.Distance(transform.position, new Vector3(right.point.x - spikeHitbox.sizeDelta.x / 2, transform.position.y));
			float distL = Vector3.Distance(transform.position, new Vector3(left.point.x + spikeHitbox.sizeDelta.x / 2, transform.position.y));

			yield return new WaitUntil(() => Mathf.Abs(selfAnim.GetFloat("Speed")) < 5);
			AnimatorStateInfo state = selfAnim.GetCurrentAnimatorStateInfo(0);
			if (state.IsName("ChangeDirToLeft")) {
				current = Directions.LEFT;
			}
			else if (state.IsName("ChangeDirToRight")) {
				current = Directions.RIGHT;
			}
			yield return new WaitForSeconds(0.5f);

			float arriveTime = current == Directions.RIGHT ? distR / 40 : distL / 40;
			yield return new WaitForSeconds(Random.Range(1, arriveTime));
			selfAnim.SetTrigger(current == Directions.RIGHT ? "Left" : "Right");
			changes++;
		}
	}
	//


	//Brimstone like Attack
	public IEnumerator VariedRotation() {
		rotationDelta = 0.1f;
		zRotation = 0;
		//this is why the attack is broken!!! - but keep is on harder difficulties because it is fun lel
		while (true) {
			yield return new WaitForSeconds(Random.Range(2, 4));
			rotationDelta = Chance.Half() ? Random.Range(0.4f, 1f) : Random.Range(-1, -0.4f);
			print(rotationDelta);
		}
	}
	//


	//Flappy bird like Attack Code
	public IEnumerator PipeGeneration() {

		const float pipeSpacing = 2f;
		float horizontalDistance = Mathf.Abs(Mathf.Abs(player.transform.position.x) - Mathf.Abs(transform.position.x));
		float arriveTime = 10f;
		bool downwardsMovement = true;
		int totalWallsGenerated = 9;
		Vector3 oppositePosition = new Vector3(BG.position.x + (BG.sizeDelta.x / 4), BG.position.y - (BG.sizeDelta.y / 2));

		for (int i = 0; i < totalWallsGenerated; i++) {
			yield return new WaitForSeconds(pipeSpacing);
			StartCoroutine(LerpFunctions.LerpPosition(gameObject,
													  new Vector3(BG.position.x + (BG.sizeDelta.x / 4),
													  downwardsMovement ? BG.position.y - (BG.sizeDelta.y / 2) : BG.position.y + (BG.sizeDelta.y / 2)),
													  Time.deltaTime));
			downwardsMovement = !downwardsMovement;

			float holeMid = Random.Range(-BG.sizeDelta.y / 2 + 20, BG.sizeDelta.y / 2 - 20);

			float timeAtStart = Time.timeSinceLevelLoad;
			while (transform.position != attack5StartPos && transform.position != oppositePosition) {

				float change = arriveTime - (Time.timeSinceLevelLoad - timeAtStart);

				if ((transform.position.y > holeMid + 15 || transform.position.y < holeMid - 15)) {
					Projectile shot = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
					shot.transform.position = transform.position;
					shot.transform.rotation = Quaternion.Euler(0, 0, 270);
					shot.gameObject.SetActive(true);
					shot.projectileSpeed = horizontalDistance / change;
					shot.Fire();
					shot.StartCoroutine(shot.Deactivate(arriveTime + 2));
				}
				yield return new WaitForSeconds(0.02f);
			}
		}
		yield return new WaitForSeconds(arriveTime);
		doneBouncing = true;
	}
	//

	////Bouce off walls
	private void OnCollisionEnter2D(Collision2D col) {
		if (Attack1) {
			bounces++;
		}
	}

	private void FixedUpdate() {
		if (Attack3) {
			selfRigid.velocity = new Vector2(1, 0) * selfAnim.GetFloat("Speed");
		}
		if (Attack4 == true) {
			zRotation += rotationDelta;
			transform.rotation = Quaternion.Euler(0, 0, zRotation);

			LayerMask mask = LayerMask.GetMask(Layers.WALLS);

			RaycastHit2D up = Physics2D.Raycast(transform.position, transform.rotation * Vector3.up, BG.sizeDelta.x * 2, mask.value);
			RaycastHit2D right = Physics2D.Raycast(transform.position, transform.rotation * Vector3.right, BG.sizeDelta.x * 2, mask.value);
			RaycastHit2D down = Physics2D.Raycast(transform.position, transform.rotation * Vector3.down, BG.sizeDelta.x * 2, mask.value);
			RaycastHit2D left = Physics2D.Raycast(transform.position, transform.rotation * Vector3.left, BG.sizeDelta.x * 2, mask.value);

			print(up.transform.name);

			float distance;

			distance = Vector3.Distance(transform.position, up.point);
			topBrim.position = ((Vector2)transform.position + up.point) * 0.5f;
			topBrim.localScale = new Vector3(1, distance / topBrim.sizeDelta.y, 1);
			topBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)up.point - new Vector3(transform.position.x, transform.position.y, 0)));

			distance = Vector3.Distance(transform.position, right.point);
			rightBrim.position = ((Vector2)transform.position + right.point) * 0.5f;
			rightBrim.localScale = new Vector3(1, distance / rightBrim.sizeDelta.y, 1);
			rightBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)right.point - new Vector3(transform.position.x, transform.position.y, 0)));

			distance = Vector3.Distance(transform.position, down.point);
			bottomBrim.position = ((Vector2)transform.position + down.point) * 0.5f;
			bottomBrim.localScale = new Vector3(1, distance / bottomBrim.sizeDelta.y, 1);
			bottomBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)down.point - new Vector3(transform.position.x, transform.position.y, 0)));

			distance = Vector3.Distance(transform.position, left.point);
			leftBrim.position = ((Vector2)transform.position + left.point) * 0.5f;
			leftBrim.localScale = new Vector3(1, distance / leftBrim.sizeDelta.y, 1);
			leftBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)left.point - new Vector3(transform.position.x, transform.position.y, 0)));
		}
	}

	private Vector3 GetStartingPosition(int selectedAttack) {
		switch (selectedAttack) {
			case 1: {
				return attack1StartPos;
			}
			case 2: {
				return attack2StartPos;
			}
			case 3: {
				return attack3StartPos;
			}
			case 4: {
				return attack4StartPos;
			}
			case 5: {
				return attack5StartPos;
			}
			default: {
				throw new System.Exception("No attack type " + selectedAttack + " implemented!");
			}
		}
	}

	public static float getPlayerSpeedMultiplier {
		get { return playerSpeedMultiplier; }
		private set { playerSpeedMultiplier = value; }
	}
}