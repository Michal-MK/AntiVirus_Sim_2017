using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class LoopBulletSpawn : IAttackPattern {

		private GameObject boss;
		private MonoBehaviour bossBehaviour;

		private ObjectPool pool_EnemyProjectile;

		private GameObject cageObj;
		private float cageSize;
		private Vector3 cagePosition;

		private RectTransform backgroundTransform;

		private int totalCircles;

		public bool isAttackInProgress { get; set; }
		public Vector3 startPosition { get; set; }


		public LoopBulletSpawn(GameObject boss, Vector3 startPosition, Vector3 cagePosition, RectTransform backgroundTransform) {
			this.startPosition = startPosition;
			this.boss = boss;
			this.cagePosition = cagePosition;
			this.backgroundTransform = backgroundTransform;

			totalCircles = 2;

			pool_EnemyProjectile = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE) as GameObject);
			cageObj = Resources.Load<GameObject>(PrefabNames.CAGE);
			bossBehaviour = boss.GetComponent<MonoBehaviour>();
		}


		public IEnumerator Attack() {
			isAttackInProgress = true;
			M_Player.player.pMovement.movementMethod.movementSpeed = 10;
			GameObject positioningCage = Object.Instantiate(cageObj, M_Player.player.transform.position, Quaternion.identity);
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, cagePosition, Time.deltaTime / 2, null));
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, cagePosition, Time.deltaTime / 2, null));

			yield return new WaitForSeconds(3);
			Canvas_Renderer.script.DisplayInfo(null, "Don't forget about the zooming feature :]");

			bossBehaviour.StartCoroutine(Caged(positioningCage.gameObject, 1.1f));
			SpriteRenderer arenaSprite = backgroundTransform.GetComponent<SpriteRenderer>();
			Vector3[] positions = new[]{
				SpriteOffsets.GetPoint(arenaSprite,12,12),
				SpriteOffsets.GetPoint(arenaSprite,88,12),
				SpriteOffsets.GetPoint(arenaSprite,88,88),
				SpriteOffsets.GetPoint(arenaSprite,12,88),
			};

			float timeToCover = 5;
			for (int i = 0; i < totalCircles; i++) {
				bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(boss, positions, timeToCover));
				yield return new WaitForSeconds(timeToCover * positions.Length);
			}
			isAttackInProgress = false;
			M_Player.player.pMovement.movementMethod.movementSpeed = 50;
			yield return new WaitForSeconds(2);
			Object.Destroy(positioningCage.gameObject);
		}

		public IEnumerator Caged(GameObject cage, float waitTime) {
			cageSize = cage.GetComponent<RectTransform>().sizeDelta.x / 2;
			while (isAttackInProgress) {
				Vector3 target = GetPosInCage(cage);
				yield return new WaitForSeconds(waitTime);
				Projectile bullet = pool_EnemyProjectile.getNext.GetComponent<Projectile>();
				bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, target - boss.transform.position);
				bullet.transform.position = boss.transform.position;
				bullet.gameObject.SetActive(true);
				bullet.projectileSpeed = 15;
				bullet.Fire(1);
				waitTime -= waitTime * 0.005f;
			}
			pool_EnemyProjectile.ClearPool();
		}

		public Vector3 GetPosInCage(GameObject positioningCage) {
			float x = Random.Range(positioningCage.transform.position.x - cageSize, positioningCage.transform.position.x + cageSize);
			float y = Random.Range(positioningCage.transform.position.y - cageSize, positioningCage.transform.position.y + cageSize);
			return new Vector3(x, y, 1);
		}

		public void Update() { /* UNUSED */ }
	}
}
