using System.Collections;
using UnityEngine;

public class CageScript : MonoBehaviour { 
	public bool doneMoving = false;
	public GameObject player;
	public Rigidbody2D rg;
	public Vector2 movVelocity = new Vector2(0, 20);


	public void MoveUp() {
		StartCoroutine(Selfdestruct(5.5f));
		rg.velocity = movVelocity;
	}
	private IEnumerator Selfdestruct(float timeRemaining) {
		yield return new WaitForSeconds(timeRemaining);
		Destroy(gameObject);
	}
}