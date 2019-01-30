using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public GameObject cellPrefab;
	public GameObject wallVertical;
	public GameObject wallHorizontal;
	public GameObject wallVerticalDmg;
	public GameObject wallHorizontalDmg;

	public GameObject escapeTeleport;
	public RectTransform mazeBackground;

	public GameObject[,] grid;
	public Transform middleCell;

	public Vector2Int playerEntrancePosition;

	private GameObject infoBoardMaze;

	private Stack<GameObject> stack = new Stack<GameObject>();

	private int rowColCount;
	private float cellWidth;

	public float mazeSize { get { return rowColCount * cellWidth; } }
	public static float getMazeSpeedMultiplier { get; private set; }

	private GameObject current;
	private GameObject chosenNeighbor;
	private int xPos = 0;
	private int yPos = 0;

	private void Start() {
		infoBoardMaze = gameObject.GetComponentInChildren<SignPost>().gameObject;
		rowColCount = MazeLevel();

		grid = new GameObject[rowColCount, rowColCount];

		//Stupd hardcoded value...
		cellWidth = 200 / (rowColCount);
		Vector3 currScale = CalculateScale(rowColCount);

		for (int i = 0; i < rowColCount; i++) {
			for (int j = 0; j < rowColCount; j++) {

				GameObject cell = Instantiate(cellPrefab, transform.position + new Vector3((i * cellWidth * 2), (j * cellWidth * 2), 0), Quaternion.identity, gameObject.transform);
				cell.name = "Cell " + i + " " + j;

				Cell cellScript = cell.GetComponent<Cell>();
				cellScript.selfX = i;
				cellScript.selfY = j;

				GameObject wallT = Instantiate(Control.currDifficulty < 3 ? wallHorizontal : wallHorizontalDmg, cell.transform);
				GameObject wallR = Instantiate(Control.currDifficulty < 3 ? wallVertical : wallVerticalDmg, cell.transform);
				GameObject wallB = Instantiate(Control.currDifficulty < 3 ? wallHorizontal : wallHorizontalDmg, cell.transform);
				GameObject wallL = Instantiate(Control.currDifficulty < 3 ? wallVertical : wallVerticalDmg, cell.transform);

				wallT.name = "WallTop";
				wallR.name = "WallRight";
				wallB.name = "WallBottom";
				wallL.name = "WallLeft";

				wallT.transform.position = cell.transform.position + new Vector3(0, cellWidth, 0);
				wallR.transform.position = cell.transform.position + new Vector3(cellWidth, 0, 0);
				wallB.transform.position = cell.transform.position + new Vector3(0, -cellWidth, 0);
				wallL.transform.position = cell.transform.position + new Vector3(-cellWidth, 0, 0);

				wallT.transform.localScale = currScale;
				wallR.transform.localScale = currScale;
				wallB.transform.localScale = currScale;
				wallL.transform.localScale = currScale;

				grid[i, j] = cell;
			}
		}
		middleCell = grid[rowColCount / 2, rowColCount / 2].transform;
		mazeBackground.position = middleCell.position;
		StartCoroutine(CreatePath());
	}

	private int MazeLevel() {
		switch (Control.currDifficulty) {
			case 0: {
				getMazeSpeedMultiplier = 3;
				return 15;
			}
			case 1: {
				getMazeSpeedMultiplier = 2.75f;
				return 19;
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
		}
		throw new System.InvalidOperationException("Unknown difficulty entered " + Control.currDifficulty);
	}

	private Vector3 CalculateScale(int rowColCount) {
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
		}
		throw new System.InvalidOperationException("Unexpected rowColCount " + rowColCount);
	}

	private IEnumerator CreatePath() {
		current = grid[0, 0];
		stack.Push(current);

		while (stack.Count > 0) {
			chosenNeighbor = GetRandomNeighbor(xPos, yPos, ref current);
			string nameCurrent;
			string nameNeighbor;

			yield return null;
			if (chosenNeighbor.transform.position.x < current.transform.position.x) {
				 nameCurrent = "WallLeft";
				 nameNeighbor = "WallRight";
			}
			else if (chosenNeighbor.transform.position.x > current.transform.position.x) {
				 nameCurrent = "WallRight";
				 nameNeighbor = "WallLeft";
			}
			else if (chosenNeighbor.transform.position.y < current.transform.position.y) {
				 nameCurrent = "WallBottom";
				 nameNeighbor = "WallTop";
			}
			else {
				 nameCurrent = "WallTop";
				 nameNeighbor = "WallBottom";
			}

			current.transform.Find(nameCurrent).gameObject.SetActive(false);
			chosenNeighbor.transform.Find(nameNeighbor).gameObject.SetActive(false);

			stack.Push(chosenNeighbor);
			current = chosenNeighbor;
		}
	}


	private GameObject GetRandomNeighbor(int currentPosX, int currentPosY, ref GameObject newCurrent) {
		GameObject oldCurrent = grid[currentPosX, currentPosY];
		List<Cell> cells = new List<Cell>();

		if (currentPosY + 1 <= rowColCount - 1) {
			Cell c = grid[currentPosX, currentPosY + 1].GetComponent<Cell>();
			if (!c.visited) {
				cells.Add(c);
			}
		}
		if (currentPosX + 1 <= rowColCount - 1) {
			Cell c = grid[currentPosX + 1, currentPosY].GetComponent<Cell>();
			if (!c.visited) {
				cells.Add(c);
			}
		}
		if (currentPosY - 1 >= 0) {
			Cell c = grid[currentPosX, currentPosY - 1].GetComponent<Cell>();
			if (!c.visited) {
				cells.Add(c);
			}
		}
		if (currentPosX - 1 >= 0) {
			Cell c = grid[currentPosX - 1, currentPosY].GetComponent<Cell>();
			if (!c.visited) {
				cells.Add(c);
			}
		}

		if (cells.Count == 0) {
			if (stack.Count == 0) {
				FinalizeMazeGeneration();
				return null;
			}
			Cell c = stack.Pop().GetComponent<Cell>();

			xPos = c.selfX;
			yPos = c.selfY;
			newCurrent = oldCurrent;
			return GetRandomNeighbor(xPos, yPos, ref newCurrent);
		}

		int selectedIndex = Random.Range(0, cells.Count);

		xPos = cells[selectedIndex].selfX;
		yPos = cells[selectedIndex].selfY;
		cells[selectedIndex].visited = true;
		newCurrent = oldCurrent;
		return cells[selectedIndex].gameObject;
	}

	/// <summary>
	/// Returns a vector of maze array indexes
	/// </summary>
	/// <param name="antiBias">Supply a vector to prevent getting simillar results.</param>
	public Vector2Int GetEdgeCell(Vector2Int antiBias = default) {
		Directions dir = (Directions)Random.Range(0, 4);
		switch (dir) {
			case Directions.TOP: {
				Vector2Int vec = new Vector2Int(Random.Range(0, rowColCount), rowColCount - 1);
				if (antiBias != default && Vector2Int.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			case Directions.RIGHT: {
				Vector2Int vec = new Vector2Int(0, Random.Range(0, rowColCount));
				if (antiBias != default && Vector2Int.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			case Directions.BOTTOM: {
				Vector2Int vec = new Vector2Int(Random.Range(0, rowColCount), 0);
				if (antiBias != default && Vector2Int.Distance(antiBias, vec) <= 5) {
					return GetEdgeCell(antiBias);
				}
				else {
					return vec;
				}
			}
			case Directions.LEFT: {
				Vector2Int vec = new Vector2Int(rowColCount - 1, Random.Range(0, rowColCount));
				if (antiBias != default && Vector2Int.Distance(antiBias, vec) <= 5) {
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
		Vector2Int rndEdge = GetEdgeCell(playerEntrancePosition);
		escapeTeleport.transform.position = grid[rndEdge.x, rndEdge.y].transform.position;
	}

	private void FinalizeMazeGeneration() {
		StopAllCoroutines();
		Vector2Int rndSignPos = GetEdgeCell();
		infoBoardMaze.transform.position = grid[rndSignPos.x, rndSignPos.y].transform.position;
	}
}
