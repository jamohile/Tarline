using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour {
	public enum optionChosen{
		nothing,
		revive,
		norevive
	}
	public static optionChosen chosen = optionChosen.nothing;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		gameObject.transform.position = new Vector3 (Camera.main.transform.position.x, gameObject.transform.position.y, -1.4f);
	}
	public void revivePlayer(int cost){
		MainGameManager.current_game_state = MainGameManager.game_state.GameOver;
		MainGameManager.player.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
		StartCoroutine (lookForInput (cost));
			
	}
	public void TimeOutNoRevive(){
		chosen = optionChosen.norevive;
	}
	public void DestoryNotification(){
		Destroy (gameObject);
	}
	IEnumerator lookForInput(int cost){
		do {
			if (chosen == optionChosen.norevive) {
				gameObject.GetComponent<Animator>().Play("Notification_Out");
				yield return new WaitForSeconds(0.3f);
				chosen = optionChosen.nothing;
				MainGameManager.Game_Over ();
				yield break;
			}else if(chosen == optionChosen.revive){
				chosen = optionChosen.nothing;
				gameObject.GetComponent<Animator>().Play("Notification_Out");
				if (MainGameManager.player.GetComponentInChildren<Animator> ().GetBool ("RotatedUp") == false) {
					MainGameManager.player.GetComponentInChildren<Animator> ().SetBool ("RotatedUp", true);
					MainGameManager.player.GetComponentInChildren<Animator> ().Play ("ToDown");
				}
				MainGameManager.coins -= cost;
				MainGameManager.coinReadout.GetComponentInChildren<TextMesh>().text = MainGameManager.coins.ToString();
				SaveManager.SetCoins(MainGameManager.coins);
				yield return new WaitForSeconds(0.5f);
				MainGameManager.current_game_state = MainGameManager.game_state.Playing;
				MainGameManager.Revive();
				yield break;
			}
			yield return null;
		} while(true);
			
	}
	}

