using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class KillerBlockPath : IAttackPattern {

		private ObjectPool pool_KillerBlock;

		private int changes = 0;

		public Vector3 startPosition { get; set; }

		public GameObject cageObj;

		private GameObject boss;
		private RectTransform bossTransform;
		private MonoBehaviour bossBehaviour;
		private RectTransform arenaTransform;

		private Animator selfAnim;
		private Rigidbody2D selfRigid;


		public KillerBlockPath(GameObject boss, Vector3 start, RectTransform arenaTransform) {
			pool_KillerBlock = new ObjectPool(Resources.Load(PrefabNames.ENEMY_KILLERBLOCK_BOSS) as GameObject);
			startPosition = start;
			bossTransform = boss.GetComponent<RectTransform>();
			this.arenaTransform = arenaTransform;

			selfAnim = boss.AddComponent<Animator>();
			selfRigid = boss.AddComponent<Rigidbody2D>();
			selfRigid.isKinematic = true;
			selfRigid.gravityScale = 0;
		}

		public bool isAttackInProgress { get; set; }

		public IEnumerator Attack() {
			isAttackInProgress = true;

			MoveScript positioningCage = GameObject.Instantiate(cageObj, M_Player.player.transform.position, Quaternion.identity).GetComponent<MoveScript>();

			yield return new WaitForSeconds(2);
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, bossTransform.position + new Vector3(0, 50, 0), Time.deltaTime / 2, null));
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, bossTransform.position + new Vector3(0, 50, 0), Time.deltaTime / 2, null));
			positioningCage.destroy = true;
			yield return new WaitForSeconds(2);

			bossBehaviour.StartCoroutine(KillerBlockPathGenerator(positioningCage.GetComponent<MoveScript>()));

			while (isAttackInProgress) {

				MoveScript BlockL = pool_KillerBlock.getNext.GetComponent<MoveScript>();
				BlockL.gameObject.SetActive(true);
				BlockL.transform.position = new Vector3(bossTransform.position.x - bossTransform.sizeDelta.x / 2, bossTransform.position.y, 1);
				BlockL.transform.localScale = new Vector3(3, 3, 1);
				BlockL.Move();

				MoveScript BlockR = pool_KillerBlock.getNext.GetComponent<MoveScript>();
				BlockR.gameObject.SetActive(true);
				BlockR.transform.position = new Vector3(bossTransform.position.x + bossTransform.sizeDelta.x / 2, bossTransform.position.y, 1);
				BlockR.transform.localScale = new Vector3(3, 3, 1);
				BlockR.Move();

				yield return new WaitForSeconds(0.2f);

				if (changes >= 10) {
					selfAnim.SetTrigger("Reset");
					changes = 0;
					break;
				}
			}
			selfRigid.velocity = Vector3.zero;
			isAttackInProgress = false;
		}

		public IEnumerator KillerBlockPathGenerator(MoveScript positioningCage) {
			Directions current = Directions.RIGHT;
			yield return new WaitForSeconds(1);
			selfAnim.Play("SpeedUp");
			positioningCage.Move();

			while (isAttackInProgress) {
				LayerMask mask = LayerMask.GetMask(Layers.WALLS);
				RaycastHit2D right = Physics2D.Raycast(bossTransform.position, Vector3.right, arenaTransform.sizeDelta.x, mask.value);
				RaycastHit2D left = Physics2D.Raycast(bossTransform.position, Vector3.left, arenaTransform.sizeDelta.x, mask.value);

				float distR = Vector3.Distance(bossTransform.position, new Vector3(right.point.x - bossTransform.sizeDelta.x / 2, bossTransform.position.y));
				float distL = Vector3.Distance(bossTransform.position, new Vector3(left.point.x + bossTransform.sizeDelta.x / 2, bossTransform.position.y));

				yield return new WaitUntil(() => Mathf.Abs(selfAnim.GetFloat("Speed")) < 5);
				AnimatorStateInfo state = selfAnim.GetCurrentAnimatorStateInfo(0);
				if (state.IsName("ChangeDirToLeft")) {
					current = Directions.LEFT;
				}
				else if (state.IsName("ChangeDirToRight")) {
					current = Directions.RIGHT;
				}
				yield return new WaitForSeconds(0.5f);

				float arriveTime = current == Directions.RIGHT ? distR / 40 : distL / 40;
				yield return new WaitForSeconds(Random.Range(1, arriveTime));
				selfAnim.SetTrigger(current == Directions.RIGHT ? "Left" : "Right");
				changes++;
			}
		}

		public void Update() {
			selfRigid.velocity = new Vector2(1, 0) * selfAnim.GetFloat("Speed");
		}
	}
}
