using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class LoopBulletSpawn : IAttackPattern {

		private ObjectPool pool_EnemyProjectile;
		private GameObject boss;
		private MonoBehaviour bossBehaviour;

		private float cageSize;

		public Vector3 startPosition { get; set; }

		private Vector3 cagePosition;

		private int totalCircles;

		public GameObject cageObj;

		public LoopBulletSpawn(GameObject boss, Vector3 start, Vector3 cagePosition) {
			totalCircles = 2;
			pool_EnemyProjectile = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_INACCUARATE) as GameObject);
			startPosition = start;
			this.boss = boss;
			this.cagePosition = cagePosition;
			bossBehaviour = boss.GetComponent<MonoBehaviour>();
		}

		public bool isAttackInProgress { get; set; }

		public IEnumerator Attack() {
			isAttackInProgress = true;
			MoveScript positioningCage = Object.Instantiate(cageObj, M_Player.player.transform.position, Quaternion.identity).GetComponent<MoveScript>();
			yield return new WaitForSeconds(2);
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, cagePosition, Time.deltaTime / 2, null));
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, cagePosition, Time.deltaTime / 2, null));
			positioningCage.destroy = true;
			yield return new WaitForSeconds(3);
			Canvas_Renderer.script.DisplayInfo(null, "Don't forget about the zooming feature :]");

			bossBehaviour.StartCoroutine(Caged(positioningCage.gameObject, 1.1f));
			for (int i = 0; i <= totalCircles; i++) {
				yield return new WaitForSeconds(15f);
			}
			yield return new WaitForSeconds(2f);
			Object.Destroy(positioningCage.gameObject);
			isAttackInProgress = false;
		}

		public IEnumerator Caged(GameObject cage, float waitTime) {
			while (isAttackInProgress) {
				cageSize = cage.GetComponent<RectTransform>().sizeDelta.x / 2;
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
