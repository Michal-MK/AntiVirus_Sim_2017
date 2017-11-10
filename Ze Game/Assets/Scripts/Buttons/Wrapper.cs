using UnityEngine;
using UnityEngine.UI;

public class Wrapper : MonoBehaviour {
	public Toggle start;
	public Button[] buttons;

	#region Reasonable code here

	public void SetSelectedProfile(GameObject profile) {
		Profile.SelectProfile(profile);
	}

	public char Validate(char ch) {
		if (ch == '$' || ch == '~' || ch == '@' || ch == '_' || ch == '#') {
			ch = '\0';
		}
		return ch;
	}

	#endregion

	public void ToggleButtonInteractivity() {

		foreach (Button item in buttons) {
			item.interactable = !item.interactable;
		}
		start.interactable = !start.interactable;
	}
}
