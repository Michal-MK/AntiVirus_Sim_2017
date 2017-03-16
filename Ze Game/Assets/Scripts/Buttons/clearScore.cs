using UnityEngine;

public class clearScore : MonoBehaviour {
	public displayScore dsp;

	public void OnPress() {

		foreach (Transform result in GameObject.Find("Results").GetComponentsInChildren<Transform>()) {
			if (result.name != "Results") {
				Destroy(result.gameObject);
			}
		}

		for (int i = 0; i < 50; i++) {
			PlayerPrefs.SetFloat(i.ToString(), Mathf.Infinity);
		}
		dsp.CreateBoard();
		dsp.Display();

	}
}
