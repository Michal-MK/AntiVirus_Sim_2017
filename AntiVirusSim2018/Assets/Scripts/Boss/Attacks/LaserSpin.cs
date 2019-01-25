using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class LaserSpin : IAttackPattern {

		private GameObject brimstone;


		private RectTransform topBrim;
		private RectTransform rightBrim;
		private RectTransform bottomBrim;
		private RectTransform leftBrim;

		private float zRotation = 0;
		private float rotationDelta = 0.1f;

		private GameObject brimstonePrefab;
		private GameObject boss;
		private MonoBehaviour bossBehaviour;
		private RectTransform arenaTransform;

		public Vector3 startPosition { get; set; }

		public bool isAttackInProgress { get; set; }

		public LaserSpin(GameObject boss, Vector3 start, GameObject brimstone, RectTransform arenaBackground) {
			startPosition = start;
			this.boss = boss;
			bossBehaviour = boss.GetComponent<MonoBehaviour>();
			brimstonePrefab = brimstone;
			this.arenaTransform = arenaTransform;
		}

		public IEnumerator Attack() {
			isAttackInProgress = true;

			boss.GetComponent<ParticleSystem>().Emit(100);
			yield return new WaitForSeconds(2f);


			topBrim = Object.Instantiate(brimstonePrefab, boss.transform).GetComponent<RectTransform>();
			topBrim.transform.localPosition = Vector3.zero;
			topBrim.name = "Top";

			rightBrim = Object.Instantiate(brimstonePrefab, boss.transform).GetComponent<RectTransform>();
			rightBrim.transform.localPosition = Vector3.zero;
			rightBrim.name = "Right";

			bottomBrim = Object.Instantiate(brimstonePrefab, boss.transform).GetComponent<RectTransform>();
			bottomBrim.transform.localPosition = Vector3.zero;
			bottomBrim.name = "Bottom";

			leftBrim = Object.Instantiate(brimstonePrefab, boss.transform).GetComponent<RectTransform>();
			leftBrim.transform.localPosition = Vector3.zero;
			leftBrim.name = "Left";

			bossBehaviour.StartCoroutine(VariedRotation());

			yield return new WaitForSeconds(35);
			//--//

			topBrim.gameObject.SetActive(false);
			rightBrim.gameObject.SetActive(false);
			bottomBrim.gameObject.SetActive(false);
			leftBrim.gameObject.SetActive(false);
			boss.transform.rotation = Quaternion.identity;
			bossBehaviour.StopCoroutine(VariedRotation());

			isAttackInProgress = false;
		}

		public IEnumerator VariedRotation() {
			rotationDelta = 0.1f;
			zRotation = 0;
			//this is why the attack is broken!!! - but keep it on harder difficulties because it is fun lel
			while (true) {
				yield return new WaitForSeconds(Random.Range(2, 4));
				rotationDelta = Chance.Half() ? Random.Range(0.4f, 1f) : Random.Range(-1, -0.4f);
			}
		}

		public void Update() {
			zRotation += rotationDelta;
			boss.transform.rotation = Quaternion.Euler(0, 0, zRotation);

			LayerMask mask = LayerMask.GetMask(Layers.WALLS);

			RaycastHit2D up = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.up, arenaTransform.sizeDelta.x * 2, mask.value);
			RaycastHit2D right = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.right, arenaTransform.sizeDelta.x * 2, mask.value);
			RaycastHit2D down = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.down, arenaTransform.sizeDelta.x * 2, mask.value);
			RaycastHit2D left = Physics2D.Raycast(boss.transform.position, boss.transform.rotation * Vector3.left, arenaTransform.sizeDelta.x * 2, mask.value);

			//print(up.transform.name);

			float distance;

			distance = Vector3.Distance(boss.transform.position, up.point);
			topBrim.position = ((Vector2)boss.transform.position + up.point) * 0.5f;
			topBrim.localScale = new Vector3(1, distance / topBrim.sizeDelta.y, 1);
			topBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)up.point - new Vector3(boss.transform.position.x, boss.transform.position.y, 0)));

			distance = Vector3.Distance(boss.transform.position, right.point);
			rightBrim.position = ((Vector2)boss.transform.position + right.point) * 0.5f;
			rightBrim.localScale = new Vector3(1, distance / rightBrim.sizeDelta.y, 1);
			rightBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)right.point - new Vector3(boss.transform.position.x, boss.transform.position.y, 0)));

			distance = Vector3.Distance(boss.transform.position, down.point);
			bottomBrim.position = ((Vector2)boss.transform.position + down.point) * 0.5f;
			bottomBrim.localScale = new Vector3(1, distance / bottomBrim.sizeDelta.y, 1);
			bottomBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)down.point - new Vector3(boss.transform.position.x, boss.transform.position.y, 0)));

			distance = Vector3.Distance(boss.transform.position, left.point);
			leftBrim.position = ((Vector2)boss.transform.position + left.point) * 0.5f;
			leftBrim.localScale = new Vector3(1, distance / leftBrim.sizeDelta.y, 1);
			leftBrim.rotation = Quaternion.FromToRotation(Vector3.up, ((Vector3)left.point - new Vector3(boss.transform.position.x, boss.transform.position.y, 0)));
		}
	}
}
