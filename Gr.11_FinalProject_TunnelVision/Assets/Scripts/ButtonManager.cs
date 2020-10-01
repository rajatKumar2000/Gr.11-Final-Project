using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

    public class ButtonManager : MonoBehaviour {
	private bool statsOpener = false;

	public void Openbtn(string Maze) {
        SceneManager.LoadScene(Maze);
    }

    public void ExitGamebtn() {
        Application.Quit();
    }

	public void ShowStatsbtn() {
		statsOpener = !statsOpener;
		GameObject.Find ("UI").transform.GetChild (4).gameObject.SetActive (statsOpener); 
	}

	public void LeaderBoardsbtn() {
		
	}
}
