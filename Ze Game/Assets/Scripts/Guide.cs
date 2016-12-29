using UnityEngine;
using System.Collections;


public class Guide : MonoBehaviour {

	public Spike spike;
	public M_Player player;
	private Vector3 spikepos;
	private Vector3 playerpos;
	public GameObject Arrow;
	GameObject pointArrow;
	float r;


	void init () {

		

		spikepos = new Vector3 (spike.transform.position.x, spike.transform.position.y, 0);
		playerpos = new Vector3 (player.transform.position.x, player.transform.position.y, 0);

		Destroy (pointArrow);

		pointArrow = (GameObject)Instantiate (Arrow, new Vector3 (0,-1,0), Quaternion.FromToRotation (Vector3.up, (spikepos-playerpos)));
		pointArrow.transform.SetParent(transform.parent);

		}

	void Update() {


		if (pointArrow != null && timer.run == true) {
			Vector2 PlayToSpike =  (Vector2) spike.transform.position - (Vector2)player.transform.position;
			Vector2 normVec = new Vector2 (PlayToSpike.y, -PlayToSpike.x);

			float a = normVec.x;
			float b = normVec.y;
			float m = player.transform.position.x;
			float n = player.transform.position.y;
			float c = -a * m - b * n;
			float distance = Mathf.Clamp(Vector3.Distance (playerpos, spikepos),5,10);
			r = distance;

			Debug.DrawRay (new Vector3 (m, n, 0), (Vector3)PlayToSpike);
	
			spikepos = new Vector3 (spike.transform.position.x, spike.transform.position.y, 0);
			playerpos = new Vector3 (player.transform.position.x, player.transform.position.y, 0);

			pointArrow.transform.rotation = Quaternion.FromToRotation (Vector3.up, (spikepos - playerpos));


			if (spikepos.x - player.transform.position.x > 0){

				float X = 1 / (2 * (a * a + b * b)) * (Mathf.Sqrt (Mathf.Pow (2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * r * r + 2 * b * c * n + c * c)) - 2 * a * b * n - 2 * a * c + 2 * b * b * m);

				while (float.IsNaN (X) == false) {
					float newX = X;
					float newY = (-a * X - c) / b;
				
					pointArrow.transform.position = new Vector3 (newX, newY, 0);
					break;
				}
					
			} else if (spikepos.x - player.transform.position.x < 0) {
				float X = 1 / (2 * (a * a + b * b)) * (-Mathf.Sqrt (Mathf.Pow (2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * r * r + 2 * b * c * n + c * c)) - 2 * a * b * n - 2 * a * c + 2 * b * b * m);

				while (float.IsNaN (X) == false) {
					float newX = X;
					float newY = (-a * X - c) / b;

					pointArrow.transform.position = new Vector3 (newX, newY, 0);
					break;
				}
			}
		}
	}
}