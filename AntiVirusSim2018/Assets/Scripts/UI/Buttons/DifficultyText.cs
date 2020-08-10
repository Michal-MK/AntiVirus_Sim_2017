using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultyText : MonoBehaviour, IPointerEnterHandler, ISelectHandler {

	public Text source;
	public Text destination;

	public void OnPointerEnter(PointerEventData eventData) {
		destination.text = source.text;
		destination.GetComponent<ShowDifficultyInfo>().Appear();
	}

	public void OnSelect(BaseEventData eventData) {
		destination.text = source.text;
		destination.GetComponent<ShowDifficultyInfo>().Appear();
	}
}
