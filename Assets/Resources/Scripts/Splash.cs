﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Splash : MonoBehaviour {
	SpriteRenderer logo_spriteRenderer;
	float alpha;
	float animationTime;
	float initialTime;
	// Use this for initialization
	void Start () {
		//Start a DataManager Object
		gameObject.AddComponent<DataManager> ();

		//set up the alpha effect
		logo_spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		animationTime = 1f;
		//manage saved data, will be done later
		MainGameManager.GPGS_Logged_In = true;

	}
	
	// Update is called once per frame
	void Update () {
		//animate the alpha fade in
		if (Time.time <= animationTime) {
			alpha = Time.time / animationTime;
			logo_spriteRenderer.color = new Color (1f, 1f, 1f, alpha);
		} else {
			if (initialTime == 0){
				initialTime = Time.time;
				alpha = 1;
			}

			Debug.Log(alpha);
			logo_spriteRenderer.color = new Color (1f, 1f, 1f, alpha);
			alpha-= animationTime/100;

			if (alpha < 0.05){
				
				Start_Game();
			}
		}
	}

	void Start_Game(){
		logo_spriteRenderer.color = new Color (1f, 1f, 1f, 0f);
		//configure the google play games platform
		PlayGamesPlatform.Activate();
		//login
		Social.localUser.Authenticate ((bool success) => {
			if (success) {
				Debug.Log ("Login Succesful");
				MainGameManager.GPGS_Logged_In = true;
			} else {
				Debug.Log ("Login Failed");
			}

		});




		SceneManager.LoadScene("StartMenu");
	}



}
