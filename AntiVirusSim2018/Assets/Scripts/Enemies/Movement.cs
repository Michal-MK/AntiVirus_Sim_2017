using Igor.Constants.Strings;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Movement : MonoBehaviour {
	public Vector3 direction;
	public bool selfDestruct;

	public float selfDestructDelay;

	public bool isPooled { get; set; }

	private bool destroyOnBackgroundEnter;
	private bool destroyOnWallEnter;
	private bool destroyOnBackgroundLeave;
	private bool destroyOnWallLeave;

	#region Movement & Destroy calls

	public void MoveAndDestroyOnWallLeave() {
		destroyOnWallLeave = true;
		selfDestruct = false;
		isPooled = GetComponent<IPoolable>() == null ? false : GetComponent<IPoolable>().IsPooled;
		Move();
	}

	public void MoveAndDestroyOnBackgroundLeave() {
		destroyOnBackgroundLeave = true;
		selfDestruct = false;
		isPooled = GetComponent<IPoolable>() == null ? false : GetComponent<IPoolable>().IsPooled;
		Move();
	}

	public void MoveAndDestroyOnWallEnter() {
		destroyOnWallEnter = true;
		selfDestruct = false;
		isPooled = GetComponent<IPoolable>() == null ? false : GetComponent<IPoolable>().IsPooled;
		Move();
	}

	public void MoveAndDestroyOnBackgroundEnter() {
		destroyOnBackgroundEnter = true;
		selfDestruct = false;
		isPooled = GetComponent<IPoolable>() == null ? false : GetComponent<IPoolable>().IsPooled;
		Move();
	}

	#endregion

	public void Move() {
		GetComponent<Rigidbody2D>().velocity = direction;
		if (selfDestruct) {
			StartCoroutine(SelfDestruct(selfDestructDelay));
		}
	}

	private IEnumerator SelfDestruct(float selfDestructDelay) {
		yield return new WaitForSeconds(selfDestructDelay);
		if (isPooled) {
			gameObject.SetActive(false);
		}
		else {
			Destroy(gameObject);
		}
	}

	private void Conditions(string tag, bool enter) {
		if (enter) {
			if (destroyOnBackgroundEnter && tag == Tags.BACKGROUND) {
				Destroy();
			}
			if (destroyOnWallEnter && tag == Tags.WALL) {
				Destroy();
			}
		}
		else {
			if (destroyOnBackgroundLeave && tag == Tags.BACKGROUND) {
				Destroy();
			}
			if (destroyOnWallLeave && tag == Tags.WALL) {
				Destroy();
			}
		}
	}

	#region Collision Checkers

	private void OnTriggerEnter2D(Collider2D collision) {
		Conditions(collision.tag, true);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		Conditions(collision.tag, false);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		Conditions(collision.transform.tag, true);
	}

	private void OnCollisionExit2D(Collision2D collision) {
		Conditions(collision.transform.tag, false);
	}

	#endregion

	private void Destroy() {
		if (isPooled) {
			gameObject.SetActive(false);
		}
		else {
			Destroy(gameObject);
		}
	}
}

