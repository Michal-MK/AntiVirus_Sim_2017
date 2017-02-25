using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {

	#region Outside References
	public Sprite SDmg1;
	public Sprite SDmg2;
	public Sprite SDmg3;

	public SpriteRenderer InnerS;

	public Animator anim;

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
	#endregion

	#region Inside References
	float initX = 1;
	float initY = 1;

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

	//private List<GameObject> Blocks = new List<GameObject>();

	private List<GameObject> bullets = new List<GameObject>();

	private float cageSize;

	private Vector2 upMid;
	private Vector2 rightMid;
	private Vector2 downMid;
	private Vector2 leftMid;
	private float z;
	private Quaternion q;
	private float rotationDelta = 0.1f;

	private bool doOnceR = true;
	private bool doOnceL = true;
	private bool initialDealy = true;
	private bool doneBouncing = false;

	Vector3 attack1StartPos;
	Vector3 attack2StartPos;
	Vector3 attack3StartPos;
	Vector3 attack4StartPos;
	Vector3 attack5StartPos;

	Coroutine Atk;
	Coroutine Positioning;

	public float changeDirComparison;

	#endregion


	void Start() {
		BG = GameObject.Find("Background_room_Boss_1").GetComponent<RectTransform>();
		player = GameObject.FindGameObjectWithTag("Player");
		poolOfEnemyProjectiles = GameObject.Find("EnemyProjectile Pooler").GetComponent<ObjectPooler>();
		poolOfKillerBlocks = GameObject.Find("KillerBlockBoss Pooler").GetComponent<ObjectPooler>();
		rigid = gameObject.GetComponent<Rigidbody2D>();

		attack1StartPos = BG.position;
		attack2StartPos = new Vector3(-530, -70, 1);
		attack3StartPos = new Vector3(-368, -70, 1);
		attack4StartPos = BG.position;
		//attack5StartPos = insert position
		fullCircle = 1;

		StartCoroutine(InitialAttack());

	}

	private IEnumerator InitialAttack() {
		yield return new WaitForSecondsRealtime(5);
		//StartCoroutine(Attacks(3));
		StartCoroutine(InterPhase());
		//Canvas_Renderer.script.infoRenderer("Be careful and good luck.");
	}

	private IEnumerator InterPhase() {
		hp.invincible = false;
		int choice = ChooseAttack();
		yield return new WaitForSeconds(3);
		StartCoroutine(Attacks(choice));
		

	}

	public int ChooseAttack() {
		int previous = attackNo;

		while (previous == attackNo) {
			attackNo = UnityEngine.Random.Range(1, 6);
		}
		return attackNo;
	}

	public IEnumerator Attacks(int attack) {
		switch (attack) {

			//Bouncing Attack
			#region case 1:

			case 1:
			hp.invincible = true;
			Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack1StartPos));
			yield return new WaitForSecondsRealtime(2);
			body.GetComponent<CircleCollider2D>().isTrigger = true;
			anim.Play("Attack" + attack);


			//Actual Attack --
			isAttacking = true;
			Attack1 = true;
			calculatedVec = new Vector2(initX, initY);
			oldvec = calculatedVec;
			yield return new WaitUntil(() => animFrame == 1800);
			//--//

			anim.SetTrigger("Attack" + attack);
			isAttacking = false;
			Attack1 = false;
			body.GetComponent<CircleCollider2D>().isTrigger = false;
			StartCoroutine(InterPhase());
			StopCoroutine(Attacks(attack));

			break;

			#endregion

			//Caged Attack
			#region case 2:
			case 2:

			isAttacking = true;
			Attack2 = true;
			hp.invincible = true;
			Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack2StartPos));
			positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
			positioningCage.name = "PositioningCage";

			yield return new WaitForSecondsRealtime(2);

			//Actual Attack


			Positioning = StartCoroutine(LerpPos(positioningCage, positioningCage.transform.position, BG.transform.position));
			yield return new WaitForSecondsRealtime(3);

			float waitTime = 1.1f;

			for (int i = 0; i <= fullCircle; i++) {

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
			#endregion

			//Avoid KillerBlocks
			#region case 3:
			case 3:

			hp.invincible = true;

			Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack3StartPos));

			positioningCage = Instantiate(cageObj, player.transform.position, Quaternion.identity);
			positioningCage.name = "PositioningCage";

			yield return new WaitForSecondsRealtime(3);

			Positioning = StartCoroutine(LerpPos(positioningCage, positioningCage.transform.position, gameObject.transform.position + new Vector3(0, 50, 0)));
			moveCage = true;
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
				if (changes >= 3) {
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
			rigid.velocity = Vector3.zero;
			StartCoroutine(InterPhase());
			StopCoroutine(Attacks(attack));
			break;
			#endregion

			//Brimstone like Attack
			#region case 4:
			case 4:

			anim.Play("Attack" + attack);
			hp.invincible = true;
			Atk = StartCoroutine(LerpPos(gameObject, transform.position, attack4StartPos));
			yield return new WaitForSecondsRealtime(2);

			//Actual Attack 
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

			yield return new WaitForSeconds(30);
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
			#endregion

			//Flappybird like Attack
			#region case 5:
			case 5:
			isAttacking = true;
			hp.invincible = true;
			yield return new WaitForSecondsRealtime(2);
			Canvas_Renderer.script.infoRenderer("Flappy Bird!!! (Press \"UpArrow\" or \"W\") to flap. ");

			player.transform.position = (Vector2)BG.transform.position - BG.sizeDelta / 2 + new Vector2(20, 20);
			transform.position = (Vector2)BG.transform.position + BG.sizeDelta / 2 + new Vector2(-10, 0);

			player.GetComponent<M_Player>().ChangeFlappy(true);
			StartCoroutine(PipeGeneration());

			yield return new WaitUntil(() => doneBouncing);

			player.GetComponent<M_Player>().ChangeFlappy(false);
			isAttacking = false;
			doneBouncing = false;
			StartCoroutine(InterPhase());
			StopCoroutine(Attacks(attack));

			break;
			#endregion
		}
	}


	//Bouncing Attack Code
	public Vector2 AddABitOfRandomness(Vector2 current, bool horisontal) {
		float randPositive = UnityEngine.Random.value;
		float randNegative = -UnityEngine.Random.value;

		if (horisontal) {

			Vector2 directed = new Vector2(current.x, current.y * -1);
			Vector2 changed = new Vector2(directed.x + randPositive, directed.y + randNegative);
			oldvec = changed;
			return changed.normalized;
		}
		else {
			Vector2 directed = new Vector2(current.x * -1, current.y);


			Vector2 changed = new Vector2(directed.x + randNegative, directed.y + randPositive);
			oldvec = changed;
			return changed.normalized;
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
		while (isAttacking) {
			if (initialDealy) {
				yield return new WaitForSeconds(3);
				Attack3 = true;
				initialDealy = false;
			}
			yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(2, 5));
			bool headingRight;
			if (anim.GetFloat("Speed") > 0) {
				headingRight = true;
			}
			else {
				headingRight = false;
			}

			if (doOnceL || doOnceR) {
				int r = Random.Range(0, 2);
				if (r == 0) {
					if (headingRight) {
						anim.SetTrigger("RDist");
						changes++;
					}
				}
				else {
					if (!headingRight) {
						anim.SetTrigger("LDist");
						changes++;
					}
				}
			}


		}
	}
	//


	//Brimstone like Attack
	public IEnumerator VariedRotation() {
		while (true) {
			yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(2, 4));
			int choice = UnityEngine.Random.Range(0, 2);

			if (choice == 0) {
				rotationDelta = UnityEngine.Random.Range(0.02f, 0.5f);
			}
			else {
				rotationDelta = UnityEngine.Random.Range(-0.5f, -0.02f);
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
		

		print(true);
		for (int i = 0; i < 5; i++) {
			// generate new pipe

			int shots = (int)(spawningPhaseTime / shotdelay);
			//
			yield return new WaitForSecondsRealtime(PipePeriod);
			float holeMid = Random.Range(-BG.sizeDelta.y / 2 + 20, BG.sizeDelta.y / 2 - 20);
			if (GoingDown == true) {
				Atk = StartCoroutine(LerpPos(gameObject, new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2)-10, BG.transform.position.y + (BG.sizeDelta.y / 2), 0), new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y - (BG.sizeDelta.y / 2), 0), false));
				GoingDown = false;
			}
			else {
				Atk = StartCoroutine(LerpPos(gameObject, new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y - (BG.sizeDelta.y / 2), 0), new Vector3(BG.transform.position.x + (BG.sizeDelta.x / 2) - 10, BG.transform.position.y + (BG.sizeDelta.y / 2), 0), false));
				GoingDown = true;
			}
			float timeOnStart = Time.timeSinceLevelLoad;
			while (shots > 0) {
				if (transform.position.y > holeMid + 15 || transform.position.y < holeMid - 15) {
					GameObject shot = poolOfEnemyProjectiles.GetPool();
					shot.GetComponent<Projectile>().timeTillDestruct = arriveTime + 0.5f;
					shot.GetComponent<Projectile>().DisableCollisions = true;
					shot.transform.position = transform.position;
					shot.transform.rotation = Quaternion.Euler(0, 0, 270);
					Projectile.projectileSpeed = distToPly / (arriveTime - (Time.timeSinceLevelLoad - timeOnStart));
					shot.SetActive(true);
				}
				shots -= 1;
				yield return new WaitForSecondsRealtime(shotdelay);

			}

		}
		yield return new WaitForSecondsRealtime(arriveTime);
		doneBouncing = true;
	}
	//

	//Bouce off walls
	private void OnTriggerEnter2D(Collider2D col) {
		if (isAttacking == true && Attack1) {

			if (col.name == "Wall_Horizontal" || col.name == "TopWall") {
				calculatedVec = AddABitOfRandomness(oldvec, true);
				rigid.velocity = calculatedVec * speed;

			}
			if (col.name == "Wall_Vertical") {
				calculatedVec = AddABitOfRandomness(oldvec, false);
				rigid.velocity = calculatedVec * speed;

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

	private void Update() {

		if (Attack1 == true) {
			rigid.velocity = calculatedVec * speed;
		}

		if (Attack3 == true) {
			rigid.velocity = Vector2.right * anim.GetFloat("Speed");
			RaycastHit2D[] right = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.right, BG.sizeDelta.x * 2);
			RaycastHit2D[] left = Physics2D.RaycastAll(transform.position, transform.rotation * Vector3.left, BG.sizeDelta.x * 2);

			foreach (RaycastHit2D hit in right) {
				if (hit.transform.tag == "BossWall") {
					float dist = Vector3.Distance(transform.position, hit.point);

					if (dist > changeDirComparison + 10) {
						doOnceR = true;
					}
					if (dist < changeDirComparison) {
						if (doOnceR) {
							anim.SetTrigger("RDist");
							changes++;
							doOnceR = false;
						}
					}
				}
			}
			foreach (RaycastHit2D hit in left) {
				if (hit.transform.tag == "BossWall") {
					float dist = Vector3.Distance(transform.position, hit.point);

					if (dist > changeDirComparison + 10) {
						doOnceL = true;
					}
					if (dist < changeDirComparison) {
						if (doOnceL) {
							anim.SetTrigger("LDist");
							changes++;
							doOnceL = false;
						}
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
}
