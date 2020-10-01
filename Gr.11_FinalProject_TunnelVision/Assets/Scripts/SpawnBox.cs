using UnityEngine;
using System.Collections;

public class SpawnBox : MonoBehaviour {

	//The enemies
	private GameObject spawnBoxHolder;
	public GameObject spawner; 

	//Maze size and number of enemies
	public float enemyDistence;
	public int xSize; 
	public int ySize;
	public int enemyCellDistence;
	public int cellSize; 

	private int enemyType;//used to see which enemy will be generated

	private Vector3 initialPos;
	private Vector3 enemyPos;

	// Use this for initialization
	void Start () {
		spawnBoxHolder = new GameObject ();
		spawnBoxHolder.name = "Spawn Box";
		GameObject tempSpawner;

		initialPos = new Vector3 ((-(xSize / 2)) * cellSize + (cellSize / 2), 0.0f, (-(ySize / 2)) * cellSize + (cellSize / 2)); 

		//Loop used to generate the enemies
		for (int i = 0; i < (ySize / enemyCellDistence); i++) {
			for (int j = 0; j <= ((xSize - 1) / enemyCellDistence); j++) {
				//Pick which random enemy will be generated

				//Calculate position at which new enemy will be generated and genertate that enemy
				enemyPos = new Vector3 (initialPos.x + (j * enemyDistence), 0.0f, initialPos.x + (i * enemyDistence));
				tempSpawner = Instantiate (spawner, enemyPos, Quaternion.identity) as GameObject;
				tempSpawner.transform.parent = spawnBoxHolder.transform;
			}
		}

	}		
}
