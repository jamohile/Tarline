using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Splash : MonoBehaviour {
	SpriteRenderer logo_spriteRenderer;
	float alpha;
	float animationTime;
	float initialTime;
	// Use this for initialization
	void Start () {
		//Start a DataManager Object
		gameObject.AddComponent<DataManager> ();
		//manage saved data, will be done later

	}
	
	// Update is called once per frame
	void Update () {
	}

	void Start_Game(){
		//configure the google play games platform

		SceneManager.LoadScene("StartMenu");
	}



}
