using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public interface IAttackPattern {
		IEnumerator Attack();

		bool isAttackInProgress { get; }
		Vector3 startPosition { get; }

		void Update();
	}
}
