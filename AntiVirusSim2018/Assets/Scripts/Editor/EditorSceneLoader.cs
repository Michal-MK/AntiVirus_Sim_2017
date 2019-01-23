using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Igor.Constants.Strings;

public class EditorSceneLoader : ScriptableObject {

	[MenuItem("Scenes/Switch to MenuScene _F5")]
	static void MenuScene() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + SceneNames.MENU_SCENE + ".unity", OpenSceneMode.Single);
	}

	[MenuItem("Scenes/Switch to GameScene _F6")]
	static void Game() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + SceneNames.GAME1_SCENE + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Switch to Saves _F7")]
	static void Saves() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + SceneNames.SAVES_SCENE + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Switch to Leader board _F8")]
	static void Leaderboard() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + SceneNames.LEADERBOARD_SCENE + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Switch to Help _F9")]
	static void Help() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + SceneNames.HELP1_SCENE + ".unity", OpenSceneMode.Single);
	}
	[MenuItem("Scenes/Switch to Credits _F10")]
	static void Credits() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/" + SceneNames.CREDITS_SCENE + ".unity", OpenSceneMode.Single);
	}
}