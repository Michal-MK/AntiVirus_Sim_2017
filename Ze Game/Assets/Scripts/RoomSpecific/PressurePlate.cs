using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public Sprite Active;
	public Sprite Inactive;
	SpriteRenderer selfSprite;
	public Spike spike;

	public AudioSource sound;

	public AudioClip On;
	public AudioClip Off;


	void Start(){
		selfSprite = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player" || col.name == "Block") {
			selfSprite.sprite = Active;
			sound.clip = On;
			sound.Play();
			spike.SetPosition();
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player" || col.name == "Block") {
			sound.clip = Off;
			sound.Play();
			selfSprite.sprite = Inactive;
			spike.Hide();
		}
	}
}
