using UnityEngine;
using System.Collections;

public class KillerBlock : Enemy {
	public GameObject killerBlock;
	public GameObject warnSign;
	private Animator selfAnim;
	private BoxCollider2D selfCol;
	private M_Player player;
	private RectTransform background;

	public float timeKillerBlocksActive = 1.8f;
	public float timeCycleIdle = 0.2f;
	public float timeWarning = 1.5f;

	private void Start() {
		_type = EnemyType.TOUCH;
		_is_Destroyable = false;
		selfAnim = killerBlock.GetComponent<Animator>();
		selfCol = killerBlock.GetComponent<BoxCollider2D>();
		player = FindObjectOfType<M_Player>();
		background = GameObject.Find(M_Player.currentBG_name).GetComponent<RectTransform>();
		StartCoroutine(Cycle());
	}

	public IEnumerator Cycle() {
		while (true) {
			selfCol.enabled = false;
			warnSign.SetActive(false);
			killerBlock.SetActive(true);

			yield return new WaitForSeconds(0.2f);
			selfCol.enabled = true;

			yield return new WaitForSeconds(timeKillerBlocksActive);
			selfAnim.SetTrigger("Despawn");
			selfCol.enabled = false;

			yield return new WaitForSeconds(timeCycleIdle);
			killerBlock.SetActive(false);
			Vector3 pos = KBPositions();
			killerBlock.transform.position = pos;
			warnSign.transform.position = pos;
			warnSign.SetActive(true);
			yield return new WaitForSeconds(timeWarning);
		}
	}

	public Vector3 KBPositions() {
		Vector3 killerblockpos = player.transform.position;
		while (Vector2.Distance(player.transform.position, killerblockpos) < 12) {

			float x = Random.Range(-background.sizeDelta.x / 2 + 2, background.sizeDelta.x / 2 - 2);
			float y = Random.Range(-background.sizeDelta.y / 2 + 2, background.sizeDelta.y / 2 - 2);
			killerblockpos = new Vector2(x, y);
		}
		return killerblockpos;
	}
}
