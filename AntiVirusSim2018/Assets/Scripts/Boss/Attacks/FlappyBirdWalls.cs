using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class FlappyBirdWalls : IAttackPattern {

		private GameObject boss;
		private MonoBehaviour bossBehaviour;

		private ObjectPool pool_EnemyProjectile;

		public Vector3 startPosition { get; set; }

		public bool isAttackInProgress { get; set; }

		private bool informOnce = true;
		private bool isGeneratingPipes = true;

		private GameObject cageObj;

		RectTransform arenaBackground { get; }


		public FlappyBirdWalls(GameObject boss, Vector3 start, RectTransform arenaBackground) {
			pool_EnemyProjectile = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_INACCUARATE) as GameObject);
			startPosition = start;
			this.boss = boss;
			cageObj = Resources.Load<GameObject>(PrefabNames.CAGE); //TODO
			bossBehaviour = boss.GetComponent<MonoBehaviour>();
		}

		public IEnumerator Attack() {
			isAttackInProgress = true;
			isGeneratingPipes = true;

			MoveScript positioningCage = Object.Instantiate(cageObj, M_Player.player.transform.position, Quaternion.identity).GetComponent<MoveScript>();
			yield return new WaitForSeconds(2);
			if (informOnce) {
				informOnce = false;
				Canvas_Renderer.script.DisplayInfo("Flappy Bird!!! (Press \"UpArrow\" or \"W\") to flap. ", "Press \"Up or W\" to flap.");
			}
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, (Vector2)arenaBackground.transform.position - arenaBackground.sizeDelta / 2 + new Vector2(40, 20), Time.deltaTime / 2, null));
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, (Vector2)arenaBackground.transform.position - arenaBackground.sizeDelta / 2 + new Vector2(40, 20), Time.deltaTime / 2, null));
			positioningCage.destroy = true;

			yield return new WaitForSeconds(2.5f);
			M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.FLAPPY);
			Object.Destroy(positioningCage.gameObject);
			bossBehaviour.StartCoroutine(PipeGeneration());

			yield return new WaitUntil(() => !isGeneratingPipes);

			M_Player.player.pMovement.SetMovementMode(Player_Movement.PlayerMovement.ARROW);
			//Attack5 = false;
			isGeneratingPipes = false;
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(boss, arenaBackground.transform.position + new Vector3(arenaBackground.sizeDelta.x / 2 - 140, 0, 0), Time.deltaTime / 2, null));
			isAttackInProgress = false;
		}

		public IEnumerator PipeGeneration() {

			const float pipeSpacing = 2f;
			float horizontalDistance = Mathf.Abs(Mathf.Abs(M_Player.player.transform.position.x) - Mathf.Abs(boss.transform.position.x));
			float arriveTime = 10f;
			bool downwardsMovement = true;
			int totalWallsGenerated = 9;
			Vector3 oppositePosition = new Vector3(arenaBackground.position.x + (arenaBackground.sizeDelta.x / 4), arenaBackground.position.y - (arenaBackground.sizeDelta.y / 2));

			for (int i = 0; i < totalWallsGenerated; i++) {
				yield return new WaitForSeconds(pipeSpacing);
				bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(boss,
														  new Vector3(arenaBackground.position.x + (arenaBackground.sizeDelta.x / 4),
														  downwardsMovement ? arenaBackground.position.y - (arenaBackground.sizeDelta.y / 2) : arenaBackground.position.y + (arenaBackground.sizeDelta.y / 2)),
														  Time.deltaTime, null));
				downwardsMovement = !downwardsMovement;

				float holeMid = Random.Range(-arenaBackground.sizeDelta.y / 2 + 20, arenaBackground.sizeDelta.y / 2 - 20);

				float timeAtStart = Time.timeSinceLevelLoad;
				while (boss.transform.position != startPosition && boss.transform.position != oppositePosition) {

					float change = arriveTime - (Time.timeSinceLevelLoad - timeAtStart);

					if (boss.transform.position.y > holeMid + 15 || boss.transform.position.y < holeMid - 15) {
						Projectile shot = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
						shot.transform.position = boss.transform.position;
						shot.transform.rotation = Quaternion.Euler(0, 0, 270);
						shot.gameObject.SetActive(true);
						shot.projectileSpeed = horizontalDistance / change;
						shot.Fire();
						shot.StartCoroutine(shot.Kill(arriveTime + 2));
					}
					yield return new WaitForSeconds(0.02f);
				}
			}
			yield return new WaitForSeconds(arriveTime);
			isGeneratingPipes = false;
		}

		public void Update() {
			throw new System.NotImplementedException();
		}
	}
}
