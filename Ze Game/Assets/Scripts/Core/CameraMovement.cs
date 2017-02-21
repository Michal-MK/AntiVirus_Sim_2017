using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour {
	public float zero = 0;
	public RectTransform bg;
	public Transform player;
	public M_Player playerScript;
	public GameObject spike;
	public Vector3 cam_pos;
	public Animator anim;
	Camera thisone; 
	public List<GameObject> BackGroundS = new List<GameObject>();
	public float camWidht;
	public float camHeight;
	public Vector3 middle;
	public float currentBGX;
	public float currentBGY;


	public static GameObject[] loadedZones;

	public RectTransform bossRoom;
	public RectTransform MazeBG;

	public bool inBossRoom = false;
	public bool inMaze = false;

	void Start(){
		

		//Cursor.visible = false;
		thisone = GetComponent<Camera> ();
		BackGroundS.Add (bg.gameObject);
		camWidht = thisone.aspect * thisone.orthographicSize;
		camHeight = thisone.orthographicSize;
		

	}
		

	public void raycastForRooms(){
		BackGroundS.Clear ();

		bg = GameObject.Find(M_Player.currentBG_name).GetComponent<RectTransform>();

		currentBGY = bg.sizeDelta.y / 2;
		currentBGX = bg.sizeDelta.x / 2;


		RaycastHit2D[] up = Physics2D.RaycastAll (new Vector2 (bg.position.x, bg.position.y), Vector2.up, currentBGY +10);
		RaycastHit2D[] down = Physics2D.RaycastAll (new Vector2 (bg.position.x, bg.position.y), Vector2.down, Mathf.Abs (-currentBGY -10));
		RaycastHit2D[] left = Physics2D.RaycastAll (new Vector2 (bg.position.x, bg.position.y), Vector2.left, Mathf.Abs (-currentBGX -10));
		RaycastHit2D[] right = Physics2D.RaycastAll (new Vector2 (bg.position.x, bg.position.y), Vector2.right, currentBGX +10);

		Debug.DrawRay (new Vector2 (bg.position.x, bg.position.y),Vector2.up*100,Color.blue, 5);
		Debug.DrawRay (new Vector2 (bg.position.x, bg.position.y), Vector2.down*100, Color.green, 5);
		Debug.DrawRay (new Vector2 (bg.position.x, bg.position.y), Vector2.left*100, Color.red, 5);
		Debug.DrawRay (new Vector2 (bg.position.x, bg.position.y), Vector2.right*100, Color.yellow, 5);



		bool continueInLoop = true;

		foreach (RaycastHit2D hits in up) {
			if (continueInLoop == false) {
				break;
			}
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				continueInLoop = false;
				break;
			}
			if (hits.transform.tag == "BG" && BackGroundS.Contains (hits.transform.gameObject) == false) {

					GameObject hitObj = hits.transform.gameObject;
					BackGroundS.Add (hitObj);
				
			}
		}

		continueInLoop = true;
		foreach (RaycastHit2D hits in down) {
			if (continueInLoop == false) {
				break;
			}
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				continueInLoop = false;
				break;
			}
			if (hits.transform.tag == "BG" && BackGroundS.Contains (hits.transform.gameObject) == false) {
				
					
					GameObject hitObj = hits.transform.gameObject;
					BackGroundS.Add (hitObj);

				}

		}

		continueInLoop = true;
		foreach (RaycastHit2D hits in left) {

			if (continueInLoop == false) {
				break;
			}
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				continueInLoop = false;
				break;
			}
			if (hits.transform.tag == "BG"  && BackGroundS.Contains (hits.transform.gameObject) == false) {
				

					GameObject hitObj = hits.transform.gameObject;
					BackGroundS.Add (hitObj);
				}

		}

		continueInLoop = true;
		foreach (RaycastHit2D hits in right) {
			if (continueInLoop == false) {
				break;
			}
			if (hits.transform.gameObject.activeSelf == true && hits.transform.tag == "Wall/Door" || hits.transform.tag == "Wall") {
				continueInLoop = false;
				break;
			}		
			if (hits.transform.tag == "BG" && BackGroundS.Contains (hits.transform.gameObject) == false) {
					
					GameObject hitObj = hits.transform.gameObject;
					BackGroundS.Add (hitObj);

			}
		}
		calculateArea ();
			 

	}



	public void calculateArea(){

		currentBGX = 0;
		currentBGY = 0;
		int i = 0;
		bool Vertical = true;
		bool Horisontal = true;
		GameObject[] BGarray = new GameObject[BackGroundS.Count];

		bg = GameObject.Find (M_Player.currentBG_name).GetComponent <RectTransform>();

		BGarray = BackGroundS.ToArray ();
		loadedZones = new GameObject[BGarray.Length];
		loadedZones = BGarray;
		//foreach (GameObject zones in loadedZones) {
		//	Debug.Log (i + "  " + zones);
		//	i++;
		//} 

		float xForAll = 0;
		float yForAll = 0;
		foreach (GameObject gg in BGarray) {
			if (i == 0) {
				xForAll = gg.transform.position.x;
				yForAll = gg.transform.position.y;
			}
			if (gg.transform.position.x != xForAll) {
				Vertical = false;
			}
			if (gg.transform.position.y != yForAll) {
				Horisontal = false;
			}

		}
		if (Horisontal && Vertical) {
			Horisontal = false;
			Vertical = false;
		}

		if (Vertical == true) {
//			Debug.Log ("Vertical " + M_Player.currentBG_name);
			float TopBorder = -Mathf.Infinity;
			float BottomBorder = Mathf.Infinity;
			foreach (GameObject BackGroundRect in BGarray) {
				if (BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2 > currentBGX) {
					currentBGX = BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2;
				}
				currentBGY += BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2;
				float specificTop = BackGroundRect.transform.position.y + BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2;
				float specificBottom = BackGroundRect.transform.position.y - BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2;
				if (specificBottom < BottomBorder) {
					BottomBorder = specificBottom;
				}
				if (specificTop > TopBorder) {
					TopBorder = specificTop;
				}
			


			}
			middle.x = bg.position.x;
			middle.y = (BottomBorder + TopBorder) / 2;

		} else if (Horisontal == true) {
//			Debug.Log ("Horizontal " + M_Player.currentBG_name);
			float LeftBorder = Mathf.Infinity;
			float RightBorder = -Mathf.Infinity;
			foreach (GameObject BackGroundRect in BGarray) {
				if (BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2 > currentBGY) {
					currentBGY = BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2;
				}
				currentBGX += BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2;
				float specificLeft = BackGroundRect.transform.position.x - BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2;
				float specificRight = BackGroundRect.transform.position.x + BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2;

				if (specificLeft < LeftBorder) {
					LeftBorder = specificLeft;
				}
				if (specificRight > RightBorder) {
					RightBorder = specificRight;
				}
			}
			middle.y = bg.position.y;
			middle.x = (LeftBorder + RightBorder) / 2;
		}
		else 
		{
//			Debug.Log ("I never asked for this. Objects not in line or only one Object in Array " + M_Player.currentBG_name);

			float TopBorder = -Mathf.Infinity;
			float BottomBorder = Mathf.Infinity;
			foreach (GameObject BackGroundRect in BGarray) {
				float specificTop = BackGroundRect.transform.position.y + BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2;
				float specificBottom = BackGroundRect.transform.position.y - BackGroundRect.GetComponent<RectTransform> ().sizeDelta.y / 2;
				if (specificBottom < BottomBorder) {
					BottomBorder = specificBottom;
				}
				if (specificTop > TopBorder) {
					TopBorder = specificTop;
				}



			}
			currentBGY = ( - BottomBorder + TopBorder) / 2;
			middle.y = (BottomBorder + TopBorder) / 2;
			float LeftBorder = Mathf.Infinity;
			float RightBorder = -Mathf.Infinity;
			foreach (GameObject BackGroundRect in BGarray) {

				float specificLeft = BackGroundRect.transform.position.x - BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2;
				float specificRight = BackGroundRect.transform.position.x + BackGroundRect.GetComponent<RectTransform> ().sizeDelta.x / 2;
				if (specificLeft < LeftBorder) {
					LeftBorder = specificLeft;
				}
				if (specificRight > RightBorder) {
					RightBorder = specificRight;
				}
			}
			middle.x = (LeftBorder + RightBorder) / 2;
			currentBGX = ( - LeftBorder + RightBorder) / 2;
		}
	}

	float zero1 = 0;

	private bool inBossRoomOnce = false;

	private void Update() {
		camWidht = thisone.aspect * thisone.orthographicSize;
		camHeight = thisone.orthographicSize;
	}

	void LateUpdate(){

		if (inBossRoom == false && inMaze == false) {
			cam_pos = new Vector3 (camX (), camY (), player.position.z - 10);
			gameObject.transform.position = cam_pos;

		}
		else {
			if (zero < 0.15f && inMaze == true) {
				zero += Time.deltaTime / 100;
				Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, MazeBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, zero);
				if (zero >= 0.15f && zero < 0.2f) {

					zero1 += Time.deltaTime / 100;
					Camera.main.transform.position = new Vector3((Mathf.SmoothStep(Camera.main.transform.position.x, player.transform.position.x, zero1)),
																 (Mathf.SmoothStep(Camera.main.transform.position.y, player.transform.position.y, zero1)),
																  -10);
					if (zero > 0.3f) {
						inMaze = false;
						zero = 0;
						zero1 = 0;
					}
				}
			}

			if (inBossRoom && zero < 0.1f && !inBossRoomOnce) {
				zero += Time.deltaTime * 0.01f;
				//print(zero);
				Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, bossRoom.sizeDelta.x * 0.5f * Screen.height / Screen.width, zero);
				if (zero > 0.1f) {
					Camera.main.orthographicSize = 108;
					inBossRoomOnce = true;
					zero = 0;
				}

			}
		}
	}


	public float camX() {
		if (player.position.x > currentBGX + middle.x - camWidht) {
			
			return currentBGX + middle.x - camWidht;

		} else if (player.position.x < -currentBGX + middle.x + camWidht) {
			
			return 	-currentBGX + middle.x + camWidht;

		} else {
			
			return player.position.x;
		}
			
	}

	public float camY(){
		
		if (player.position.y > currentBGY + middle.y - camHeight) {
			
			return currentBGY + middle.y - camHeight;

		} else if (player.position.y < -currentBGY + middle.y + camHeight) {
			
			return 	-currentBGY + middle.y + camHeight;

		} else {
			
			return player.position.y;
		}
	}

	public void bossFightCam(int bossNo){
		inBossRoom = true;

		bossRoom = GameObject.Find ("Background_room_Boss_" + bossNo).GetComponent <RectTransform>();

		float bossX = bossRoom.position.x;
		float bossY = bossRoom.position.y;

		player.position = new Vector3 (bossX, bossY, 0);
		gameObject.transform.position = new Vector3 (bossX, bossY, -10);

	}
	public void mazeCam() {
		inMaze = true;
		RectTransform mazeBG = GameObject.Find("MazeBG").GetComponent<RectTransform>();
		float mazeX = mazeBG.position.x;
		float mazey = mazeBG.position.y;
		print("Transfrom!");
		gameObject.transform.position = new Vector3(mazeX, mazey, -10);
	}
}