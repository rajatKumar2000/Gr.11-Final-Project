using UnityEngine;
using System.Collections;

public class ChangeWallTextures : MonoBehaviour {
	private int x;
	public Texture myTexture; 


	void Start () {
	//	GameObject Walls = GameObject.Find ("Walls");
	//	for (x = 0; x > Walls.transform.childCount; x++) {
		//	Walls.transform.GetChild (x).GetComponent<Renderer> ().material.mainTexture = textures;
		//	Debug.Log ("working");
		Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers)
		{
			r.material.mainTexture = myTexture;
		}
	}
}

