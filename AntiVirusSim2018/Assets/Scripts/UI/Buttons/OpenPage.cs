using UnityEngine;

public class OpenPage : MonoBehaviour {

	public void OpenIt() {
		switch (gameObject.name) {
			case "Source": {
				Application.OpenURL("https://github.com/Michal-MK/AntiVirus_Sim_2017");
				break;
			}
			case "School": {
				Application.OpenURL("http://www.sportgym.cz/");
				break;
			}
			case "Kevin": {
				Application.OpenURL("http://incompetech.com/wordpress/");
				break;
			}
			case "Creo": {
				Application.OpenURL("http://freemusicarchive.org/music/Creo/~/Memory_1520");
				break;
			}
			case "Eric": {
				Application.OpenURL("https://soundcloud.com/eric-skiff/prologue");
				break;
			}
		}
	}
}
