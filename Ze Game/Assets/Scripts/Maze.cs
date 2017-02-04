using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Maze : MonoBehaviour {

	public GameObject Cell;
	public GameObject WallI;
	public GameObject WallT;
	public GameObject player;
	public CameraMovement cam;

	public GameObject BG;
	Vector3 hpos;
	public int rowcollCount;

	float widhtHeight;

	public GameObject[,] grid;
	public GameObject[,] wallsT;
	public GameObject[,] wallsR;
	public GameObject[,] wallsB;
	public GameObject[,] wallsL;


	void Start() {


		MazeLevel();

		int size = (int)Mathf.Pow(rowcollCount, 2);
		grid = new GameObject[rowcollCount, rowcollCount];
		wallsT = new GameObject[size, size];
		wallsR = new GameObject[size, size];
		wallsB = new GameObject[size, size];
		wallsL = new GameObject[size, size];


		widhtHeight = BG.GetComponent<RectTransform>().sizeDelta.x / (rowcollCount);

		hpos = BG.transform.position;
		//player.transform.position = hpos;

		//cam.inMaze = true;
		//cam.mazeCam();

		for (int i = 0; i < rowcollCount; i++) {
			for (int j = 0; j < rowcollCount; j++) {

				GameObject cell = Instantiate(Cell, hpos + new Vector3((i * widhtHeight * 2), (j * widhtHeight * 2), 0), Quaternion.identity, gameObject.transform);
				cell.name = "Cell " + i + " " + j;

				cell.GetComponent<Cell>().selfX = i;
				cell.GetComponent<Cell>().selfY = j;

				GameObject wallR = Instantiate(WallI, cell.transform);
				GameObject wallT = Instantiate(WallT, cell.transform);
				GameObject wallL = Instantiate(WallI, cell.transform);
				GameObject wallB = Instantiate(WallT, cell.transform);



				wallT.name = "WallTop";
				wallR.name = "WallRight";
				wallB.name = "WallBottom";
				wallL.name = "WallLeft";

				wallT.transform.position = cell.transform.position + new Vector3(0, widhtHeight, 0);
				wallR.transform.position = cell.transform.position + new Vector3(widhtHeight, 0, 0);
				wallB.transform.position = cell.transform.position + new Vector3(0, -widhtHeight, 0);
				wallL.transform.position = cell.transform.position + new Vector3(-widhtHeight, 0, 0);

				Vector3 currScale = CalculateScale();

				wallT.transform.localScale = currScale;
				wallR.transform.localScale = currScale;
				wallB.transform.localScale = currScale;
				wallL.transform.localScale = currScale;

				grid[i, j] = cell;

				wallsT[i, j] = wallT;
				wallsR[i, j] = wallR;
				wallsB[i, j] = wallB;
				wallsL[i, j] = wallL;
			}
		}
		StartCoroutine(CreatePath());
	}

	public void MazeLevel() {
		switch (PlayerPrefs.GetInt("difficulty")) {
			case 0:
			rowcollCount = 16;
			return;

			case 1:
			rowcollCount = 20;
			return;

			case 2:
			rowcollCount = 22;
			return;

			case 3:
			rowcollCount = 25;
			return;

			case 4:
			rowcollCount = 30;
			return;
		}
	}

	public Vector3 CalculateScale() {
		if (rowcollCount == 16) {
			return new Vector3(2.61f, 2.61f, 0);
		}
		else if (rowcollCount == 20) {
			return new Vector3(2.1f, 2.1f, 0);
		}
		else if (rowcollCount == 22) {
			return new Vector3(2f, 2f, 0);
		}
		else if (rowcollCount == 25) {
			return new Vector3(1.7f, 1.7f, 0);
		}
		else if (rowcollCount == 30) {
			return new Vector3(1.45f, 1.45f, 0);
		}
		else {
			//impossible to happen
			return Vector3.one;
		}
	}

	public Stack<GameObject> stack = new Stack<GameObject>();
	List<GameObject> nope = new List<GameObject>();

	public GameObject current;
	private int XPosition = 0;
	private int Yposition = 0;
	public bool run = true;
	public GameObject chosenNeighbor;

	public IEnumerator CreatePath() {


		GameObject start = grid[0, 0];
		current = start;
		stack.Push(current);

		while (stack.Count > 0 && run == true) {
			if(stack.Count == 0) {
				MazeStopped();
			}else {
				chosenNeighbor = GetNeighbor(XPosition, Yposition);
				//M_Player.doNotMove = true;
				//player.transform.position = hpos;
			}
			if (run == false) {
				//M_Player.doNotMove = false;
			}

			yield return new WaitForSeconds(0.001f);


			if (chosenNeighbor.transform.position.x < current.transform.position.x) {
				string nameCurrent = "WallLeft";
				string nameNeighbor = "WallRight";

				Component[] compsC = current.GetComponentsInChildren<Component>();
				Component[] compsCH = chosenNeighbor.GetComponentsInChildren<Component>();

				foreach (Component g in compsC) {
					if (nameCurrent == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				foreach (Component g in compsCH) {
					if (nameNeighbor == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				stack.Push(chosenNeighbor);

				current = chosenNeighbor;



			}
			else if (chosenNeighbor.transform.position.x > current.transform.position.x) {
				string nameCurrent = "WallRight";
				string nameNeighbor = "WallLeft";

				Component[] compsC = current.GetComponentsInChildren<Component>();
				Component[] compsCH = chosenNeighbor.GetComponentsInChildren<Component>();

				foreach (Component g in compsC) {
					if (nameCurrent == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				foreach (Component g in compsCH) {
					if (nameNeighbor == g.name) {
						g.gameObject.SetActive(false);
					}
				}



				stack.Push(chosenNeighbor);

				current = chosenNeighbor;

			}
			else if (chosenNeighbor.transform.position.y < current.transform.position.y) {
				string nameCurrent = "WallBottom";
				string nameNeighbor = "WallTop";

				Component[] compsC = current.GetComponentsInChildren<Component>();
				Component[] compsCH = chosenNeighbor.GetComponentsInChildren<Component>();

				foreach (Component g in compsC) {
					if (nameCurrent == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				foreach (Component g in compsCH) {
					if (nameNeighbor == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				stack.Push(chosenNeighbor);

				current = chosenNeighbor;

			}
			else if (chosenNeighbor.transform.position.y > current.transform.position.y) {
				string nameCurrent = "WallTop";
				string nameNeighbor = "WallBottom";

				Component[] compsC = current.GetComponentsInChildren<Component>();
				Component[] compsCH = chosenNeighbor.GetComponentsInChildren<Component>();

				foreach (Component g in compsC) {
					if (nameCurrent == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				foreach (Component g in compsCH) {
					if (nameNeighbor == g.name) {
						g.gameObject.SetActive(false);
					}
				}

				stack.Push(chosenNeighbor);

				current = chosenNeighbor;

			}
		}
	}


	List<GameObject> neighbors;



	public GameObject GetNeighbor(int x, int y) {
		bool continuee = true;
		int currentPosX = x;
		int currentPosY = y;

		int RightX = 0;
		int RightY = 0;

		int LeftX = 0;
		int LeftY = 0;

		int TopX = 0;
		int TopY = 0;

		int BottomX = 0;
		int BottomY = 0;

		neighbors = new List<GameObject>();

		while (continuee == true && run == true) {

			if(stack.Count <= 0) {
				MazeStopped();
			}

			


			if (currentPosX + 1 <= rowcollCount - 1) {
				if (stack.Contains(grid[currentPosX + 1, currentPosY]) == false && nope.Contains(grid[currentPosX + 1, currentPosY]) == false) {
					if (grid[currentPosX + 1, currentPosY].GetComponent<Cell>().neverVisitMeAgain == false) {
						//print("Added Right neigbor to the list");
						neighbors.Add(grid[currentPosX + 1, currentPosY]);

						RightX = currentPosX + 1;
						RightY = currentPosY;
						continuee = false;
					}
				}
			}
			if (currentPosY + 1 <= rowcollCount - 1) {
				if (stack.Contains(grid[currentPosX, currentPosY + 1]) == false && nope.Contains(grid[currentPosX, currentPosY + 1]) == false) {
					if (grid[currentPosX, currentPosY + 1].GetComponent<Cell>().neverVisitMeAgain == false) {
						//print("Added Upper neigbor to the list");
						neighbors.Add(grid[currentPosX, currentPosY + 1]);

						TopX = currentPosX;
						TopY = currentPosY + 1;
						continuee = false;
					}
				}
			}
			if (currentPosX - 1 >= 0) {
				if (stack.Contains(grid[currentPosX - 1, currentPosY]) == false && nope.Contains(grid[currentPosX - 1, currentPosY]) == false) {
					if (grid[currentPosX - 1, currentPosY].GetComponent<Cell>().neverVisitMeAgain == false) {
						//print("Added Left neigbor to the list");
						neighbors.Add(grid[currentPosX - 1, currentPosY]);

						LeftX = currentPosX - 1;
						LeftY = currentPosY;
						continuee = false;
					}
				}
			}
			if (currentPosY - 1 >= 0) {
				if (stack.Contains(grid[currentPosX, currentPosY - 1]) == false && nope.Contains(grid[currentPosX, currentPosY - 1]) == false) {
					if (grid[currentPosX, currentPosY - 1].GetComponent<Cell>().neverVisitMeAgain == false) {
						//print("Added Bottom neigbor to the list");
						neighbors.Add(grid[currentPosX, currentPosY - 1]);

						BottomX = currentPosX;
						BottomY = currentPosY - 1;
						continuee = false;
					}
				}
			}

			if (neighbors.Count <= 0) {


				nope.Add(current);

				current.GetComponent<Cell>().neverVisitMeAgain = true;

				XPosition = current.GetComponent<Cell>().selfX;
				Yposition = current.GetComponent<Cell>().selfY;

				if (stack.Count > 0) {
					current = stack.Pop();
				}

				currentPosX = current.GetComponent<Cell>().selfX;
				currentPosY = current.GetComponent<Cell>().selfY;

				print("OVER!");

				MazeStopped();
			}
		}
		if (neighbors.Count > 0) {
			int choice = Random.Range(0, neighbors.Count);

			string name = neighbors[choice].name;
			print(name);

			if (name == "Cell " + RightX + " " + RightY) {
				XPosition = RightX;
				Yposition = RightY;

			}

			else if (name == "Cell " + TopX + " " + TopY) {
				XPosition = TopX;
				Yposition = TopY;
			}

			else if (name == "Cell " + LeftX + " " + LeftY) {
				XPosition = LeftX;
				Yposition = LeftY;

			}

			else if (name == "Cell " + BottomX + " " + BottomY) {
				XPosition = BottomX;
				Yposition = BottomY;

			}


			return neighbors[choice];
		}
		else {
			MazeStopped();
			return null;
		}

	}

	public void MazeStopped() {
		if (neighbors.Count == 0 && stack.Count <= 1) {
			run = false;
			StopAllCoroutines();
			print("MazeStopped");
		}
	}
}
