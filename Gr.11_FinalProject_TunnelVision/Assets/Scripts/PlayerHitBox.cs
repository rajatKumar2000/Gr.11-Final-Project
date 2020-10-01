using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHitBox : MonoBehaviour {
	public float damageTaken; 
	private GameObject bossCamera;
	private MeshRenderer wallMesh;
	private BoxCollider wallCol;
	private Animator bossAnim;
	private float timeOfAnim; //used as timer for animation clip

	void OnTriggerEnter(Collider other) {
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;
		if (other.CompareTag ("ZombieHand")) {
			if (sceneName == "LevelOne") {
			damageTaken += 5;
			}
			else if (sceneName == "LevelTwo") {
				damageTaken += 7.5f;
			}
			else if (sceneName == "LevelThree") {
				damageTaken += 10;
			}
		}
		if (other.CompareTag ("BossHand")) {
			if (sceneName == "LevelOne") {
				damageTaken += 10;
			}
			else if (sceneName == "LevelTwo") {
				damageTaken += 13;
			}
			else if (sceneName == "LevelThree") {
				damageTaken += 15;
			}
		}
		if (other.CompareTag ("BossFight")) {
			Debug.Log ("Play Boss Fight animation");
			bossCamera = GameObject.Find ("BossRoomCam");
			bossCamera.transform.GetChild (0).gameObject.SetActive (true); //enables the camera for animation clip 
			GameObject.Find ("Player").GetComponent<PlayerAnimController> ().enabled = false;
			timeOfAnim = Time.time;
			GameObject.Find ("Boss").GetComponent<BossAnimController> ().enabled = true;
		}

		if (other.CompareTag ("Exp")) {
			Destroy(GameObject.Find("Exp"));
		}
	}
	void OnTriggerStay(Collider other) {
		if (other.CompareTag ("BossFight")) {
			if ((Time.time - timeOfAnim) > 7) {
				bossCamera.transform.GetChild (0).gameObject.SetActive (false); //disables the camera for animation clip 
				wallMesh = GameObject.Find ("BossWall").GetComponent<MeshRenderer> ();
				wallCol = GameObject.Find ("BossWall").GetComponent<BoxCollider> ();
				wallMesh.enabled = true;
				wallCol.enabled = true;	
				//bossAnim = GameObject.Find ("Boss").GetComponent<Animator> ();
				//bossAnim.SetBool ("IsIdle", true);
				GameObject.Find ("Player").transform.position = new Vector3 (-3f, 0.5f, -1.55f);
				GameObject.Find ("Player").GetComponent<PlayerAnimController> ().enabled = true;
			}
		}
	}
}
