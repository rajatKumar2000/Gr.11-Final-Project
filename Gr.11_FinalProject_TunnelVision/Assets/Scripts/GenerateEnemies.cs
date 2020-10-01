using UnityEngine;
using System.Collections;

public class GenerateEnemies: MonoBehaviour {
	//The enemies
	public GameObject badGuy1;
	public GameObject badGuy2;
	public GameObject badGuy3;
	private GameObject enemy; 
	public GameObject spawnBox;
	public GameObject enemyHolder;

	private int enemyType;//used to see which enemy will be generated
	private int t;

	private Vector3 initialPos;
	private Vector3 enemyPos;

	// Use this for initialization
	void Start () {
		Spawn ();
	}

	//Makes new Enemies every 18000 frames (5 to 10 min)
	void FixedUpdate(){
		t++;

		if (t == 18000) {
			Spawn ();
			t = 0;
		}
	}

	//Spawns random Enemies
	void Spawn(){
		//Used to orginize the enemies
		GameObject tempEnemy;

			//Pick which random enemy will be generated
			enemyType = Random.Range (1, 4);
			switch (enemyType) {
			case 1: enemy = badGuy1; break;
			case 2: enemy = badGuy2; break;
			case 3: enemy = badGuy3; break;
			}

			//Calculate position at which new enemy will be generated and genertate that enemy
			enemyPos = spawnBox.transform.position;
			tempEnemy = Instantiate (enemy, enemyPos, Quaternion.identity) as GameObject;
			tempEnemy.transform.parent = enemyHolder.transform;
	}
}	
	
