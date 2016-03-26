using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickHandler : MonoBehaviour {
	public static string Scene_To_Load = "";
	// Handle Click events

	void OnMouseDown(){
		switch (gameObject.tag) {
		case "PlayButton":
			Scene_To_Load = "MainGame";
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Ui/Play_Down");
			break;
		case "CharactersButton":
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Ui/Characters_Down");
			break;
		case "RetryButton":
			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Ui/Retry_Down");
			break;
		case "HomeButton":
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Sprites/Ui/Home_Down");
			break;
		case "SettingsButton":
			Scene_To_Load = "StartMenu";
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Sprites/Ui/Settings_Down");
			break;
		}

	}
	void OnMouseUp(){
		if (SceneManager.GetActiveScene ().name == "StartMenu") {
			Animator transition = Camera.main.GetComponent<Animator> ();
			transition.Play ("StartMenu_Camera_RotateAway");
		}

		switch (gameObject.tag) {
		case "RetryButton":
			MainGameManager.Start_New ();
			break;
		case "HomeButton":
			Scene_To_Load = "StartMenu";
			Change_Scene ();
			break;
		
		case "CharactersButton":
			Scene_To_Load = "Characters";
			Change_Scene ();
			break;
		}
	}


	public void Change_Scene(){
		SceneManager.LoadScene (Scene_To_Load);
	}

}





