using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class LaserSpin : IAttackPattern {

		private RectTransform topBrim;
		private RectTransform rightBrim;
		private RectTransform bottomBrim;
		private RectTransform leftBrim;

		private float zRotation = 0;
		private float rotationDelta = 0.1f;

		private GameObject brimstonePrefab;
		private GameObject boss;
		private MonoBehaviour bossBehaviour;
		private RectTransform arenaBackground;

		public Vector3 startPosition { get; set; }

		public bool isAttackInProgress { get; set; }

		public LaserSpin(GameObject boss, Vector3 startPosition, RectTransform arenaBackground) {
			this.startPosition = startPosition;
			this.boss = boss;
			this.arenaBackground = arenaBackground;

			bossBehaviour = boss.GetComponent<MonoBehaviour>();
			brimstonePrefab = Resources.Load<GameObject>(PrefabNames.BOSS_BRIMSTONE);
		}

		public IEnumerator Attack() {

			boss.GetComponent<ParticleSystem>().Emit(100);
			yield return new WaitForSeconds(2);

			SpawnBrim(ref topBrim, "Top");
			SpawnBrim(ref rightBrim, "Right");
			SpawnBrim(ref bottomBrim, "Bottom");
			SpawnBrim(ref leftBrim, "Left");

			isAttackInProgress = true;
			bossBehaviour.StartCoroutine(VariedRotation());

			yield return new WaitForSeconds(35);

			topBrim.gameObject.SetActive(false);
			rightBrim.gameObject.SetActive(false);
			bottomBrim.gameObject.SetActive(false);
			leftBrim.gameObject.SetActive(false);
			boss.transform.rotation = Quaternion.identity;
			bossBehaviour.StopCoroutine(VariedRotation());

			isAttackInProgress = false;
		}

		private void SpawnBrim(ref RectTransform brimRef, string name) {
			brimRef = Object.Instantiate(brimstonePrefab, boss.transform).GetComponent<RectTransform>();
			brimRef.transform.localPosition = Vector3.zero;
			brimRef.name = name;
		}

		public IEnumerator VariedRotation() {
			rotationDelta = 0.1f;
			zRotation = 0;
			while (isAttackInProgress) {
				yield return new WaitForSeconds(Random.Range(2, 4));
				rotationDelta = Chance.Half() ? Random.Range(0.4f, 1f) : Random.Range(-1, -0.4f);
			}
		}

		public void Update() {
			zRotation += rotationDelta;
			boss.transform.rotation = Quaternion.Euler(0, 0, zRotation);

			LayerMask mask = LayerMask.GetMask(Layers.WALLS);

			RaycastHit2D up = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.up, arenaBackground.sizeDelta.x * 2, mask.value);
			RaycastHit2D right = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.right, arenaBackground.sizeDelta.x * 2, mask.value);
			RaycastHit2D bottom = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.down, arenaBackground.sizeDelta.x * 2, mask.value);
			RaycastHit2D left = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.left, arenaBackground.sizeDelta.x * 2, mask.value);

			UpdatePosition(topBrim, up.point);
			UpdatePosition(rightBrim, right.point);
			UpdatePosition(bottomBrim, bottom.point);
			UpdatePosition(leftBrim, left.point);
		}

		private void UpdatePosition(RectTransform brimstone, Vector2 hitPoint) {
			float distance = Vector2.Distance(boss.transform.position, hitPoint);
			brimstone.position = ((Vector2)boss.transform.position + hitPoint) * 0.5f;
			brimstone.localScale = new Vector3(1, distance / brimstone.sizeDelta.y, 1);
			brimstone.rotation = Quaternion.FromToRotation(Vector3.up, hitPoint - new Vector2(boss.transform.position.x, boss.transform.position.y));
		}
	}
}
