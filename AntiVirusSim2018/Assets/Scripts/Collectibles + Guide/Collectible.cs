using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	public bool includeNonTriggerCollisions;
	public Transform objectToCheckCollisionWith;

	private void OnTriggerEnter2D(Collider2D collision) {
		if(objectToCheckCollisionWith == collision.transform) {
			objectToCheckCollisionWith.SendMessage("Collided", transform, SendMessageOptions.RequireReceiver);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (includeNonTriggerCollisions && objectToCheckCollisionWith == collision.transform) {
			objectToCheckCollisionWith.SendMessage("Collided", transform, SendMessageOptions.RequireReceiver);
		}
	}
}
