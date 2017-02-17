using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {




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
	public ObjectPooler pool;


	GameObject player;
	RectTransform BG;
	public Rigidbody2D rigid;

	public bool isHit = false;
	public bool isAttacking = false;

	private bool Attack1 = false;
	private bool Attack2 = false;
	private bool Attack3 = false;
	private bool Attack4 = false;
	private bool Attack5 = false;


	public float IFrames = 2;
	//public float WaitTime;
	public float speed = 100;
	public float animFrame = 0;


	public int attackNo;
	public int fullCircle;

	public Vector2 oldvec;
	public Vector2 calculatedVec;




	void Start() {
		BG = GameObject.Find("Background_room_Boss_1").GetComponent<RectTransform>();
		player = GameObject.FindGameObjectWithTag("Player");
		rigid = gameObject.GetComponent<Rigidbody2D>();
		StartCoroutine(Attacks(2));
		fullCircle = 2;
	}

	public int ChooseAttack() {
		int previous = attackNo;

		while (previous == attackNo) {
			attackNo = Random.Range(1, 6);
		}
		return attackNo;
	}


	float initX = 1;
	float initY = 1;

	GameObject cage;
	Coroutine mycor;

	public IEnumerator Attacks(int attack) {
		yield return new WaitForSecondsRealtime(10);

		switch (attack) {
			//Bouncing Attack
			case 1:
			isAttacking = true;
			Attack1 = true;

			anim.Play("Attack" + attack);

			gameObject.transform.position = BG.position;

			calculatedVec = new Vector2(initX, initY);
			oldvec = calculatedVec;

			yield return new WaitUntil(() => animFrame == 1800);

			anim.SetTrigger("Attack" + attack);

			isAttacking = false;
			Attack1 = false;

			StopCoroutine(Attacks(attack));

			break;



			case 2:
			isAttacking = true;
			Attack2 = true;

			print(attack);

			gameObject.transform.position = new Vector3(-520, -70, 0);
			player.transform.position = BG.transform.position;
			cage = Instantiate(cageObj,BG.transform.position, Quaternion.identity);
			float waitTime = 1.1f;

			for (int i = 0; i <= fullCircle; i++) {

				float newWait = waitTime - 0.1f;
				waitTime = newWait;

				anim.Play("Attack" + attack);
				mycor = StartCoroutine(Caged(newWait));

				yield return new WaitUntil(() => animFrame == 871);

				anim.SetTrigger("Attack" + attack);

				yield return new WaitForSeconds(0.02f);

				StopCoroutine(mycor);


			}

			anim.SetTrigger("Attack" + attack);

			isAttacking = false;
			Attack2 = false;

			StopCoroutine(mycor);

			yield return new WaitForSeconds(1.5f);

			ClearBullets();
			StopCoroutine(Attacks(attack));

			break;



			case 3:
			isAttacking = true;
			print(attack);
			anim.Play("Attack" + attack);



			yield return new WaitForSeconds(10);
			isAttacking = false;
			StopCoroutine(Attacks(attack));
			break;



			//case 4:
			//isAttacking = true;
			//print(attack);
			//anim.Play("Attack" + attack);



			//yield return new WaitForSeconds(WaitTime);
			//isAttacking = false;
			//StopCoroutine(Attacks(attack));
			//break;



			//case 5:
			//isAttacking = true;
			//print(attack);
			//anim.Play("Attack" + attack);



			//yield return new WaitForSeconds(WaitTime);
			//isAttacking = false;
			//StopCoroutine(Attacks(attack));
			//break;

		}
	}

	//Bouncing Attack Code
	public Vector2 AddABitOfRandomness(Vector2 current, bool horisontal) {
		float randPos = Random.Range(0, 1);
		float randNeg = Random.Range(-1, 0);

		if (horisontal) {

			print("Current is: " + current);
			Vector2 directed = new Vector2(current.x, current.y * -1);
			print("Directed is: " + directed);

			Vector2 changed = new Vector2(directed.x + randPos, directed.y + randNeg);
			print("Changed is: " + changed);
			oldvec = changed;
			Debug.DrawLine(gameObject.transform.position, changed * 10, Color.red, 10);
			return changed.normalized;
		}
		else {
			print("Current is: " + current);
			Vector2 directed = new Vector2(current.x * -1, current.y);

			print("Directed is: " + directed);

			Vector2 changed = new Vector2(directed.x + randNeg, directed.y + randPos);
			print("Changed is: " + changed);
			oldvec = changed;
			Debug.DrawLine(gameObject.transform.position, changed * 10, Color.red, 10);
			return changed.normalized;
		}
	}

	//Caged Attack Code
	public List<GameObject> bullets = new List<GameObject>();
	float cageSize;
	public IEnumerator Caged(float waitTime) {

		while (Attack2) {
			cageSize = cage.GetComponent<RectTransform>().sizeDelta.x / 2;

			Vector3 target = GetPosInCage();

			yield return new WaitForSeconds(waitTime);
			GameObject bullet = pool.GetPool();

			bullet.GetComponent<Projectile>().byBoss = true;
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, target - gameObject.transform.position);
			bullet.transform.position = gameObject.transform.position;
			bullets.Add(bullet);
			bullet.SetActive(true);
			StopCoroutine(Caged(waitTime));
		}
	}
	public Vector3 GetPosInCage() {
		float x = Random.Range(cage.transform.position.x - cageSize, cage.transform.position.x + cageSize);
		float y = Random.Range(cage.transform.position.y - cageSize, cage.transform.position.y + cageSize);
		return new Vector3(x, y, 1);
	}
	public void ClearBullets() {
		foreach (GameObject bullet in bullets) {
			bullet.SetActive(false);
		}
		Destroy(cage.gameObject);
	}









	private void OnTriggerEnter2D(Collider2D col) {
		if (isAttacking == true || col.tag == "Wall") {

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
	private void Update() {
		if (Attack1 == true) {
			rigid.velocity = calculatedVec * speed;
		}
	}
}
