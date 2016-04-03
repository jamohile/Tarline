using UnityEngine;
using System.Collections;

public class MainGameClicks : MonoBehaviour {
	GameObject player;
	public Sprite Retry_Down;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		//handle normal rotate clicks
		switch(MainGameManager.current_game_state){
			case MainGameManager.game_state.PreStart:
			MainGameManager.Start_Game();
			break;
			case MainGameManager.game_state.Playing:
			if (MainGameManager.player.GetComponentInChildren<Animator> ().GetBool ("RotatedUp") == true) {
				MainGameManager.player.GetComponentInChildren<Animator> ().SetBool ("RotatedUp", false);
				MainGameManager.player.GetComponentInChildren<Animator> ().Play ("ToUp");

			} else {
				MainGameManager.player.GetComponentInChildren<Animator> ().SetBool ("RotatedUp", true);
				MainGameManager.player.GetComponentInChildren<Animator> ().Play ("ToDown");

				
			}
			break;

	}
	}
}
