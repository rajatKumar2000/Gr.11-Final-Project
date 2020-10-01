using UnityEngine;
using System.Collections;
// using zombies script as base for boss script
	public class BossAnimController : MonoBehaviour {
	public int BossHP = 20; //zombies health points
	public int BossSpeed = 1; //speed at which zombie walks
	private int hit = 0;		// the hits the zombies take
	private Animator zombieAnim; //animator for zombie
	private BoxCollider boxCol;  //hitbox for zombie
	private PlayerAnimController playerScript;  //players script
	private int attack = 0;	//the damage done per hit
	private float startAttack;  //the start time of the zombies attack
	public Transform target;  //some complicated thing, dont mess with this
	private bool inRange = false;
	private bool spotted = false;
	//public GameObject Exp;
	//GameObject myExpClone;

	void Start () {
		zombieAnim = GetComponent<Animator> ();  //is animator componenet of zombie
		boxCol = GetComponent<BoxCollider>(); //hitbox of zombie
		playerScript = GameObject.Find("Player").GetComponent<PlayerAnimController>(); //is the script for the player
		//myExpClone = Instantiate(Exp, transform.position, Quaternion.identity) as GameObject;
	}

	void Update () {
		attack = playerScript.strength;  //takes the value of strength from players script
		if (zombieAnim.GetBool ("IsDead")) { //zombie is dead
			//makes sure zombie doesnt rotate if its dead
		}
		else if (this.zombieAnim.GetCurrentAnimatorStateInfo (0).IsName ("ZombieAttack")) //if currently in attack position
		{
			ZombieRotation (); 
			if ((Time.time - startAttack) > 1.5) {  //once 1.5 seconds are passed since zombie attacks, it will "recharge" and attack again.
				zombieAnim.SetBool ("IsAttack", false); 
				startAttack = 0;
			//	boxCol.enabled = true;
			}
		}
		else if (zombieAnim.GetBool ("IsAttack")) {
			//so no changes happen while the bool is turned true
		}
		else 
		{
			transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).gameObject.SetActive (false); //disables the hitbox for the zombies arm
			ZombieHitDetection ();
			ZombieRotation ();
			if (spotted == true) {
				transform.Translate(Vector3.forward * BossSpeed * Time.deltaTime);
//Debug.Log ("can run at player!");
			}
		}
	}
	void ZombieHitDetection() {
		if (inRange == true) {
			Debug.Log ("In Attack Zone");
			transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).gameObject.SetActive (true); //enables the hitbox for the zombies arm
			zombieAnim.SetBool ("IsAttack", true);
			startAttack = Time.time;				//starts counter of when attack happened.
		} 
		else {
			
		}
	}

	void ZombieRotation() { //makes zombie follows player
		Vector3 relativePos = target.position - transform.position;
		transform.rotation = Quaternion.LookRotation (relativePos);
	}

	void OnTriggerEnter (Collider other){ //when the hitbox of the zombie is touched
		
		if (other.CompareTag ("PlayerSword"))  //if the players sword touches the zombie, then the zombie takes damage
		{									   // depending on value of damage, the zombie is either impacted or dead
			hit += attack;
			Debug.Log ("damage taken: " + hit);
			if (BossHP <= hit) {
				zombieAnim.SetBool ("IsDead", true);
				GameObject.Find ("UI").transform.GetChild (6).gameObject.SetActive (true);
				boxCol.enabled = false; 					//disables zombies hitbox, so no further things can happen to this gameobject
				zombieAnim.SetBool ("ZomWalk", false);
				transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).gameObject.SetActive (false); //disables the hitbox for the zombies arm

				//Instantiate(Exp);
			} else {
				zombieAnim.SetBool ("IsImpact", true);
			}
		}
		if (other.CompareTag ("Player")) {		//if the player is within range of the zombie, it will attack the player
			//boxCol.enabled = false;
			inRange = true;
			spotted = false;
			zombieAnim.SetBool ("ZomWalk", false);
		}
		if (other.CompareTag ("WithinRange")) {		//if the player is within range of the zombie, it will run to player
			spotted = true;
			zombieAnim.SetBool ("ZomWalk", true);
		}
	}

	void OnTriggerExit (Collider other) {			//once the sword is no longer touching the hitbox, the impact animation will stop playing.
		if (other.CompareTag ("Player")) {
			Debug.Log ("Out of zone!");
			inRange = false;
			spotted = true;
			zombieAnim.SetBool ("ZomWalk", true);
		}
		if (other.CompareTag ("WithinRange")) {
			spotted = false;
			zombieAnim.SetBool ("ZomWalk", false);
		}
		zombieAnim.SetBool ("IsImpact", false);
	}
}
