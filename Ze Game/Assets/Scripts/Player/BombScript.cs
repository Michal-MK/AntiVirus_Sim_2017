using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BombScript : MonoBehaviour {
	public CircleCollider2D col;
	public Animator anim;
	public GameObject exp;
	public bool primed = false;
	private bool once = true;




	void Start() {

		if (gameObject.name == "Bomb") {
			anim.enabled = true;
			col.enabled = false;
			StartCoroutine(After());
		}
	}
	public void Collided(Collider2D it) {
		if (it.transform.name == "Top" || it.transform.name == "Right" || it.transform.name == "Bottom" || it.transform.name == "Left") {
			it.gameObject.SetActive(false);
		}
	}

	private IEnumerator After() {
		yield return new WaitForSeconds(1.5f);
		exp.GetComponent<CircleCollider2D>().enabled = true;
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	void Update() {

		if (primed && !once) {
			anim.enabled = true;
			col.enabled = false;
			once = false;
		}
	}
}
