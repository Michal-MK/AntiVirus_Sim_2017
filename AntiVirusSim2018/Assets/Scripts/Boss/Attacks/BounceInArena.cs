using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class BounceInArena : IAttackPattern {

		public bool isAttackInProgress { get; set; }

		public Vector3 startPosition { get; set; }

		private PhysicsMaterial2D bouncyMaterial;
		private PhysicsMaterial2D standardMaterial;

		private int bounces;

		public BounceInArena(GameObject boss, Vector3 start) {
			startPosition = start;
		}

		public IEnumerator Attack() {
			yield return null;
			/* TODO
			 isAttackInProgress = true;
			selfRigid.velocity = Random.insideUnitCircle;
			while (selfRigid.velocity.magnitude < 250) {
				selfRigid.velocity += selfRigid.velocity.normalized * 5;
				yield return null;
			}

			yield return new WaitUntil(() => bounces >= 20);
			selfRigid.drag = 3;
			yield return new WaitUntil(() => selfRigid.velocity == Vector2.zero);
			selfRigid.drag = 0;
			bounces = 0;
			isAttackInProgress = false;
			*/
		}

		public void Update() {
			throw new System.NotImplementedException();
		}
	}
}
