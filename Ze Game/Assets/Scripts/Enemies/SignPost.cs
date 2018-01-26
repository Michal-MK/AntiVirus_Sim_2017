using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public class SignPost : MonoBehaviour {

	public delegate void SignPostInteractions();
	public static event SignPostInteractions OnAvoidanceBegin;

	private void Interact() {
		switch (gameObject.name) {
			case ObjNames.AVOIDANCE_SIGN: {
				if (OnAvoidanceBegin != null) {
					OnAvoidanceBegin();
				}
				MusicHandler.script.TransitionMusic(MusicHandler.script.room1_3_avoidance);
				break;
			}
			case "SignPost Start": {
				Canvas_Renderer.script.InfoRenderer("The virus can not be damaged while it is attacking.", null);
				break;
			}
			case "SignPost Room 1": {
				Canvas_Renderer.script.InfoRenderer("All the spikes you are collecting have a purpouse, hold on to them.", null);
				break;
			}
			case "SignPost PostAvoidance": {
				Canvas_Renderer.script.InfoRenderer("Minions of the Virus are deadly, but you have to endure!", null);
				break;
			}
			case "SignPost Maze": {
				Canvas_Renderer.script.InfoRenderer("The coins are up to no use... yet", null);
				break;
			}
			case "SignPost PreBoss": {
				Canvas_Renderer.script.InfoRenderer("Fired bullets can be picked up and reused. Handy if you miss the taget. Sorry for telling you this late lel. No regrets.", null);
				break;
			}
		}
		StartCoroutine(Fade());
	}

	private IEnumerator Fade() {
		SpriteRenderer selfRender = GetComponent<SpriteRenderer>();
		for (int i = 255; i > 0; i--) {
			selfRender.color = new Color32(255, 255, 255, (byte)i);
			yield return null;
		}
		Destroy(gameObject);
	}
}
