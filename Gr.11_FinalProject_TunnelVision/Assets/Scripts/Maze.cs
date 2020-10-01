using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
	[System.Serializable]
	//Used to keep track of infromation on a cell
	public class Cell {
		public bool visited; //If the cell was visited
		public GameObject north; //1
		public GameObject east; //2
		public GameObject west; //3
		public GameObject south; //4
	}

	public GameObject wall; //gameobject that will be generated as the maze walls 
	public float wallLength = 1.0f; //length of the wall
	public float wallHeight = 1.0f; //Height of the wall 
	private GameObject wallHolder; //to orgenize the walls created
	private Cell[] cells;
	private int currentCell = 0;
	private int visitedCells = 0;
	private bool startedBuilding; //See if the maze has already started to be built
	private int currentNeighbour = 0;
	private List<int> lastCells;
	private int backingUp = 0;
	private int wallToBreak;

	//size of the maze
	public int xSize = 5; 
	public int ySize = 5;
    private int totalCells;

    private Vector3 initialPos; //where maze will start to be generated from

	// Use this for initialization
	void Start () {
		CreateWalls ();
	}

	//generate inital walls
	void CreateWalls (){
		
		//used to orginize the walls created
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";
		GameObject tempWall;

		//where maze will start to be generated from
		initialPos = new Vector3 ((-xSize / 2)* wallLength + wallLength/2, 0.0f, (-ySize/2)* wallLength+ wallLength);
		Vector3 myPos = initialPos;

		//for x axis 
		for (int i = 0; i < ySize; i++){
			for (int j = 0; j <= xSize; j++){
				//Calculate position at which new wall will be generated and genertate that wall
				myPos = new Vector3 (initialPos.x + (j * wallLength) - wallLength / 2, wallHeight/2, initialPos.z + (i * wallLength) - wallLength / 2);
				tempWall = Instantiate (wall, myPos, Quaternion.identity) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}

		//for y axis 
		for (int i = 0; i <= ySize; i++){
			for (int j = 0; j < xSize; j++){
				//Calculate position at which new wall will be generated and genertate that wall
				myPos = new Vector3 (initialPos.x + (j * wallLength), wallHeight/2, initialPos.z + (i * wallLength) - wallLength);
				tempWall = Instantiate (wall, myPos, Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}

		CreateCells ();
	}

	//This creates the cells
	void CreateCells(){
		lastCells = new List<int> ();
		lastCells.Clear ();
        totalCells = (xSize * ySize);
        GameObject[] allWalls; //Hold the walls
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject[children];
		cells = new Cell[xSize * ySize];
		int eastWestProcess = 0;
		int childProcess = 0;
		int termCount = 0;

		//Gets All of the children
		for (int i = 0; i < children; i++) {
			allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
		}
				
		//Assigns walls to the cells
		for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++){

			if (termCount == xSize) {
				eastWestProcess ++;
				termCount = 0;
			} 

			cells[cellprocess] = new Cell ();
			cells[cellprocess].east = allWalls[eastWestProcess];
			cells[cellprocess].south = allWalls[childProcess+(xSize+1)*ySize];

			eastWestProcess++;
			termCount++;
			childProcess++;

			cells[cellprocess].west = allWalls[eastWestProcess];
			cells[cellprocess].north = allWalls[(childProcess+(xSize+1)*ySize)+xSize-1];
		}

		CreateMaze ();
	}

	//Makes the actual maze
	void CreateMaze(){
		while (visitedCells < totalCells) {
			if (startedBuilding) {
				GiveMeNeighbour ();

				if (cells [currentNeighbour].visited == false && cells [currentCell].visited == true) { //if the the program has visited a new cell
					BreakWall ();
					cells [currentNeighbour].visited = true;
					visitedCells++;
					lastCells.Add (currentCell);
					currentCell = currentNeighbour;

					if (lastCells.Count > 0) {
						backingUp = lastCells.Count - 1;
					}
				}	
			} else {
				currentCell = Random.Range (0, totalCells);
				cells [currentCell].visited = true;
				visitedCells++;
				startedBuilding = true;
			}
				
		}

		Debug.Log("Finished");
	}

	//Breaks the walls between neighbours
	void BreakWall (){
		switch (wallToBreak) {
		case 1: Destroy (cells [currentCell].north); break;
		case 2: Destroy (cells [currentCell].east); break;
		case 3: Destroy (cells [currentCell].west); break;
		case 4: Destroy (cells [currentCell].south); break;
		}
	}

	//Find Neighbours
	void GiveMeNeighbour(){
		int length = 0;
		int[] neighbours = new int[4];
		int[] connectingWall = new int[4];	
		int check = 0;

		check = ((((currentCell + 1) / xSize) - 1) * xSize) + xSize; //see if current cell is on the edge of the map 

		//west
		if (currentCell + 1 < totalCells && (currentCell + 1) != check) {
			if (cells [currentCell + 1].visited == false) {
				neighbours [length] = currentCell + 1;
				connectingWall [length] = 3;
				length++;
			}
		}

		//east
		if (currentCell - 1 >= 0 && currentCell != check) {
			if (cells [currentCell - 1].visited == false) {
				neighbours [length] = currentCell - 1;
				connectingWall [length] = 2;
				length++;
			}
		}

		//north
		if (currentCell + xSize < totalCells) {
			if (cells [currentCell + xSize].visited == false) {
				neighbours [length] = currentCell + xSize;
				connectingWall [length] = 1;
				length++;
			}
		}

		//south
		if (currentCell - xSize >= 0) {
			if (cells [currentCell - xSize].visited == false) {
				neighbours [length] = currentCell - xSize;
				connectingWall [length] = 4;
				length++;
			}
		}

		if (length != 0) {
			int rand= Random.Range(0, length);
			currentNeighbour = neighbours [rand];
			wallToBreak = connectingWall [rand];
		} else {
			if (backingUp > 0) {
				currentCell = lastCells [backingUp];
				backingUp--;
			}
		}

	}
	///////
}