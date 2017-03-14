using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public Sprite Active;
	public Sprite Inactive;
	SpriteRenderer selfSprite;

	public Spike spike;

	public AudioSource sound;

	public AudioClip On;
	public AudioClip Off;

	public GameObject wall;

	private int attempts = 0;

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
			attempts++;
			if(attempts == 1) {
				Canvas_Renderer.script.infoRenderer("A projectile pushed the block off of the pressure plate...", "These projectiles sure are a nuisance");

			}
			if (attempts == 3) {
				Canvas_Renderer.script.infoRenderer(null, "Ok, let me help you a little.");
				GameObject protection = Instantiate(wall, gameObject.transform.position + new Vector3(10, 0, 0), Quaternion.identity);
				protection.transform.parent = gameObject.transform.parent;
				protection.name = "Blocker";
				protection.GetComponent<BoxCollider2D>().isTrigger = true;
				protection.transform.localScale = new Vector3(0.2f, 0.1f, 1);
			}
			sound.clip = Off;
			sound.Play();
			selfSprite.sprite = Inactive;
			spike.Hide();
		}
	}
}
