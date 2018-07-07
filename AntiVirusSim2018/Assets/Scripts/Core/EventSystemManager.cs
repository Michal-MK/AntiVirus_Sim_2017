using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour {

	public List<GameObject> unselectabeObjects = new List<GameObject>();

	public void TryDeselect() {
		bool deselect = true;
		EventSystem eventSystem = GetComponent<EventSystem>();
		for (int i = 0; i < unselectabeObjects.Count; i++) {
			if (unselectabeObjects[i] == eventSystem.currentSelectedGameObject) {
				deselect = false;
			}
		}
		if (deselect) {
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
