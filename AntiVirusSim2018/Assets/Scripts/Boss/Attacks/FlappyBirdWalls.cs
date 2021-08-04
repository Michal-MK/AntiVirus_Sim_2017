using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class FlappyBirdWalls : IAttackPattern {

		private GameObject boss;
		private MonoBehaviour bossBehaviour;

		private ObjectPool pool_EnemyProjectile;
		private GameObject cageObj;

		public Vector3 startPosition { get; set; }
		public bool isAttackInProgress { get; set; }

		private bool informOnce = true;

		private RectTransform arenaBackground;


		public FlappyBirdWalls(GameObject boss, Vector3 startPosition, RectTransform arenaBackground) {
			this.startPosition = startPosition;
			this.boss = boss;
			this.arenaBackground = arenaBackground;

			pool_EnemyProjectile = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE) as GameObject);
			cageObj = Resources.Load<GameObject>(PrefabNames.CAGE);
			bossBehaviour = boss.GetComponent<MonoBehaviour>();
		}

		public IEnumerator Attack() {
			isAttackInProgress = true;

			GameObject positioningCage = Object.Instantiate(cageObj, Player.Instance.transform.position, Quaternion.identity);
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(boss, startPosition, Time.deltaTime / 2, null));
			yield return new WaitForSeconds(2);
			if (informOnce) {
				informOnce = false;
				HUDisplay.Instance.DisplayInfo("Flappy Bird!!! (Press \"UpArrow\" or \"W\") to flap. ", "Press \"Up or W\" to flap.");
			}
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, (Vector2)arenaBackground.transform.position - arenaBackground.sizeDelta / 2 + new Vector2(40, 20), Time.deltaTime / 2, null));
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(Player.Instance.gameObject, (Vector2)arenaBackground.transform.position - arenaBackground.sizeDelta / 2 + new Vector2(40, 20), Time.deltaTime / 2, null));

			yield return new WaitForSeconds(2.5f);
			Player.Instance.pMovement.SetMovementMode(PlayerMovementType.FLAPPY);
			Object.Destroy(positioningCage.gameObject);
			bossBehaviour.StartCoroutine(PipeGeneration());

			yield return new WaitUntil(() => !isAttackInProgress);
			Player.Instance.pMovement.SetMovementMode(PlayerMovementType.ARROW);
		}

		public IEnumerator PipeGeneration() {

			const float pipeSpacing = 2f;
			float horizontalDistance = Mathf.Abs(Mathf.Abs(Player.Instance.transform.position.x) - Mathf.Abs(boss.transform.position.x));
			const float arriveTime = 10f;
			bool downwardsMovement = true;
			int wallCount = 9;
			float xCoord = arenaBackground.position.x + (arenaBackground.sizeDelta.x / 4);
			Vector3 posA = boss.transform.position;
			Vector3 posB = new Vector3(xCoord , arenaBackground.position.y - (arenaBackground.sizeDelta.y / 2));

			for (int i = 0; i < wallCount; i++) {
				yield return new WaitForSeconds(pipeSpacing);
				bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(boss, downwardsMovement ? posB : posA, Time.deltaTime));

				downwardsMovement = !downwardsMovement;

				float holeMid = Random.Range(-arenaBackground.sizeDelta.y / 2 + 20, arenaBackground.sizeDelta.y / 2 - 20);

				float timeAtStart = Time.timeSinceLevelLoad;
				yield return null;
				while (boss.transform.position != startPosition && boss.transform.position != posB) {

					float change = arriveTime - (Time.timeSinceLevelLoad - timeAtStart);

					if (boss.transform.position.y > holeMid + 15 || boss.transform.position.y < holeMid - 15) {
						Projectile shot = pool_EnemyProjectile.Next.GetComponent<Projectile>();
						shot.transform.position = boss.transform.position;
						shot.transform.rotation = Quaternion.Euler(0, 0, 270);
						shot.gameObject.SetActive(true);
						shot.projectileSpeed = horizontalDistance / change;
						shot.Fire();
					}
					yield return null;
				}
			}
			yield return new WaitForSeconds(arriveTime);
			isAttackInProgress = false;
		}

		public void Update() {
			//Unused
		}
	}
}
