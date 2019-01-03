using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Collections.Generic;

public class UICallbacks : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler {

	public enum ActionEnum {

	}

	//private Action Action;

	public ActionEnum current;


	public bool onDeselect;
	public bool onPointerEnter;
	public bool onPointerExit;
	public bool onPointerClick;

	private void Start() {
		//switch (current) {

		//}

		int counter = 0;
		if (onDeselect) {
			counter++;
		}
		if (onPointerClick) {
			counter++;
		}
		if (onPointerEnter) {
			counter++;
		}
		if (onPointerExit) {
			counter++;
		}
		if (counter > 1) {
			throw new InvalidOperationException("Can not have multiple callback enabled in one UICallbacks script, it can lead to unintended behaviour. If you want multiple, add this script multiple times!");
		}
	}


	public void OnDeselect(BaseEventData eventData) {
		if (onDeselect) {
			//print("OnDeselect");
			//Action();
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (onPointerClick) {
			//print("OnPointerClick");
			//Action();
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (onPointerEnter) {
			//print("OnPointerEnter");
			//Action();
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (onPointerExit) {
			//print("OnPointerExit");
			//Action();
		}
	}
}
