using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickHandler : MonoBehaviour {
	public static string Scene_To_Load = "";
	// Handle Click events

	void OnMouseDown(){
		switch (gameObject.name) {
		case "Play":
			Scene_To_Load = "MainGame";
			break;
		case "Home_Up":
			Scene_To_Load = "StartMenu";
			Change_Scene ();
			break;

		case "Characters":
			Scene_To_Load = "Characters";
			Change_Scene ();
			break;
		}
		Animator transition = Camera.main.GetComponent<Animator> ();
		transition.Play ("StartMenu_Camera_RotateAway");

	}
	void OnMouseUp(){
		switch (gameObject.name) {
		case "Retry_Up":
			MainGameManager.Start_New ();
			break;
		case "Home_Up":
			Scene_To_Load = "StartMenu";
			Change_Scene ();
			break;
		
		case "Characters_Up":
			Scene_To_Load = "Characters";
			Change_Scene ();
			break;
		}
	}


	public void Change_Scene(){
		SceneManager.LoadScene (Scene_To_Load);
	}

}





