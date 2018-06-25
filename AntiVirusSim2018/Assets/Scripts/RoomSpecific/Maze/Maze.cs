using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public delegate void MazeBehaviour();

	public GameObject Cell;
	public GameObject WallI;
	public GameObject WallT;
	public GameObject WallIDmg;
	public GameObject WallTDmg;

	public GameObject teleport;
	public RectTransform mazeBackground;
	public int rowcollCount;

	private float widthHeight;
	public float mazeDimension;

	public GameObject[,] grid;
	private GameObject[,] wallsT;
	private GameObject[,] wallsR;
	private GameObject[,] wallsB;
	private GameObject[,] wallsL;

	private static float _mazeSpeedMultiplier;
	public Transform middleCell;
	public GameObject infoBoardMaze;

	private Stack<GameObject> stack = new Stack<GameObject>();
	private List<GameObject> nope = new List<GameObject>();

	private GameObject current;
	private int XPosition = 0;
	private int Yposition = 0;
	public bool run = true;
	private GameObject chosenNeighbor;

	public Vector2 playerEntrancePosition;

	void Start() {

		rowcollCount = MazeLevel();

		int size = (int)Mathf.Pow(rowcollCount, 2);
		grid = new GameObject[rowcollCount, rowcollCount];
		wallsT = new GameObject[size, size];
		wallsR = new GameObject[size, size];
		wallsB = new GameObject[size, size];
		wallsL = new GameObject[size, size];

		//Stupd hardcoded value...
		widthHeight = 200 / (rowcollCount);
		mazeDimension = widthHeight * rowcollCount;

		for (int i = 0; i < rowcollCount; i++) {
			for (int j = 0; j < rowcollCount; j++) {

				GameObject cell = Instantiate(Cell, transform.position + new Vector3((i * widthHeight * 2), (j * widthHeight * 2), 0), Quaternion.identity, gameObject.transform);
				cell.name = "Cell " + i + " " + j;

				cell.GetComponent<Cell>().selfX = i;
				cell.GetComponent<Cell>().selfY = j;

				GameObject wallR;
				GameObject wallT;
				GameObject wallL;
				GameObject wallB;

				if (Control.currDifficulty < 3) {
					wallR = Instantiate(WallI, cell.transform);
					wallT = Instantiate(WallT, cell.transform);
					wallL = Instantiate(WallI, cell.transform);
					wallB = Instantiate(WallT, cell.transform);
				}
				else {
					wallR = Instantiate(WallIDmg, cell.transform);
					wallT = Instantiate(WallTDmg, cell.transform);
					wallL = Instantiate(WallIDmg, cell.transform);
					wallB = Instantiate(WallTDmg, cell.transform);
				}


				wallT.name = "WallTop";
				wallR.name = "WallRight";
				wallB.name = "WallBottom";
				wallL.name = "WallLeft";

				wallT.transform.position = cell.transform.position + new Vector3(0, widthHeight, 0);
				wallR.transform.position = cell.transform.position + new Vector3(widthHeight, 0, 0);
				wallB.transform.position = cell.transform.position + new Vector3(0, -widthHeight, 0);
				wallL.transform.position = cell.transform.position + new Vector3(-widthHeight, 0, 0);

				Vector3 currScale = CalculateScale(rowcollCount);

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
		middleCell = grid[rowcollCount / 2, rowcollCount / 2].transform;
		mazeBackground.position = new Vector3(middleCell.position.x, middleCell.position.y, 0);
		StartCoroutine(CreatePath());
	}

	public int MazeLevel() {
		switch (Control.currDifficulty) {
			case 0: {
				getMazeSpeedMultiplier = 3;
				return 15;
			}

			case 1: {
				getMazeSpeedMultiplier = 2.75f;
				return 21;
			}

			case 2: {
				getMazeSpeedMultiplier = 2.75f;
				return 23;
			}

			case 3: {
				getMazeSpeedMultiplier = 1.75f;
				return 25;
			}

			case 4: {
				getMazeSpeedMultiplier = 1.5f;
				return 29;
			}
			default: {
				return -1;
			}
		}
	}

	public Vector3 CalculateScale(int rowColCount) {

		switch (rowColCount) {
			case 15: {
				return new Vector3(2.7f, 2.7f, 1);
			}
			case 21: {
				return new Vector3(2.1f, 2.1f, 1);
			}
			case 23: {
				return new Vector3(1.9f, 1.9f, 1);
			}
			case 25: {
				return new Vector3(1.7f, 1.7f, 1);
			}
			case 29: {
				return new Vector3(1.45f, 1.45f, 1);
			}
			default: {
				return Vector3.zero;
			}
		}
	}

	public IEnumerator CreatePath() {

		GameObject start = grid[0, 0];
		current = start;
		stack.Push(current);

		while (stack.Count > 0 && run == true) {
			if (stack.Count == 0) {
				MazeStopped();
			}
			else {
				chosenNeighbor = GetNeighbor(XPosition, Yposition);
			}

			yield return null;

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

			if (stack.Count <= 0) {
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

				MazeStopped();
			}
		}
		if (neighbors.Count > 0) {
			int choice = Random.Range(0, neighbors.Count);

			string name = neighbors[choice].name;
			//print(name);

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

	/// <summary>
	/// Returns a vector of maze array indexes
	/// </summary>
	/// <param name="antiBias">Supply a vector to prevent getting simillar results.</param>
	/// <returns></returns>
	public Vector2 GetEdgeCell(Vector2 antiBias = default(Vector2)) {
		Directions dir = (Directions)Random.Range(0, 4);
		switch (dir) {
			case Directions.TOP: {
				Vector2 vec = new Vector2(Random.Range(0, rowcollCount), rowcollCount - 1);
				if (antiBias != default(Vector2) && Vector2.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			case Directions.RIGHT: {
				Vector2 vec = new Vector2(0, Random.Range(0, rowcollCount));
				if (antiBias != default(Vector2) && Vector2.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			case Directions.BOTTOM: {
				Vector2 vec = new Vector2(Random.Range(0, rowcollCount), 0);
				if (antiBias != default(Vector2) && Vector2.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			case Directions.LEFT: {
				Vector2 vec = new Vector2(rowcollCount - 1, Random.Range(0, rowcollCount));
				if (antiBias != default(Vector2) && Vector2.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			default: {
				throw new System.Exception("Undefined state.");
			}
		}
	}


	public void MazeEscape() {
		Vector2 rndEdge = GetEdgeCell(playerEntrancePosition);
		teleport.transform.position = grid[(int)rndEdge.x, (int)rndEdge.y].transform.position;
	}

	public void MazeStopped() {
		if (neighbors.Count == 0 && stack.Count <= 1) {
			run = false;
			StopAllCoroutines();
			Vector2 rndSignPos = GetEdgeCell();
			infoBoardMaze.transform.position = grid[(int)rndSignPos.x, (int)rndSignPos.y].transform.position;
		}
	}

	public static float getMazeSpeedMultiplier {
		get { return _mazeSpeedMultiplier; }
		private set { _mazeSpeedMultiplier = value; }
	}

}
