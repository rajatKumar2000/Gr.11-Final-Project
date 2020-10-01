using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//healthbar script has been added to PlayerAnimController Script!
public class HealthBar : MonoBehaviour {
	public Image currentHealthBar;
	public Text percent;
	private PlayerAnimController playerScript;  //players script
	private int healthPoints;
	private int maxHealth;

 	void Start() {
		playerScript = GameObject.Find("Player").GetComponent<PlayerAnimController>(); //is the script for the player
	}
}
