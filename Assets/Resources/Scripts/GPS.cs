using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPS : MonoBehaviour {
	public static Dictionary<string,string> achievements= new Dictionary<string,string>();
	public static string leaderboard ="CgkIjazYnLIBEAIQBg" ;
	// Use this for initialization
	void Start(){
		//basic achivements
		achievements.Add ("FirstGame", "CgkIjazYnLIBEAIQAQ" );
		//distance based achievements
		achievements.Add ("Bronze", "CgkIjazYnLIBEAIQAg" );
		achievements.Add ("Silver", "CgkIjazYnLIBEAIQAw" );
		achievements.Add ("Gold", "CgkIjazYnLIBEAIQBA" );
		achievements.Add ("Platinum", "CgkIjazYnLIBEAIQBQ" );

	}
	
	// Update is called once per frame
	void Update () {
	}
	public static void AwardAchievement(string achievementName){
		if (Social.localUser.authenticated == true) {
			PlayGamesPlatform.Instance.ReportProgress (achievements [achievementName], 100.0f, (bool success) => {
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
