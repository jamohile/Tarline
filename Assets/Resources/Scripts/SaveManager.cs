﻿using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour {
	public static bool has_been_opened_before = false;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey ("OPENED_BEFORE") == true) {
			has_been_opened_before = true; // indicates the game has been opened before
		} else {
			PlayerPrefs.SetInt ("OPENED_BEFORE",1);
			//first time setup
			PlayerPrefs.SetInt("HighScore",0);
			PlayerPrefs.SetInt ("Coins", 0);
			has_been_opened_before =  false;
			PlayerPrefs.Save();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}

	public static bool IsFirstTime(){
		return has_been_opened_before;
	}
	public static int GetHighScore(){
		return PlayerPrefs.GetInt("HighScore");
	}
	public static void SetHighScore(int highscore){
		PlayerPrefs.SetInt ("HighScore", highscore);
		PlayerPrefs.Save ();
	}
	public static int GetCoins(){
		return PlayerPrefs.GetInt("Coins");
	}
	public static void SetCoins(int coins){
		PlayerPrefs.SetInt ("Coins", coins);
		PlayerPrefs.Save ();
	}
	public static void Save(){
		PlayerPrefs.Save ();
	}
}
	