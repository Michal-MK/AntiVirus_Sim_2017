using UnityEngine;

public class Collectible : MonoBehaviour {
	[SerializeField]
	private bool includeNonTriggerCollisions = false;

	[SerializeField]
	private Transform objectToCheckCollisionWith = null;
	/// <summary>
	/// Who can collect this <see cref="Collectible"/>
	/// </summary>
	public Transform Collector { get => objectToCheckCollisionWith; set { objectToCheckCollisionWith = value; } }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (objectToCheckCollisionWith == collision.transform) {
			objectToCheckCollisionWith.SendMessage("Collided", transform, SendMessageOptions.RequireReceiver);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (includeNonTriggerCollisions && objectToCheckCollisionWith == collision.transform) {
			objectToCheckCollisionWith.SendMessage("Collided", transform, SendMessageOptions.RequireReceiver);
		}
	}
}
