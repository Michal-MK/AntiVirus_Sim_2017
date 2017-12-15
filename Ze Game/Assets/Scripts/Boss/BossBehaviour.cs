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
	public Rigidbody2D rigid;
	public RectTransform spikeHitbox;

	private GameObject player;
	private RectTransform BG;

	#endregion

	#region Inside References

	private Animator anim;

	private GameObject positioningCage;
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


	public bool bossSpawned = false;
	private bool initialDealy = true;
	private bool doneBouncing = false;

	private Vector3 attack1StartPos;
	private Vector3 attack2StartPos;
	private Vector3 attack3StartPos;
	private Vector3 attack4StartPos;
	private Vector3 attack5StartPos;

	private Coroutine currentAttack;

	private bool headingRight = true;
	private bool preformChange = false;
	private bool dontChangeR = false;
	private bool dontChangeL = false;

	private bool Attack1 = false;
	private bool Attack2 = false;
	private bool Attack3 = false;
	private bool Attack4 = false;
	private bool Attack5 = false;

	private float distR = 0;
	private float distL = 0;
	private float changeThreshold = 45;

	private bool informOnce = true;

	public BoxCollider2D[] spikeHitboxes = new BoxCollider2D[4];

	public bool donePositioning = true;

	public static float playerSpeedMultiplier = 5;

	private LerpFunctions lerps = new LerpFunctions();

	public delegate void BossBehavior(BossBehaviour sender);
	public static event BossBehavior OnBossfightBegin;

	private int attackNo;
	private int totalCircles;

	private Vector2 calculatedVec;

	#endregion


	void Start() {
		if (OnBossfightBegin != null) {
			OnBossfightBegin(this);
		}
		playerSpeedMultiplier = 5;

		BG = GameObject.Find(BackgroundNames.BACKGROUND_BOSS_ + "1").GetComponent<RectTransform>();
		player = GameObject.FindGameObjectWithTag("Player");
		pool_EnemyProjectile = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_INACCUARATE) as GameObject);
		pool_KillerBlock = new ObjectPool(Resources.Load(PrefabNames.ENEMY_KILLERBLOCK) as GameObject);
		anim = GetComponent<Animator>();
		rigid = gameObject.GetComponent<Rigidbody2D>();
		rigid.freezeRotation = true;

		attack1StartPos = BG.position + new Vector3(-BG.sizeDelta.x / 2 + 40, -BG.sizeDelta.y / 2 + 40);
		attack2StartPos = new Vector3(-530, -70, 1);
		attack3StartPos = new Vector3(-368, -70, 1);
		attack4StartPos = BG.position;
		attack5StartPos = (Vector2)BG.transform.position + BG.sizeDelta / 2 + new Vector2(-10, 0);

		totalCircles = 2;

		StartCoroutine(InitialAttack());
	}

	private IEnumerator InitialAttack() {

		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = false;
		}
		selfRender.sprite = Invincible;


		yield return new WaitUntil(() => CameraMovement.doneMoving);

		bossSpawned = true;

		Camera.main.transform.position = BG.transform.position + new Vector3(0, 0, -10);

		print(Camera.main.transform.position);

		Canvas_Renderer.script.InfoRenderer("Ahh I see, you are persistent.. but you won't escape this time!\n The system is fully under my contol. You stande NO chance!", "Red = Invincible, Blue = Damageable. Aim for the things that extend from his body.");
		yield return new WaitForSeconds(1);
		//StartCoroutine(Attacks(ChooseAttack()));

		StartCoroutine(Attacks(2));

	}

	public IEnumerator InterPhase() {
		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = true;
		}
		selfRender.sprite = Damageable;
		spikeHitbox.GetComponent<BossHealth>().CheckShields();
		int choice = ChooseAttack();
		yield return new WaitForSeconds(5);
		currentAttack = StartCoroutine(Attacks(choice));

		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = false;
		}
	}

	//Attack Selector
	public int ChooseAttack() {
		int previous = attackNo;

		while (previous == attackNo) {
			attackNo = UnityEngine.Random.Range(2, 6);
		}
		return attackNo;
	}
	//

	//Attack handler
	public IEnumerator Attacks(int attack) {
		selfRender.sprite = Invincible;
		StartCoroutine(lerps.LerpPosition(gameObject, transform.position, GetStartingPosition(attack), Time.deltaTime / 2));

		switch (attack) {
			//Bouncing Attack
			case 1: {
				anim.enabled = false;
				rigid.isKinematic = false;
				yield return new WaitForSeconds(3);
				selfRender.GetComponent<CircleCollider2D>().isTrigger = true;


				//Actual Attack --
				Attack1 = true;
				for (int i = 0; i < 200; i += 2) {
					//print(i);
					rigid.velocity = new Vector2(1, 1) * i;
					yield return null;
				}
				rigid.velocity = new Vector2(200, 200);

				yield return new WaitUntil(() => bounces >= 20);
				rigid.drag = 1;
				yield return new WaitForSeconds(2);
				rigid.drag = 3;
				yield return new WaitUntil(() => rigid.velocity == Vector2.zero);
				rigid.drag = 0;
				bounces = 0;
				//--//

				anim.SetTrigger("Attack" + attack);
				Attack1 = false;
				anim.enabled = true;
				rigid.isKinematic = true;
				selfRender.GetComponent<CircleCollider2D>().isTrigger = false;
				break;
			}

			//Caged Attack
			case 2: {
				Attack2 = true;
				positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
				positioningCage.name = "PositioningCage";

				yield return new WaitForSeconds(2);

				//Actual Attack
				playerSpeedMultiplier = 1;
				Projectile.projectileSpeed = 15;

				StartCoroutine(lerps.LerpPosition(positioningCage, positioningCage.transform.position, BG.transform.position, Time.deltaTime / 2));
				yield return new WaitForSeconds(3);
				Canvas_Renderer.script.InfoRenderer(null, "Don't forget about the zooming feature :]");

				StartCoroutine(Caged(1.1f));
				for (int i = 0; i <= totalCircles; i++) {
					Debug.Log("Preforming " + (i + 1) + ". circle.");

					anim.Play("Attack" + attack);

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
				positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
				positioningCage.name = "PositioningCage";

				yield return new WaitForSeconds(3);

				StartCoroutine(lerps.LerpPosition(positioningCage, positioningCage.transform.position, gameObject.transform.position + new Vector3(0, 50, 0), Time.deltaTime / 2));
				yield return new WaitForSeconds(2);

				Attack3 = true;
				anim.Play("SpeedUp");

				//Actual Attack
				StartCoroutine(ChangeDir());
				while (Attack3) {

					GameObject BlockL = pool_KillerBlock.getNext;
					BlockL.SetActive(true);
					GameObject BlockR = pool_KillerBlock.getNext;
					BlockR.SetActive(true);

					BlockL.transform.position = new Vector3(transform.position.x - spikeHitbox.sizeDelta.x / 2, transform.position.y, 1);
					BlockR.transform.position = new Vector3(transform.position.x + spikeHitbox.sizeDelta.x / 2, transform.position.y, 1);

					BlockL.transform.localScale = new Vector3(3, 3, 1);
					BlockR.transform.localScale = new Vector3(3, 3, 1);

					BlockL.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 30);
					BlockR.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 30);

					yield return new WaitForSeconds(0.2f);
					print(changes);
					if (changes >= 10) {
						anim.SetTrigger("Attack" + attack);
						anim.SetTrigger("Reset");
						changes = 0;
						break;
					}
				}
				yield return new WaitForSeconds(1);

				Attack3 = false;
				initialDealy = true;
				dontChangeL = false;
				dontChangeR = false;
				distL = 0;
				distR = 0;
				rigid.velocity = Vector3.zero;
				break;
			}

			//Brimstone like Attack
			case 4: {

				anim.Play("Attack" + attack);
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
				positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
				positioningCage.name = "PositioningCage";
				Attack5 = true;
				yield return new WaitForSeconds(2);
				if (informOnce) {
					informOnce = false;
					Canvas_Renderer.script.InfoRenderer("Flappy Bird!!! (Press \"UpArrow\" or \"W\") to flap. ", "Press \"Up or W\" to flap.");
				}
				StartCoroutine(lerps.LerpPosition(positioningCage,
					positioningCage.transform.position,
					(Vector2)BG.transform.position - BG.sizeDelta / 2 + new Vector2(40, 20),
					Time.deltaTime / 2));

				yield return new WaitForSeconds(2);
				Destroy(positioningCage.gameObject);
				player.GetComponent<Player_Movement>().SetMovementMode(Player_Movement.PlayerMovent.FLAPPY);
				StartCoroutine(PipeGeneration());

				yield return new WaitUntil(() => doneBouncing);

				player.GetComponent<Player_Movement>().SetMovementMode(Player_Movement.PlayerMovent.ARROW);
				Attack5 = false;
				doneBouncing = false;
				StartCoroutine(lerps.LerpPosition(gameObject,
					transform.position,
					BG.transform.position + new Vector3(BG.sizeDelta.x / 2 - 140, 0, 0),
					Time.deltaTime / 2));
				break;
			}
		}
		StartCoroutine(InterPhase());
	}

	//Bouncing Attack Code
	public Vector2 AddABitOfRandomness(Vector2 current, string wallName) {
		bounces++;
		print(bounces);
		switch (wallName) {
			case "TopWall": {
				Vector2 newVec = new Vector2(current.x, -current.y);
				float mag = newVec.magnitude;

				newVec = newVec.normalized;

				newVec = newVec + new Vector2(newVec.x, -Random.value);
				newVec = newVec.normalized;

				newVec = newVec * mag;
				return newVec;
			}
			case "RightWall": {
				Vector2 newVec = new Vector2(-current.x, current.y);
				float mag = newVec.magnitude;

				newVec = newVec.normalized;

				newVec = newVec + new Vector2(-Random.value, newVec.y);
				newVec = newVec.normalized;

				newVec = newVec * mag;
				return newVec;
			}
			case "BottomWall": {
				Vector2 newVec = new Vector2(current.x, -current.y);
				float mag = newVec.magnitude;

				newVec = newVec.normalized;

				newVec = newVec + new Vector2(newVec.x, Random.value);
				newVec = newVec.normalized;

				newVec = newVec * mag;
				return newVec;
			}
			case "LeftWall": {
				Vector2 newVec = new Vector2(-current.x, current.y);
				float mag = newVec.magnitude;

				newVec = newVec.normalized;

				newVec = newVec + new Vector2(Random.value, newVec.y);
				newVec = newVec.normalized;

				newVec = newVec * mag;
				return newVec;
			}
			default: {
				return Vector2.zero;
			}
		}
	}
	//


	//Caged Attack Code
	public IEnumerator Caged(float waitTime) {

		while (Attack2) {
			cageSize = positioningCage.GetComponent<RectTransform>().sizeDelta.x / 2;
			Vector3 target = GetPosInCage();
			print("New wait time: " + waitTime);
			yield return new WaitForSeconds(waitTime);
			Projectile bullet = pool_EnemyProjectile.getNext.GetComponent<Projectile>();

			bullet.byBoss = true;
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, target - gameObject.transform.position);
			bullet.transform.position = gameObject.transform.position;
			bullets.Add(bullet.gameObject);
			bullet.gameObject.SetActive(true);
			bullet.Fire();
			waitTime -= waitTime * 0.005f;
		}
	}
	public Vector3 GetPosInCage() {
		float x = UnityEngine.Random.Range(positioningCage.transform.position.x - cageSize, positioningCage.transform.position.x + cageSize);
		float y = UnityEngine.Random.Range(positioningCage.transform.position.y - cageSize, positioningCage.transform.position.y + cageSize);
		return new Vector3(x, y, 1);
	}
	public void ClearBullets() {
		foreach (GameObject bullet in bullets) {
			bullet.SetActive(false);
		}
	}
	//


	//Dodge KillerBlocks Attack Code
	public IEnumerator ChangeDir() {

		if (initialDealy) {
			yield return new WaitForSeconds(3);
			Attack3 = true;
			initialDealy = false;
			anim.Play("SpeedUp");
		}
		while (Attack3) {
			yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));
			preformChange = true;
		}
	}
	//


	//Brimstone like Attack
	public IEnumerator VariedRotation() {
		rotationDelta = 0.1f;
		zRotation = 0;
		while (true) {
			yield return new WaitForSeconds(UnityEngine.Random.Range(2, 4));
			int choice = UnityEngine.Random.Range(0, 2);

			if (choice == 0) {
				rotationDelta = UnityEngine.Random.Range(0.4f, 1f);
			}
			else {
				rotationDelta = UnityEngine.Random.Range(-1f, -0.4f);
			}
		}
	}
	//


	//Flappy bird like Attack Code
	public IEnumerator PipeGeneration() {

		float pipeSpacing = 1f;
		float playerDistance = Mathf.Abs(Mathf.Abs(player.transform.position.x) - Mathf.Abs(gameObject.transform.position.x));
		float arriveTime = 10f;
		float spawningPhaseTime = 2f;
		bool downwardsMovement = true;
		float shotDelay = 0.06f;
		int totalWallsGenerated = 9;


		print(true);
		for (int i = 0; i < totalWallsGenerated; i++) {

			int shots = (int)(spawningPhaseTime / shotDelay);
			float holeMid = Random.Range(-BG.sizeDelta.y / 2 + 20, BG.sizeDelta.y / 2 - 20);

			yield return new WaitForSeconds(pipeSpacing);


			if (downwardsMovement == true) {
				StartCoroutine(lerps.LerpPosition(gameObject,
					new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y + (BG.sizeDelta.y / 2), 0),
					new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y - (BG.sizeDelta.y / 2), 0),
					Time.deltaTime / 2));
				downwardsMovement = false;
			}
			else {
				StartCoroutine(lerps.LerpPosition(gameObject,
					new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y - (BG.sizeDelta.y / 2), 0),
					new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y + (BG.sizeDelta.y / 2), 0),
					Time.deltaTime / 2));
				downwardsMovement = true;
			}

			float timeAtStart = Time.timeSinceLevelLoad;
			while (shots > 0) {

				float change = arriveTime - (Time.timeSinceLevelLoad - timeAtStart);

				if ((transform.position.y > holeMid + 15 || transform.position.y < holeMid - 15) && currentAttack != null) {
					Projectile shot = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
					Projectile.projectileSpeed = playerDistance / change;
					shot.transform.position = transform.position;
					shot.transform.rotation = Quaternion.Euler(0, 0, 270);
					shot.gameObject.SetActive(true);
					shot.Fire();
					shot.StartCoroutine(shot.SelfDestruct(change + 1.5f));
				}
				shots--;
				yield return new WaitForSeconds(shotDelay);
			}
		}
		yield return new WaitForSeconds(arriveTime);
		doneBouncing = true;
	}

	//Bouce off walls
	private void OnCollisionEnter2D(Collision2D col) {
		if (Attack1) {
			if (col.transform.name == "BottomWall" || col.transform.name == "TopWall") {
				calculatedVec = AddABitOfRandomness(col.relativeVelocity, col.transform.name);
				rigid.velocity = calculatedVec;
			}
			if (col.transform.name == "LeftWall" || col.transform.name == "RightWall") {
				calculatedVec = AddABitOfRandomness(col.relativeVelocity, col.transform.name);
				rigid.velocity = calculatedVec;
			}
		}
	}
	//

	//Loop
	private void Update() {

		if (Attack3 == true) {
			rigid.velocity = new Vector2(1, 0) * anim.GetFloat("Speed");
			RaycastHit2D[] right = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.right, BG.sizeDelta.x * 2);
			RaycastHit2D[] left = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.left, BG.sizeDelta.x * 2);

			foreach (RaycastHit2D hit in right) {
				if (hit.transform.tag == "BossWall") {
					distR = Vector3.Distance(transform.position, hit.point);
					if (distR < changeThreshold && dontChangeR == false) {
						dontChangeR = true;
						anim.SetTrigger("Right");
						changes++;

					}
					else if (distR > changeThreshold + 10) {
						dontChangeR = false;
					}
				}
			}
			foreach (RaycastHit2D hit in left) {
				if (hit.transform.tag == "BossWall") {
					distL = Vector3.Distance(transform.position, hit.point);
					if (distL < changeThreshold && dontChangeL == false) {
						dontChangeL = true;
						anim.SetTrigger("Left");
						changes++;
					}
					else if (distL > changeThreshold + 10) {
						dontChangeL = false;
					}
				}
			}
			if (!dontChangeR && !dontChangeL && preformChange == true) {
				preformChange = false;

				if (anim.GetFloat("Speed") > 0) {
					headingRight = true;
				}
				else {
					headingRight = false;
				}

				int r = Random.Range(0, 2);
				if (r == 0) {
					if (headingRight) {
						anim.SetTrigger("Right");
						changes++;
					}
				}
				else {
					if (!headingRight) {
						anim.SetTrigger("Left");
						changes++;
					}
				}
			}
		}
	}

	private void FixedUpdate() {
		if (Attack4 == true) {
			zRotation += rotationDelta;
			transform.rotation = Quaternion.Euler(0, 0, zRotation);

			RaycastHit2D[] up = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.up, BG.sizeDelta.x * 2);
			RaycastHit2D[] right = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.right, BG.sizeDelta.x * 2);
			RaycastHit2D[] down = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.down, BG.sizeDelta.x * 2);
			RaycastHit2D[] left = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.left, BG.sizeDelta.x * 2);

			//Debug.DrawRay(transform.position, transform.rotation * Vector3.up * 500, Color.blue);
			//Debug.DrawRay(transform.position, transform.rotation * Vector3.right * 500, Color.green);
			//Debug.DrawRay(transform.position, transform.rotation * Vector3.down * 500, Color.red);
			//Debug.DrawRay(transform.position, transform.rotation * Vector3.left * 500, Color.yellow);

			foreach (RaycastHit2D hit in up) {
				if (hit.transform.tag == "BossWall") {
					topBrim.position = ((Vector2)transform.position + hit.point) * 0.5f;

					float dist = Vector3.Distance(transform.position, hit.point);
					topBrim.localScale = new Vector3(1, dist / topBrim.sizeDelta.y, 1);
					topBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
				}
			}
			foreach (RaycastHit2D hit in right) {
				if (hit.transform.tag == "BossWall") {
					rightBrim.position = ((Vector2)transform.position + hit.point) * 0.5f;

					float dist = Vector3.Distance(transform.position, hit.point);
					rightBrim.localScale = new Vector3(1, dist / rightBrim.sizeDelta.y, 1);
					rightBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
				}
			}
			foreach (RaycastHit2D hit in down) {
				if (hit.transform.tag == "BossWall") {
					bottomBrim.position = ((Vector2)transform.position + hit.point) * 0.5f;

					float dist = Vector3.Distance(transform.position, hit.point);
					bottomBrim.localScale = new Vector3(1, dist / bottomBrim.sizeDelta.y, 1);
					bottomBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
				}
			}
			foreach (RaycastHit2D hit in left) {
				if (hit.transform.tag == "BossWall") {
					leftBrim.position = ((Vector2)transform.position + hit.point) * 0.5f;

					float dist = Vector3.Distance(transform.position, hit.point);
					leftBrim.localScale = new Vector3(1, dist / leftBrim.sizeDelta.y, 1);
					leftBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
				}
			}
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
}