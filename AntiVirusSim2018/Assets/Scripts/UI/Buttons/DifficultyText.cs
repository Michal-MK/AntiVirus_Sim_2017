using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultyText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler {

	public Text source;
	public Text destination;

	public void OnPointerEnter(PointerEventData eventData) {
		destination.text = source.text;
		destination.GetComponent<ShowDifficultyInfo>().Appear();
	}

	public void OnPointerExit(PointerEventData eventData) {
		destination.GetComponent<ShowDifficultyInfo>().Hide();
	}

	public void OnSelect(BaseEventData eventData) {
		destination.text = source.text;
		destination.GetComponent<ShowDifficultyInfo>().Appear();
	}
}
