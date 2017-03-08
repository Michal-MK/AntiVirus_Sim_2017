using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public Sprite Active;
	public Sprite Inactive;
	SpriteRenderer selfSprite;
	public Spike spike;


	void Start(){
		selfSprite = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player" || col.name == "Block") {
			selfSprite.sprite = Active;
			spike.SetPosition();
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player" || col.name == "Block") {
			selfSprite.sprite = Inactive;
			spike.Hide();
		}
	}
}
