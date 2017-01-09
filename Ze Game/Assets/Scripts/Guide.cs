using UnityEngine;
using System.Collections;


public class Guide : MonoBehaviour {

	public Spike spike;
	public M_Player player;
	private Vector3 spikepos;
	private Vector3 playerpos;
	public GameObject Arrow;
	GameObject pointArrow;
	Transform GuideObj;


	void Start(){
		
		GuideObj = GameObject.Find ("Guide").transform;
	}


	void Recalculate () {

		Destroy (pointArrow);

		spikepos = new Vector3 (spike.transform.position.x, spike.transform.position.y, 0);
		playerpos = new Vector3 (player.transform.position.x, player.transform.position.y, 0);


		pointArrow = (GameObject)Instantiate (Arrow, new Vector3 (0,-1,0), Quaternion.FromToRotation (Vector3.up, (spikepos-playerpos)));
		pointArrow.transform.SetParent(GuideObj);

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
			float r = Mathf.Clamp(Vector3.Distance (playerpos, spikepos),5,10);



			Debug.DrawRay (new Vector3 (m, n, 0), (Vector3)PlayToSpike);
	
			spikepos = new Vector3 (spike.transform.position.x, spike.transform.position.y, 0);
			playerpos = new Vector3 (player.transform.position.x, player.transform.position.y, 0);

			pointArrow.transform.rotation = Quaternion.FromToRotation (Vector3.up, (spikepos - playerpos));

			if (spikepos.x - m > 0){

				float X = 1 / (2 * (a * a + b * b)) * (Mathf.Sqrt (Mathf.Pow (2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * r * r + 2 * b * c * n + c * c)) - 2 * a * b * n - 2 * a * c + 2 * b * b * m);

				float Yline;
				float Ycirc;

				Yline = (- X*a - c ) / b;

				float kinda_r = Mathf.Sqrt(Mathf.Pow ((X - m), 2) + Mathf.Pow ((Yline - n), 2));

				if (kinda_r > r - 0.1f && kinda_r < r + 0.1f) {
					pointArrow.transform.position = new Vector3 (X, Yline, 0);
					return;
				} 
				else
				{
//					Debug.Log (kinda_r);
//					Debug.Log ("X> " + (X-m));
//					Debug.Log ("Y> " + (Yline - n));
					if (spikepos.y - n > 0) {	
						Ycirc = n + Mathf.Sqrt (-Mathf.Pow (m, 2) + 2 * m * X + Mathf.Pow (r, 2) - Mathf.Pow (X, 2));
					} 
					else if (spikepos.y - n < 0) {
						Ycirc = n - Mathf.Sqrt (-Mathf.Pow (m, 2) + 2 * m * X + Mathf.Pow (r, 2) - Mathf.Pow (X, 2));
					} 
					else {
						if (PlayToSpike.y > 0) {
							Ycirc = r;
						} 
						else {
							Ycirc = -r;
						}
					}
				}
				kinda_r = (Mathf.Pow ((X - m), 2)) + (Mathf.Pow ((Ycirc - n), 2));

				if (kinda_r > Mathf.Pow (r,2) - 0.1f && kinda_r < Mathf.Pow (r,2) + 0.1f)  {
					pointArrow.transform.position = new Vector3 (X, Ycirc, 0);
				} 
				else {
					if (PlayToSpike.y > 0) {
						Ycirc = r + n;
					} 
					else {
						Ycirc = -r + n;
					}
					X = m;
					pointArrow.transform.position = new Vector3 (X, Ycirc, 0);
				}




			} else if (spikepos.x - m < 0) {
				float X = 1 / (2 * (a * a + b * b)) * (-Mathf.Sqrt (Mathf.Pow (2 * a * b * n + 2 * a * c - 2 * b * b * m, 2) - 4 * (a * a + b * b) * (b * b * m * m + b * b * n * n - b * b * r * r + 2 * b * c * n + c * c)) - 2 * a * b * n - 2 * a * c + 2 * b * b * m);


				float Yline;
				float Ycirc;

				Yline = (- X*a - c ) / b;

				float kinda_r = Mathf.Sqrt(Mathf.Pow ((X - m), 2) + Mathf.Pow ((Yline - n), 2));

				if (kinda_r > r - 0.1f && kinda_r < r + 0.1f) {
					pointArrow.transform.position = new Vector3 (X, Yline, 0);
					return;
				} 
				else
				{
//					Debug.Log (kinda_r);
//					Debug.Log ("X> " + (X-m));
//					Debug.Log ("Y> " + (Yline - n));
					if (spikepos.y - n > 0) {	
						Ycirc = n + Mathf.Sqrt (-Mathf.Pow (m, 2) + 2 * m * X + Mathf.Pow (r, 2) - Mathf.Pow (X, 2));
					} 
					else if (spikepos.y - n < 0) {
						Ycirc = n - Mathf.Sqrt (-Mathf.Pow (m, 2) + 2 * m * X + Mathf.Pow (r, 2) - Mathf.Pow (X, 2));
					} 
					else {
						if (PlayToSpike.y > 0) {
							Ycirc = r;
						} 
						else {
							Ycirc = -r;
						}
					}
				}
				kinda_r = (Mathf.Pow ((X - m), 2)) + (Mathf.Pow ((Ycirc - n), 2));

				if (kinda_r > Mathf.Pow (r,2) - 0.1f && kinda_r < Mathf.Pow (r,2) + 0.1f)  {
					pointArrow.transform.position = new Vector3 (X, Ycirc, 0);
				} 
				else {
					if (PlayToSpike.y > 0) {
						Ycirc = r + n;
					} 
					else {
						Ycirc = -r + n;
					}
					X = m;
					pointArrow.transform.position = new Vector3 (X, Ycirc, 0);
				}
			}
		}
	}
	public void disableGuide(){
		gameObject.SetActive (false);
	}
}