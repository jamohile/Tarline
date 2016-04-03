using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class StartMenuManager : MonoBehaviour {
	public static GameObject coinReadout;
	public static GameObject highScoreReadout;

	public static int coins;
	public static int HighScore;
	public static string currentCharacter;


	// Use this for initialization
	void Start () {
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


		HighScore = SaveManager.GetHighScore();
		coins = SaveManager.GetCoins ();
		currentCharacter = SaveManager.GetCharacter();

		highScoreReadout = GameObject.FindGameObjectWithTag ("HighScore");
		highScoreReadout.GetComponent<TextMesh> ().text = HighScore.ToString ();
		coinReadout = GameObject.FindGameObjectWithTag ("CoinBanner");
		coinReadout.GetComponentInChildren<TextMesh> ().text = coins.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
