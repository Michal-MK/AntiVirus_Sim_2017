using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemManager : MonoBehaviour {

	public List<GameObject> unselectabeObjects = new List<GameObject>();
	
	public void TryDeselect() {
		bool deselect = true;
		for (int i = 0; i < unselectabeObjects.Count; i++) {
			if(unselectabeObjects[i] == GetComponent<EventSystem>().currentSelectedGameObject) {
				deselect = false;
			}
		}
		if (deselect) {
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
