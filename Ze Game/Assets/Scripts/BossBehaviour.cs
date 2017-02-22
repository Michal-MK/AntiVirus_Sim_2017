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

	public RectTransform self;

	GameObject player;
	RectTransform BG;
	public Rigidbody2D rigid;

	public bool isHit = false;
	public bool isAttacking = false;

	public bool Attack1 = false;
	public bool Attack2 = false;
	public bool Attack3 = false;
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

	GameObject cage;

	private GameObject topBrim;
	private GameObject rightBrim;
	private GameObject bottomBrim;
	private GameObject leftBrim;

	RectTransform topRect;
	RectTransform rightRect;
	RectTransform bottomRect;
	RectTransform leftRect;
	Coroutine mycor;
	int changes = 0;

	private List<GameObject> Blocks = new List<GameObject>();

	private List<GameObject> bullets = new List<GameObject>();

	private float cageSize;

	private Vector2 upMid;
	private Vector2 rightMid;
	private Vector2 downMid;
	private Vector2 leftMid;
	private float z;
	private Quaternion q;
	private float rotationDelta = 0.1f;

	public bool doOnceR = true;
	public bool doOnceL = true;

	Vector3 attack1StartPos;
	Vector3 attack2StartPos;
	Vector3 attack3StartPos;
	Vector3 attack4StartPos;
	Vector3 attack5StartPos;

	Coroutine Atk;

	#endregion


	void Start() {
		BG = GameObject.Find("Background_room_Boss_1").GetComponent<RectTransform>();
		player = GameObject.FindGameObjectWithTag("Player");
		poolOfEnemyProjectiles = GameObject.Find("EnemyProjectile Pooler").GetComponent<ObjectPooler>();
		poolOfKillerBlocks = GameObject.Find("KillerBlockBoss Pooler").GetComponent<ObjectPooler>();
		rigid = gameObject.GetComponent<Rigidbody2D>();

		attack1StartPos = BG.position;
		attack2StartPos = new Vector3(-520, -70, 1);
		attack3StartPos = new Vector3(-530, -70, 1);
		attack4StartPos = BG.position;
		//attack5StartPos = insert position

		//StartCoroutine(InterPhase());
		StartCoroutine(Attacks(4));
		fullCircle = 1;
	}

	private IEnumerator InterPhase() {
		int choice = ChooseAttack();
		yield return new WaitForSeconds(1);
		StartCoroutine(Attacks(choice));
	}

	public int ChooseAttack() {
		int previous = attackNo;

		while (previous == attackNo) {
			attackNo = UnityEngine.Random.Range(1, 5);
		}
		return attackNo;
	}
	
	public IEnumerator Attacks(int attack) {
		switch (attack) {

			//Bouncing Attack
			#region case 1:

			case 1:
			isAttacking = true;
			Attack1 = true;
			Canvas_Renderer.script.infoRenderer("Chose Attack: " + attack);

			Atk = StartCoroutine(LerpPos(transform.position,attack1StartPos));
			yield return new WaitForSecondsRealtime(2);

			anim.Play("Attack" + attack);

			
			//Actual Attack --
			//gameObject.transform.position = attack1StartPos;

			calculatedVec = new Vector2(initX, initY);
			oldvec = calculatedVec;
			
			yield return new WaitUntil(() => animFrame == 1800);
			//--//


			anim.SetTrigger("Attack" + attack);
			isAttacking = false;
			Attack1 = false;
			StartCoroutine(InterPhase());
			StopCoroutine(Attacks(attack));

			break;

			#endregion

			//Caged Attack
			#region case 2:
			case 2:

			isAttacking = true;
			Attack2 = true;
			Canvas_Renderer.script.infoRenderer("Chose Attack: " + attack);
			Atk = StartCoroutine(LerpPos(transform.position, attack2StartPos));
			yield return new WaitForSecondsRealtime(5);

			//Actual Attack
			player.transform.position = BG.transform.position;
			cage = Instantiate(cageObj, BG.transform.position, Quaternion.identity);
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

			ClearBullets();
			StartCoroutine(InterPhase());
			StopCoroutine(Attacks(attack));

			break;
			#endregion

			//Avoid KillerBlocks
			#region case 3:
			case 3:

			isAttacking = true;
			Attack3 = true;
			anim.Play("SpeedUp");
			Canvas_Renderer.script.infoRenderer("Chose Attack: " + attack);
			Atk = StartCoroutine(LerpPos(transform.position, attack3StartPos));
			yield return new WaitForSecondsRealtime(2);


			//Actual Attack
			//gameObject.transform.position = attack3StartPos;
			StartCoroutine(ChangeDir());
			while (Attack3) {

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
				if (changes >= 20) {
					anim.SetTrigger("Attack" + attack);
					break;
				}
			}
			yield return new WaitForSeconds(1);
			//--//

			isAttacking = false;
			Attack3 = false;
			StartCoroutine(InterPhase());
			StopCoroutine(Attacks(attack));
			break;
			#endregion

			//Brimstone like Attack
			#region case 4:
			case 4:
			isAttacking = true;

			anim.Play("Attack" + attack);
			Canvas_Renderer.script.infoRenderer("Chose Attack: " + attack);
			Atk = StartCoroutine(LerpPos(transform.position, attack4StartPos));
			yield return new WaitForSecondsRealtime(2);

			//Actual Attack 
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

			yield return new WaitForSeconds(20);
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
			//case 5:
			//isAttacking = true;
			//anim.Play("Attacks" + attack);
			//Atk = StartCoroutine(LerpPos(transform.position, attack5StartPos));
			//yield return new WaitForSecondsRealtime(2);



			//yield return new WaitForSeconds(WaitTime);
			//isAttacking = false;
			//StopCoroutine(Attacks(attack));
			//break;
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
			cageSize = cage.GetComponent<RectTransform>().sizeDelta.x / 2;

			Vector3 target = GetPosInCage();

			yield return new WaitForSeconds(waitTime);
			GameObject bullet = poolOfEnemyProjectiles.GetPool();

			bullet.GetComponent<Projectile>().byBoss = true;
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, target - gameObject.transform.position);
			bullet.transform.position = gameObject.transform.position;
			bullets.Add(bullet);
			bullet.SetActive(true);
			StopCoroutine(Caged(waitTime));
		}
	}
	public Vector3 GetPosInCage() {
		float x = UnityEngine.Random.Range(cage.transform.position.x - cageSize, cage.transform.position.x + cageSize);
		float y = UnityEngine.Random.Range(cage.transform.position.y - cageSize, cage.transform.position.y + cageSize);
		return new Vector3(x, y, 1);
	}
	public void ClearBullets() {
		foreach (GameObject bullet in bullets) {
			bullet.SetActive(false);
		}
		Destroy(cage.gameObject);
	}
	//


	//Dodge KillerBlocks Attack Code
	public IEnumerator ChangeDir() {
		while (Attack3) {
			yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(2, 6));
			bool headingRight;
			if (anim.GetFloat("Speed") > 0) {
				headingRight = true;
			}
			else {
				headingRight = false;
			}

			print("Now");

			if(doOnceL || doOnceR) {
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


	//Rotational "Brimstone" Attack
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

	//

	//Bouce off walls
	private void OnTriggerEnter2D(Collider2D col) {
		if (isAttacking == true && Attack1 && col.tag == "Wall" || col.tag == "BossWall") {

			if (col.name == "Wall_Horizontal") {
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

	public IEnumerator LerpPos(Vector3 start, Vector3 end) {

		float sX = start.x;
		float sY = start.y;
		float eX = end.x;
		float eY = end.y;


		for (float t = 0; t < 1; t += Time.deltaTime * 0.8f) {
			float newX = Mathf.SmoothStep(sX, eX, t);
			float newY = Mathf.SmoothStep(sY, eY, t);
			transform.position = new Vector3(newX, newY, 1);
			if (t < 1) {
				yield return null;
			}
			else {
				StopCoroutine(Atk);
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

					if(dist > 50) {
						doOnceR = true;
					}
					if (dist < 50) {
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

					if (dist > 50) {
						doOnceL = true;
					}
					if (dist < 50) {
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
					upMid = ((Vector2)transform.position + hit.point) / 2;
					topBrim.transform.position = upMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					topRect.transform.localScale = new Vector3(1, dist / topRect.sizeDelta.y, 1);

					topBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x,transform.position.y,0)));

					//topBrim.transform.rotation = Quaternion.Euler(0, 0, topBrim.transform.rotation.z);


				}
			}
			foreach (RaycastHit2D hit in right) {
				if (hit.transform.tag == "BossWall") {
					rightMid = ((Vector2)transform.position + hit.point) / 2;
					rightBrim.transform.position = rightMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					rightRect.transform.localScale = new Vector3(1, dist / rightRect.sizeDelta.y, 1);
					rightBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
					//rightBrim.transform.rotation = Quaternion.Euler(0, 0, rightBrim.transform.rotation.z);


				}
			}
			foreach (RaycastHit2D hit in down) {
				if (hit.transform.tag == "BossWall") {
					downMid = ((Vector2)transform.position + hit.point) / 2;
					bottomBrim.transform.position = downMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					bottomRect.transform.localScale = new Vector3(1, dist / bottomRect.sizeDelta.y, 1);

					bottomBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
					//bottomBrim.transform.rotation = Quaternion.Euler(0, 0, bottomBrim.transform.rotation.z);



				}
			}
			foreach (RaycastHit2D hit in left) {
				if (hit.transform.tag == "BossWall") {
					leftMid = ((Vector2)transform.position + hit.point) / 2;
					leftBrim.transform.position = leftMid;

					float dist = Vector3.Distance(transform.position, hit.point);
					leftRect.transform.localScale = new Vector3(1, dist / leftRect.sizeDelta.y, 1);

					leftBrim.transform.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0)));
					//leftBrim.transform.rotation = Quaternion.Euler(0, 0, leftBrim.transform.rotation.z);


				}
			}
		}
	}
}
