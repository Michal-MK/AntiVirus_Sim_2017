using System.Collections;
using UnityEngine;

public class CageScript : MonoBehaviour { 
	public bool doneMoving = false;
	public GameObject player;
	public Rigidbody2D rg;



	public void MoveUp() {
		StartCoroutine(Selfdestruct());
		rg.velocity = new Vector2(0, 20);
	}
	private IEnumerator Selfdestruct() {
		yield return new WaitForSeconds(5.5f);
		Destroy(gameObject);

	}
}