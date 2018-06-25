using UnityEngine;
using System.Collections;

public class FollowScript : MonoBehaviour {
	public Transform targetToFollow;

	public float driftCorrection = 10;
	public float maxforce = 1.4f;
	public float steerModifier = 1.2f;
	public float succtionForce = 8;
	private Vector3 velocity;
	private Vector3 acceleration;

	private const float DEFAULT_SUCCTION_FORCE = 8f;

	void FixedUpdate() {
		Behaviours();
		transform.position += velocity;
		velocity += acceleration * driftCorrection * 0.1f;
		acceleration = Vector3.zero;
	}

	private void ApplyForce(Vector3 force) {
		acceleration += force;
	}

	private void Behaviours() {
		Vector3 seek = Seek(targetToFollow);
		seek = Vector3.ClampMagnitude(seek, driftCorrection);
		ApplyForce(seek);
	}

	private Vector3 Seek(Transform target) {
		Vector3 desired = target.position - transform.position;
		desired.Normalize();
		float d = Vector3.Distance(target.position, transform.position);
		if (d > 20) {
			desired *= ValueMapping.MapFloat(d, 0, 50, 0, maxforce);
			if(succtionForce == 1) {
				succtionForce = DEFAULT_SUCCTION_FORCE * 0.5f;
			}
		}
		else if (d > 2) {
			desired *= succtionForce / Vector3.Distance(target.position, transform.position);
		}
		else {
			succtionForce /= succtionForce;
			desired *= ValueMapping.MapFloat(d, 0, 50, 0, maxforce);
		}
		Vector3 steer = (desired - velocity) * steerModifier;
		return steer * Time.fixedDeltaTime;
	}

	//Gravity like
	//private Vector3 Seek(Transform target) {
	//	Vector3 desired = target.position - transform.position;
	//	desired.Normalize();
	//	desired *= maxSpeed / Vector3.Distance(target.position, transform.position);
	//	Vector3 steer = desired - (velocity * maxforce);
	//	return steer * Time.deltaTime;
	//}
}
