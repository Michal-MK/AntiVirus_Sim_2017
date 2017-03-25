using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour {

	#region Outside References

	public Animator anim;

	public Sprite Invincible;
	public Sprite Damageable;

	public GameObject heart;
	public GameObject spikeHit;
	public GameObject spikeS;
	public GameObject body;

	public GameObject cageObj;
	public GameObject Brimstone;
	public GameObject block;

	public ObjectPooler poolOfEnemyProjectiles;
	public ObjectPooler poolOfKillerBlocks;
	public BossHealth hp;

	public RectTransform self;

	GameObject player;
	RectTransform BG;
	public Rigidbody2D rigid;

	public bool isHit = false;
	public bool isAttacking = false;

	public bool Attack1 = false;
	public bool Attack2 = false;
	public bool Attack3 = false;
	public bool moveCage = false;
	public bool Attack4 = false;
	public bool Attack5 = false;


	public float IFrames = 2;
	public float animFrame = 0;


	public int attackNo;
	public int fullCircle;

	public Vector2 oldvec;
	public Vector2 calculatedVec;

	public GameObject Buttons;

	#endregion

	#region Inside References

	public float speed = 0;

	private GameObject topBrim;
	private GameObject rightBrim;
	private GameObject bottomBrim;
	private GameObject leftBrim;
	private GameObject positioningCage;

	private RectTransform topRect;
	private RectTransform rightRect;
	private RectTransform bottomRect;
	private RectTransform leftRect;
	private Coroutine mycor;

	int changes = 0;
	int bounces = 0;

	private List<GameObject> bullets = new List<GameObject>();

	private float cageSize;

	private Vector2 upMid;
	private Vector2 rightMid;
	private Vector2 downMid;
	private Vector2 leftMid;
	private float z;
	private Quaternion q;
	private float rotationDelta = 0.1f;


	public bool bossSpawned = false;
	private bool initialDealy = true;
	private bool doneBouncing = false;

	Vector3 attack1StartPos;
	Vector3 attack2StartPos;
	Vector3 attack3StartPos;
	Vector3 attack4StartPos;
	Vector3 attack5StartPos;

	Coroutine Atk;
	Coroutine Positioning;

	private bool headingRight = true;
	private bool preformChange = false;
	private bool dontChangeR = false;
	private bool dontChangeL = false;
	private float distR = 0;
	private float distL = 0;
	private float changeThreshold = 45;

	private bool informOnce = true;

	public BoxCollider2D[] spikeHitboxes = new BoxCollider2D[4];
	public SpriteRenderer selfRender;

	#endregion

	private void Awake() {
		Statics.bossBehaviour = this;
	}

	void Start() {
		Projectile.spawnedByAvoidance = false;
		Projectile.spawnedByKillerWall = false;

		BG = GameObject.Find("Background_room_Boss_1").GetComponent<RectTransform>();
		player = GameObject.FindGameObjectWithTag("Player");
		poolOfEnemyProjectiles = GameObject.Find("EnemyProjectileInaccurate Pooler").GetComponent<ObjectPooler>();
		poolOfKillerBlocks = GameObject.Find("KillerBlockBoss Pooler").GetComponent<ObjectPooler>();
		Buttons = GameObject.Find("Buttons");
		Statics.mPlayer.disableSavesByBoss = true;
		rigid = gameObject.GetComponent<Rigidbody2D>();
		rigid.freezeRotation = true;

		attack1StartPos = BG.position + new Vector3(-BG.sizeDelta.x /2 + 40, - BG.sizeDelta.y / 2 + 40 );
		attack2StartPos = new Vector3(-530, -70, 1);
		attack3StartPos = new Vector3(-368, -70, 1);
		attack4StartPos = BG.position;
		attack5StartPos = (Vector2)BG.transform.position + BG.sizeDelta / 2 + new Vector2(-10, 0);
		fullCircle = 2;

		StartCoroutine(InitialAttack());

	}

	private IEnumerator InitialAttack() {

		yield return new WaitUntil(() => CameraMovement.doneMoving);
		Control.script.Save(false,false);
		bossSpawned = true;
		Statics.canvasRenderer.infoRenderer("Ahh I see, you are persistent.. but you won't escape me!", "Attack mode with \"Space\", aim with mouse, Red = Invincible, Blue = Damageable");
		yield return new WaitForSeconds(1);

		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = false;
		}
		selfRender.sprite = Invincible;

		//StartCoroutine(Attacks(ChooseAttack()));

		StartCoroutine(Attacks(5));

	}

	public IEnumerator InterPhase() {
		for (int i = 0; i < spikeHitboxes.Length; i++) {
			spikeHitboxes[i].enabled = true;
		}
		selfRender.sprite = Damageable;
		hp.CheckShields();
		int choice = ChooseAttack();
		yield return new WaitForSeconds(5);
		StartCoroutine(Attacks(choice));

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
		switch (attack) {

			//Bouncing Attack
			case 1: {
				anim.enabled = false;
				rigid.isKinematic = false;
				Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack1StartPos));
				yield return new WaitForSeconds(3);
				body.GetComponent<CircleCollider2D>().isTrigger = true;


				//Actual Attack --
				isAttacking = true;
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
				isAttacking = false;
				Attack1 = false;
				anim.enabled = true;
				rigid.isKinematic = true;
				body.GetComponent<CircleCollider2D>().isTrigger = false;
				StartCoroutine(InterPhase());
				StopCoroutine(Attacks(attack));

				break;
			}

			//Caged Attack
			case 2: {

				isAttacking = true;
				Attack2 = true;
				Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack2StartPos));
				//Statics.playerParticles.ActivateScript(gameObject.transform, true);
				positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
				positioningCage.name = "PositioningCage";

				yield return new WaitForSeconds(2);

				//Actual Attack
				Projectile.projectileSpeed = 15;

				Positioning = StartCoroutine(LerpPos(positioningCage, positioningCage.transform.position, BG.transform.position));
				yield return new WaitForSeconds(3);
				//Statics.playerParticles.ActivateScript(gameObject.transform, false);
				Statics.canvasRenderer.infoRenderer(null, "Don't forget aout the zooming feature :]");

				float waitTime = 1.1f;

				for (int i = 0; i <= fullCircle; i++) {
					Debug.Log("Preforming " + (i + 1) + ". circle.");
					float newWait = waitTime - 0.1f;
					waitTime = newWait;

					anim.Play("Attack" + attack);
					mycor = StartCoroutine(Caged(newWait));

					yield return new WaitUntil(() => animFrame == 871);

					anim.SetTrigger("Attack" + attack);

					yield return new WaitForSeconds(Time.deltaTime);

					StopCoroutine(mycor);
				}
				//--//

				isAttacking = false;
				Attack2 = false;
				StopCoroutine(mycor);

				yield return new WaitForSeconds(2f);
				Destroy(positioningCage.gameObject);
				ClearBullets();
				StartCoroutine(InterPhase());
				StopCoroutine(Attacks(attack));

				break;
			}

			//Avoid KillerBlocks
			case 3: {


				Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack3StartPos));
				//Statics.playerParticles.ActivateScript(gameObject.transform, true);
				positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
				positioningCage.name = "PositioningCage";

				yield return new WaitForSeconds(3);

				Positioning = StartCoroutine(LerpPos(positioningCage, positioningCage.transform.position, gameObject.transform.position + new Vector3(0, 50, 0)));
				moveCage = true;
				//Statics.playerParticles.ActivateScript(gameObject.transform, false);
				yield return new WaitForSeconds(2);

				isAttacking = true;
				anim.Play("SpeedUp");

				//Actual Attack
				StartCoroutine(ChangeDir());
				while (isAttacking) {

					GameObject BlockL = poolOfKillerBlocks.GetPool();
					BlockL.SetActive(true);
					GameObject BlockR = poolOfKillerBlocks.GetPool();
					BlockR.SetActive(true);

					BlockL.transform.position = new Vector3(transform.position.x - self.sizeDelta.x / 2, transform.position.y, 1);
					BlockR.transform.position = new Vector3(transform.position.x + self.sizeDelta.x / 2, transform.position.y, 1);

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
				//--//

				isAttacking = false;
				Attack3 = false;
				moveCage = false;
				initialDealy = true;
				dontChangeL = false;
				dontChangeR = false;
				distL = 0;
				distR = 0;
				rigid.velocity = Vector3.zero;
				StartCoroutine(InterPhase());
				StopCoroutine(Attacks(attack));
				break;
			}

			//Brimstone like Attack
			case 4: {

				anim.Play("Attack" + attack);
				Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack4StartPos));
				yield return new WaitForSeconds(2f);
				gameObject.GetComponent<ParticleSystem>().Emit(100);
				yield return new WaitForSeconds(2f);
				isAttacking = true;
				Attack4 = true;
				topBrim = Instantiate(Brimstone);
				topBrim.name = "Top";
				topRect = topBrim.GetComponent<RectTransform>();

				rightBrim = Instantiate(Brimstone);
				rightBrim.name = "Right";
				rightRect = rightBrim.GetComponent<RectTransform>();

				bottomBrim = Instantiate(Brimstone);
				bottomBrim.name = "Bottom";
				bottomRect = bottomBrim.GetComponent<RectTransform>();

				leftBrim = Instantiate(Brimstone);
				leftBrim.name = "Left";
				leftRect = leftBrim.GetComponent<RectTransform>();

				StartCoroutine(VariedRotation());

				yield return new WaitForSeconds(35);
				//--//

				topBrim.SetActive(false);
				rightBrim.SetActive(false);
				bottomBrim.SetActive(false);
				leftBrim.SetActive(false);
				transform.rotation = Quaternion.identity;
				Attack4 = false;
				isAttacking = false;


				StopCoroutine(VariedRotation());
				StartCoroutine(InterPhase());
				StopCoroutine(Attacks(attack));
				break;
			}

			//Flappybird like Attack
			case 5: {
				isAttacking = true;
				positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
				positioningCage.name = "PositioningCage";

				Atk = StartCoroutine(LerpPos(gameObject, gameObject.transform.position, attack5StartPos));

				yield return new WaitForSeconds(2);
				if (informOnce) {
					informOnce = false;
					Statics.canvasRenderer.infoRenderer("Flappy Bird!!! (Press \"UpArrow\" or \"W\") to flap. ", "Press \"Up or W\" to flap.");
				}
				Positioning = StartCoroutine(LerpPos(positioningCage, positioningCage.transform.position, (Vector2)BG.transform.position - BG.sizeDelta / 2 + new Vector2(40, 20)));

				yield return new WaitForSeconds(2);
				Destroy(positioningCage.gameObject);
				player.GetComponent<M_Player>().ChangeFlappy(true);
				StartCoroutine(PipeGeneration());

				yield return new WaitUntil(() => doneBouncing);

				player.GetComponent<M_Player>().ChangeFlappy(false);
				isAttacking = false;
				doneBouncing = false;
				Atk = StartCoroutine(LerpPos(gameObject, transform.position, BG.transform.position + new Vector3(BG.sizeDelta.x / 2 - 140, 0, 0)));
				StartCoroutine(InterPhase());
				StopCoroutine(Attacks(attack));

				break;
			}
		}
	}
	//

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

			yield return new WaitForSeconds(waitTime);
			GameObject bullet = poolOfEnemyProjectiles.GetPool();

			bullet.GetComponent<Projectile>().byBoss = true;
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, target - gameObject.transform.position);
			bullet.transform.position = gameObject.transform.position;
			bullets.Add(bullet);
			bullet.SetActive(true);
			StopCoroutine(Caged(waitTime));
			//print("Loooped");
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
		z = 0;
		while (true) {
			yield return new WaitForSeconds(UnityEngine.Random.Range(2, 4));
			int choice = UnityEngine.Random.Range(0, 2);

			if (choice == 0) {
				rotationDelta = UnityEngine.Random.Range(0.1f, 0.4f);
			}
			else {
				rotationDelta = UnityEngine.Random.Range(-0.4f, -0.1f);
			}
		}
	}
	//


	//Flappy bird like Attack Code
	public IEnumerator PipeGeneration() {

		float PipePeriod = 1f;
		float distToPly = Mathf.Abs(Mathf.Abs(player.transform.position.x) - Mathf.Abs(gameObject.transform.position.x));
		float arriveTime = 10f;
		float spawningPhaseTime = 2f;
		bool GoingDown = true;
		float shotdelay = 0.05f;
		int noOfWallsGenerated = 11;


		print(true);
		for (int i = 0; i < noOfWallsGenerated; i++) {

			int shots = (int)(spawningPhaseTime / shotdelay);
			float holeMid = Random.Range(-BG.sizeDelta.y / 2 + 20, BG.sizeDelta.y / 2 - 20);

			yield return new WaitForSeconds(PipePeriod);

			
			if (GoingDown == true) {
				Atk = StartCoroutine(LerpPos(gameObject, new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y + (BG.sizeDelta.y / 2), 0), new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y - (BG.sizeDelta.y / 2), 0), false));
				GoingDown = false;
			}
			else {
				Atk = StartCoroutine(LerpPos(gameObject, new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y - (BG.sizeDelta.y / 2), 0), new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y + (BG.sizeDelta.y / 2), 0), false));
				GoingDown = true;
			}

			float timeOnStart = Time.timeSinceLevelLoad;
			while (shots > 0) {

				float change = arriveTime - (Time.timeSinceLevelLoad - timeOnStart);

				if (transform.position.y > holeMid + 12 || transform.position.y < holeMid - 12) {
					GameObject shot = poolOfEnemyProjectiles.GetPool();
					Projectile script = shot.GetComponent<Projectile>();
					Projectile.projectileSpeed = distToPly / change;

					script.DisableCollisions = true;
					shot.transform.position = transform.position;
					shot.transform.rotation = Quaternion.Euler(0, 0, 270);
					shot.SetActive(true);
					script.StartCoroutine(script.SelfDestruct(change + 1.5f));
				}
				shots -= 1;
				yield return new WaitForSeconds(shotdelay);

			}

		}
		yield return new WaitForSeconds(arriveTime);
		doneBouncing = true;
	}
	//

	//Bouce off walls
	private void OnCollisionEnter2D(Collision2D col) {
		if (isAttacking == true && Attack1) {

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

	//Generic functions
	public IEnumerator LerpPos(GameObject obj, Vector3 start, Vector3 end, bool smooth = true) {


		float sX = start.x;
		float sY = start.y;
		float eX = end.x;
		float eY = end.y;
		float pX = player.transform.position.x;
		float pY = player.transform.position.y;

		print(obj.name);
		for (float t = 0; t < 2; t += Time.deltaTime * 0.5f) {
			if (Time.timeScale == 0) {
				yield return new WaitForSeconds(0.1f);
			}
			else {
				float newX;
				float newY;
				float newPX;
				float newPY;

				if (smooth) {
					newX = Mathf.SmoothStep(sX, eX, t);
					newY = Mathf.SmoothStep(sY, eY, t);
					newPX = Mathf.SmoothStep(pX, eX, t);
					newPY = Mathf.SmoothStep(pY, eY, t);
				}
				else {
					newX = Mathf.Lerp(sX, eX, t);
					newY = Mathf.Lerp(sY, eY, t);
					newPX = Mathf.Lerp(pX, eX, t);
					newPY = Mathf.Lerp(pY, eY, t);
				}
				if (obj != null) {
					obj.transform.position = new Vector3(newX, newY, start.z);

					if (obj.name == "PositioningCage") {
						player.transform.position = new Vector3(newPX, newPY, end.z);
					}
					if (hp.stopEverything) {
						yield break;
					}
					if (t < 1) {
						yield return null;
					}
					if (t > 1) {
						if (moveCage) {
							positioningCage.GetComponent<CageScript>().MoveUp();
							StopCoroutine(Positioning);
							break;
						}
						else {
							StopCoroutine(Atk);
							break;
						}
					}
				}
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

		if (Attack4 == true) {
			z += rotationDelta;
			q = Quaternion.Euler(0, 0, z);
			transform.rotation = q;

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
					upMid = ((Vector2)transform.position + hit.point) * 0.5f;
					topBrim.transform.position = upMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					topRect.transform.localScale = new Vector3(1, dist / topRect.sizeDelta.y, 1);

					topBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));


				}
			}
			foreach (RaycastHit2D hit in right) {
				if (hit.transform.tag == "BossWall") {
					rightMid = ((Vector2)transform.position + hit.point) * 0.5f;
					rightBrim.transform.position = rightMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					rightRect.transform.localScale = new Vector3(1, dist / rightRect.sizeDelta.y, 1);
					rightBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));


				}
			}
			foreach (RaycastHit2D hit in down) {
				if (hit.transform.tag == "BossWall") {
					downMid = ((Vector2)transform.position + hit.point) * 0.5f;
					bottomBrim.transform.position = downMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					bottomRect.transform.localScale = new Vector3(1, dist / bottomRect.sizeDelta.y, 1);

					bottomBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));



				}
			}
			foreach (RaycastHit2D hit in left) {
				if (hit.transform.tag == "BossWall") {
					leftMid = ((Vector2)transform.position + hit.point) * 0.5f;
					leftBrim.transform.position = leftMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					leftRect.transform.localScale = new Vector3(1, dist / leftRect.sizeDelta.y, 1);

					leftBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));


				}
			}
		}
	}
	//

	private void OnDestroy() {
		Statics.bossBehaviour = null;
	}
}



