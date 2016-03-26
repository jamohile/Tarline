using UnityEngine;
using System.Collections;

public class StartMenuManager : MonoBehaviour {
	public static GameObject coinReadout;
	public static GameObject highScoreReadout;

	public static int coins;
	public static int HighScore;


	// Use this for initialization
	void Start () {
		HighScore = SaveManager.GetHighScore();
		coins = SaveManager.GetCoins ();

		highScoreReadout = GameObject.FindGameObjectWithTag ("HighScore");
		highScoreReadout.GetComponent<TextMesh> ().text = HighScore.ToString ();
		coinReadout = GameObject.FindGameObjectWithTag ("CoinBanner");
		coinReadout.GetComponentInChildren<TextMesh> ().text = coins.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
