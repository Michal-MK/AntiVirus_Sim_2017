using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

namespace Igor.Boss.Attacks {
	public class KillerBlockPath : IAttackPattern {

		private ObjectPool pool_KillerBlock;
		public GameObject cageObj;
			   
		private GameObject boss;
		private RectTransform bossTransform;
		private MonoBehaviour bossBehaviour;
		private RectTransform arenaTransform;

		private Animator selfAnim;
		private Rigidbody2D selfRigid;

		private Coroutine pathGenerationRoutine;
		private int changes = 0;

		public bool isAttackInProgress { get; set; }
		public Vector3 startPosition { get; set; }


		public KillerBlockPath(GameObject boss, Vector3 startPosition, RectTransform arenaTransform) {
			this.startPosition = startPosition;
			this.boss = boss;
			this.arenaTransform = arenaTransform;

			pool_KillerBlock = new ObjectPool(Resources.Load(PrefabNames.ENEMY_KILLERBLOCK_BOSS) as GameObject);
			cageObj = Resources.Load<GameObject>(PrefabNames.CAGE);
			bossTransform = boss.GetComponent<RectTransform>();
			bossBehaviour = boss.GetComponent<MonoBehaviour>();
		}


		public IEnumerator Attack() {
			selfAnim = boss.AddComponent<Animator>();
			selfAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(PrefabNames.BOSS_ANIM_CONTROLLER);
			selfAnim.applyRootMotion = true;
			selfRigid = boss.AddComponent<Rigidbody2D>();
			selfRigid.isKinematic = true;
			selfRigid.gravityScale = 0;

			isAttackInProgress = true;

			Movement positioningCage = Object.Instantiate(cageObj, M_Player.player.transform.position, Quaternion.identity).GetComponent<Movement>();
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(positioningCage.gameObject, bossTransform.position + new Vector3(0, 50, 0), Time.deltaTime / 2, null));
			bossBehaviour.StartCoroutine(LerpFunctions.LerpPosition(M_Player.player.gameObject, bossTransform.position + new Vector3(0, 50, 0), Time.deltaTime / 2, null));
			yield return new WaitForSeconds(2);

			pathGenerationRoutine = bossBehaviour.StartCoroutine(KillerBlockPathGenerator(positioningCage));

			while (isAttackInProgress) {

				Movement BlockL = pool_KillerBlock.getNext.GetComponent<Movement>();
				BlockL.gameObject.SetActive(true);
				BlockL.transform.position = new Vector3(bossTransform.position.x - bossTransform.sizeDelta.x / 2, bossTransform.position.y, 1);
				BlockL.transform.localScale = new Vector3(3, 3, 1);
				BlockL.direction = Vector2.up * 30;
				BlockL.MoveAndDestroyOnWallLeave();

				Movement BlockR = pool_KillerBlock.getNext.GetComponent<Movement>();
				BlockR.gameObject.SetActive(true);
				BlockR.transform.position = new Vector3(bossTransform.position.x + bossTransform.sizeDelta.x / 2, bossTransform.position.y, 1);
				BlockR.transform.localScale = new Vector3(3, 3, 1);
				BlockR.direction = Vector2.up * 30;
				BlockR.MoveAndDestroyOnWallLeave();

				yield return new WaitForSeconds(0.2f);

				if (changes >= 10) {
					selfAnim.SetTrigger(nameof(KillerBlockPath) + "_Reset");
					changes = 0;
					break;
				}
			}
			selfRigid.velocity = Vector3.zero;
			Object.DestroyImmediate(selfAnim);
			Object.DestroyImmediate(selfRigid);

			isAttackInProgress = false;
			bossBehaviour.StopCoroutine(pathGenerationRoutine);
		}

		public IEnumerator KillerBlockPathGenerator(Movement positioningCage) {
			Directions current = Directions.RIGHT;
			yield return new WaitForSeconds(1);
			selfAnim.Play(nameof(KillerBlockPath) + "_SpeedUp");
			positioningCage.direction = Vector2.up * 25;
			positioningCage.MoveAndDestroyOnWallEnter();

			while (isAttackInProgress) {
				LayerMask mask = LayerMask.GetMask(Layers.WALLS);
				RaycastHit2D right = Physics2D.Raycast(bossTransform.position, Vector3.right, arenaTransform.sizeDelta.x, mask.value);
				RaycastHit2D left = Physics2D.Raycast(bossTransform.position, Vector3.left, arenaTransform.sizeDelta.x, mask.value);

				float distR = Vector3.Distance(bossTransform.position, new Vector3(right.point.x - bossTransform.sizeDelta.x / 2, bossTransform.position.y));
				float distL = Vector3.Distance(bossTransform.position, new Vector3(left.point.x + bossTransform.sizeDelta.x / 2, bossTransform.position.y));

				yield return new WaitUntil(() => Mathf.Abs(selfAnim.GetFloat(nameof(KillerBlockPath) + "_Speed")) < 5);
				AnimatorStateInfo state = selfAnim.GetCurrentAnimatorStateInfo(0);
				if (state.IsName(nameof(KillerBlockPath) + "_ChangeDirToLeft")) {
					current = Directions.LEFT;
				}
				else if (state.IsName(nameof(KillerBlockPath) + "_ChangeDirToRight")) {
					current = Directions.RIGHT;
				}
				yield return new WaitForSeconds(0.5f);

				float arriveTime = current == Directions.RIGHT ? distR / 40 : distL / 40;
				yield return new WaitForSeconds(Random.Range(1, arriveTime));
				selfAnim.SetTrigger(current == Directions.RIGHT ? nameof(KillerBlockPath) + "_Left" : nameof(KillerBlockPath) + "_Right");
				changes++;
			}
		}

		public void Update() {
			selfRigid.velocity = new Vector2(1, 0) * selfAnim.GetFloat(nameof(KillerBlockPath) + "_Speed");
		}
	}
}
