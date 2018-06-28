using UnityEngine.SceneManagement;
using System.Collections;

namespace Igor.Minigames {
	public enum MiniGames {
		SHIPS
	}

	public class MiniGame_Manager {
		public static void LoadMinigame(MiniGames game) {
			switch (game) {
				case MiniGames.SHIPS: {
					SceneManager.LoadScene("Ships");
					return;
				}
			}
		}
	}
}
