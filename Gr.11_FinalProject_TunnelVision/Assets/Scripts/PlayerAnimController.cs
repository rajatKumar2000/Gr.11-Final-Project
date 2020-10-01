using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAnimController : MonoBehaviour {
	public float InitialHP = 100; //initial hp for player
	public float playerHP; //hp for the player
	private float ratio;
	public float moveSpeed = 3.5f; //speed at which player walks
	public float runSpeed = 6f;    //speed at which player runs
	public int rotatationSpeed = 80;  //speed at which player rotates
	public int strength = 10; //strength of players attack
	private float timeOfImpact; //used as timer for when player is hit
	private float healthLost;			//the loss of health of player
	private float impactDecision;	//decides which impact animation to play
	public Image currentHealthBar;
	public Text percent;
	private CapsuleCollider capCollider;	//is a trigger that allows enemies to see the player
	private Animator playerAnim;			//players animator controller
	private Rigidbody rbody;			//players rigidbody, allows for physics calculations
	private PlayerHitBox playersDamageScript;		//is the hitbox script attached to the players spine

	void Start () 
	{
		playerHP = InitialHP; 	//sets players hp
		rbody = GetComponent<Rigidbody> ();      //is rigidbody componenet of player
		playerAnim = GetComponent<Animator> ();  //is animator componenet of player
		rbody.freezeRotation = true;			 //stops any rotation done from physics engine
		playersDamageScript = GameObject.Find("/Player/Hips/Spine").GetComponent<PlayerHitBox>();		//access the script attached to players spine, and all of its variables
		capCollider = GetComponent<CapsuleCollider>(); //players range that the zombie can sense
	}	

	float ImpactOver(float impactTime){		//calculates the time that has passed since player was hit by enemy
		float timePassed;
		timePassed = (Time.time - impactTime);
		Debug.Log (timePassed);
		timeOfImpact = 0;
		return timePassed;
	}
	void DisableActions() {
		playerAnim.SetBool ("IsWalkForward", false);	//sets all animations to false
		playerAnim.SetBool ("IsRunning", false);
		playerAnim.SetBool ("IsWalkBackward", false);
		playerAnim.SetBool ("IsWalkRight", false);
		playerAnim.SetBool ("IsWalkLeft", false);
		playerAnim.SetBool ("IdleJump", false);
		playerAnim.SetBool ("IsRunningJump",false);
		playerAnim.SetBool ("IsSlash", false);
		playerAnim.SetBool ("IsWalkJump", false);
		playerAnim.SetBool ("IsAttackThree", false);
		//playerAnim.SetBool ("IsJumpAttack", false);
		transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (0).GetChild (5).GetChild (0).gameObject.SetActive (false); //disables the hitbox for the sword
	}
	void EnterDeath() {
		if (impactDecision > -1 & impactDecision < 7) { //if the number is between 0 - 6 
			playerAnim.SetBool ("IsDeathBack", true); //animate the player to die backwards as their last impact before death was being hit backwards
			playerAnim.SetBool ("ImpactBack", false); //stop impact anim
		} else { // if the number is between 7 - 10 
			playerAnim.SetBool ("IsDeathForward", true); //animate the player to die forwards as their last impact before death was being hit forwards
			playerAnim.SetBool ("ImpactForward", false); //stop impact anim
		}
		timeOfImpact = Time.time;
	}

	void Update () {
		healthLost = playersDamageScript.damageTaken;	//health lost is how much hp the player has lost due to damage
		if (playerHP == 3429) {				//this section of the if statement checks to see if player is dead
			DisableActions();
			if ((Time.time - timeOfImpact) > 2) {
			playerAnim.SetBool ("IsDeathBack", false);
			playerAnim.SetBool ("IsDeathForward", false);
			capCollider.enabled = false;	//enemies cannot "see" player if this is disabled so they dont attack the dead body
			GameObject.Find("Enemies").SetActive (false);
			GameObject.Find("Boss").SetActive (false);
			transform.GetChild (6).gameObject.SetActive (true);
			GameObject.Find ("UI").transform.GetChild (5).gameObject.SetActive (true);
			}
		} else {
			//	Debug.Log ("players hp: " +playerHP);
			 
			if (timeOfImpact > 0) {				//after 0.03 seconds, the players impact animation is completed
				if (ImpactOver (timeOfImpact) > 0.0001) {
//					Debug.Log ("reached here");
					playerAnim.SetBool ("ImpactBack", false);
					playerAnim.SetBool ("ImpactForward", false);
				}
			}
			if (playerHP == (InitialHP - healthLost)) {		//executes players rotation and actions if the player has not lost any hp
				PlayerRotation ();    //Rotates the player
				PlayerActions ();	  //Animates the player
			} else {
				playerHP = (InitialHP - healthLost);	//calculates how much hp is remaining for the player
				impactDecision = Random.Range (0, 11); //creates random number from 0 - 10
				if (impactDecision > -1 & impactDecision < 7) { //if the number is between 0 - 6 
					playerAnim.SetBool ("ImpactBack", true); //animate the player to do a basic impact animation
				} else { // if the number is between 7 - 10 
					playerAnim.SetBool ("ImpactForward", true); //animate the player to do a different impact animation
				}	
				timeOfImpact = Time.time; //time of impact for when the player gets hit by the enemy
				ratio = (playerHP / InitialHP);
				if (playerHP > 0) {
					percent.text = (playerHP + "/" + InitialHP);
					currentHealthBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
				}
				else {
					percent.text = ( "0/" + InitialHP);
					playerHP = 3429;		//3429 is a number that is used to check in Update function to decide if player is dead or alive
					currentHealthBar.rectTransform.localScale = new Vector3 (0, 1, 1);
					DisableActions();
					EnterDeath();
				}
			}
		}	
	}
	void FixedUpdate () {
		PlayerMovement ();	// Moves the player
	}

	void PlayerRotation() {
		float rotation = Input.GetAxis ("Mouse X") * rotatationSpeed * Time.deltaTime; //determines value of how much to rotate player
		transform.Rotate (0, rotation, 0); //Rotates player on the y axis
	}
		
	void PlayerActions() {
		if (this.playerAnim.GetCurrentAnimatorStateInfo (0).IsName ("AttackOne")) {
			//make sure there is no movement with player attacking
			playerAnim.SetBool ("IsAttackOne", false);
		} else if (this.playerAnim.GetCurrentAnimatorStateInfo (0).IsName ("AttackTwo")) {
			//make sure there is no movement with player attacking
			playerAnim.SetBool ("IsAttackTwo", false);
		} 
		else if (playerAnim.GetBool ("IsAttackOne")) {
			//make sure there is no movement with player attacking
		}
		else if (playerAnim.GetBool ("IsAttackTwo")) {
			//make sure there is no movement with player attacking
		}
		else {
			DisableActions ();
			if (Input.GetKey (KeyCode.W)) //if the player is pressing w
			{ 
				playerAnim.SetBool ("IsWalkForward", true); //animate the player to walk forward

				if (Input.GetKey (KeyCode.LeftShift)) { //if the player is pressing shift and w
					playerAnim.SetBool ("IsRunning", true); //animate the player to run forward

					if (Input.GetKey (KeyCode.Space)) { //if the player is pressing shift,w, and space
						playerAnim.SetBool ("IsRunningJump", true);  //animate the player to run and jump
					//	if (Input.GetKey (KeyCode.Mouse0)) {
						//	playerAnim.SetBool ("IsJumpAttack", true); //this is jump attack
					//	}
					}
				} 
				else if (Input.GetKey (KeyCode.Space))  //if the player is pressing w and space
				{
					playerAnim.SetBool ("IsWalkJump", true); //animate the player to walk and jump
				}
			} 
			else if (Input.GetKey (KeyCode.S)) //if the player is pressing s
			{   
				playerAnim.SetBool ("IsWalkBackward", true); //animate the player to walk backwards
			} 
			else if (Input.GetKey (KeyCode.D)) //if the player is pressing d
			{  
				playerAnim.SetBool ("IsWalkRight", true); //animate the player to walk right
			} 
			else if (Input.GetKey (KeyCode.A)) //if the player is pressing a
			{ 
				playerAnim.SetBool ("IsWalkLeft", true); //animate the player to walk left
			} 
			else if (Input.GetKey (KeyCode.Space)) //if the player is pressing space
			{  
				playerAnim.SetBool ("IdleJump", true); //animate the player to jump on the spot
			} 
			else if (Input.GetKey (KeyCode.Mouse1)) //if the player right clicks on the mouse
			{ 
				playerAnim.SetBool ("IsSlash", true); //animate the player to block with his shield
				transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (0).GetChild (5).GetChild (0).gameObject.SetActive (true); //enables the hitbox for the players sword
			} 
			else if (Input.GetKey (KeyCode.R)) 
			{
				playerAnim.SetBool ("IsAttackThree", true);
				transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (0).GetChild (5).GetChild (0).gameObject.SetActive (true); //enables the hitbox for the players sword
			}
			else if (Input.GetKey (KeyCode.Mouse0)) //if the player left clicks on the mouse
			{ 
				transform.GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (0).GetChild (5).GetChild (0).gameObject.SetActive (true); //enables the hitbox for the players sword
				int chances = Random.Range (0, 11); //creates random number from 0 - 10

				if (chances > -1 & chances < 7) //if the number is between 0 - 6 
				{  
					playerAnim.SetBool ("IsAttackOne", true); //animate the player to do a basic attack animation
				} 
				else // if the number is between 7 - 10 
				{ 
					playerAnim.SetBool ("IsAttackTwo", true); //animate the player to do a different basic attack animation

				}
			}
		}
	}
		
	void PlayerMovement() {
	   
		if (playerAnim.GetBool ("IsRunning") == true) //if the players animation is running forwards
		{
			rbody.velocity = (transform.forward * runSpeed); //the velocity of the player is moving forwards fast
		}
		else if (playerAnim.GetBool ("IsWalkForward") == true) //if the players animation is walking forwards
		{
			rbody.velocity = (transform.forward * moveSpeed); //the velocity of the player is moving forwards 
		} 
		else if (playerAnim.GetBool ("IsWalkBackward") == true) //if the players animation is walking backwards
		{
			rbody.velocity = (transform.forward * -moveSpeed); //the velocity of the player is moving backwards 
		}
		else if (playerAnim.GetBool ("IsWalkRight") == true) //if the players animation is walking to the right
		{
			rbody.velocity = (transform.right * moveSpeed); //the velocity of the player is moving right 
		} 
		else if (playerAnim.GetBool ("IsWalkLeft") == true) //if the players animation is walking to the left
		{
			rbody.velocity = (transform.right * -moveSpeed); //the velocity of the player is moving left 
		}
	}
}
