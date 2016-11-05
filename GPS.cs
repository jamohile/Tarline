using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPS : MonoBehaviour {
	public static string leaderboard ="CgkIjazYnLIBEAIQBg" ;
	// Use this for initialization
	void Start(){
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	public static void AwardAchievement(string achievementName){
		string achievementString = "";
		switch (achievementName) {
		case "FirstGame":
			achievementString = GPGS_CONTANTS.achievement_first_game;
			break;
		case "Bronze":
			achievementString = GPGS_CONTANTS.achievement_bronze;
			break;
		case "Silver":
			achievementString = GPGS_CONTANTS.achievement_silver;
			break;
		case "Gold":
			achievementString = GPGS_CONTANTS.achievement_gold;
			break;
		case "Platinum":
			achievementString = GPGS_CONTANTS.achievement_platinum;
			break;
		}
		if (Social.localUser.authenticated == true) {
			Social.ReportProgress (achievementString, 100.0f, (bool success) => {
				//succesful post
			});
		}
	}
	public static void ReportHighScore(int score){
		if (Social.localUser.authenticated == true) {
			Social.ReportScore(score,leaderboard, (bool success) => {
				//achievement posted succesfully
			});
		}
	}
}
