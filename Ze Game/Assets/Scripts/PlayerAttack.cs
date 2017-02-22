using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public GameObject spikeBullet;
	public GameObject player;
	public Spike spike;
	public Vector2 mousepos;
	public ObjectPooler pool;

	public bool fireMode = false;
	int bullets;


	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)) {
			fireMode = true;
			M_Player.doNotMove = true;
			Cursor.visible = true;
		}
		else if (Input.GetKeyUp(KeyCode.Space)) {
			fireMode = false;
			M_Player.doNotMove = false;
			Cursor.visible = false;
		}

		if (fireMode) {
			//float roll = Input.GetAxis("Mouse ScrollWheel");
			if (Input.GetMouseButtonDown(0)) {

				//bullets = Spike.spikesCollected;
				bullets = Spike.spikesCollected;
				if (bullets > 0) {
					FireSpike();
				}
				else {
					print("Out of bullets!");
				}
			}
		}
	}
	public void FireSpike() {
		
		Vector3 playpos = player.transform.position;
		mousepos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);

		GameObject bullet = pool.GetPool();

		
		bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)mousepos - player.transform.position);
		bullet.transform.position = player.transform.position - (bullet.transform.rotation  * Vector2.down * 2);

		bullet.transform.parent = GameObject.Find("Collectibles").transform;
		bullet.SetActive(true);

		bullets--;



	}
}
