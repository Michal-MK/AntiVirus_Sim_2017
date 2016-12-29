using UnityEngine;
using System.Collections;

public class WRay : MonoBehaviour {



	public void RaycastH () {

		Vector2 button = this.gameObject.transform.position;
		
		RaycastHit2D[] result = Physics2D.RaycastAll (button, Vector2.right,100);
		Debug.DrawRay (button, Vector2.right * 100, Color.red);

		for (int i = 0; i < 10; i++){

				Debug.Log (result);
			
			}
		}

	public void RaycastV () {

		Vector2 button = this.gameObject.transform.position;
	
		RaycastHit2D[] result = Physics2D.RaycastAll (button, Vector2.down,100);
		Debug.DrawRay (button, Vector2.down * 100, Color.red);

		for (int i = 0; i <= 10; i++){

				Debug.Log (result);

		}
	}
}
