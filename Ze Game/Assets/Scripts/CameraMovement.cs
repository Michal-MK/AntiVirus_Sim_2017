using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public Transform player;
	public GameObject spike;
	public Vector3 cam_pos;
	Camera thisone; 
	public Vector2 maxValues;
	public Vector2 minValues;
	public Transform referencePointMax;
	public Transform referencePointMin;

	void Start(){
		Cursor.visible = false;

		thisone = this.GetComponent<Camera> ();
//		Debug.Log (thisone.aspect  * thisone.orthographicSize);
		maxValues.y = referencePointMax.position.y - thisone.orthographicSize;
		minValues.y = referencePointMin.position.y + thisone.orthographicSize;
		maxValues.x = referencePointMax.position.x - thisone.aspect  * thisone.orthographicSize;
		minValues.x = referencePointMin.position.x + thisone.aspect  * thisone.orthographicSize;
	}

	void FixedUpdate(){

			cam_pos = new Vector3 (determineX (), determineY (), player.position.z - 10);
			gameObject.transform.position = cam_pos;


	}
	public float determineX () {
		if (player.position.x > maxValues.x || player.position.x < minValues.x){
		return transform.position.x;
		}
		else {
		return player.position.x;
		}
	}

	public float determineY () {
		if (player.position.y > maxValues.y || player.position.y < minValues.y) {
			return transform.position.y;
		}
		else {
			return player.position.y;
		}
	}
}