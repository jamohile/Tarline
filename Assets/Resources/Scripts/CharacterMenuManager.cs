using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMenuManager : MonoBehaviour {
	public static List<string> All_Characters = new List<string> ();
	public static string currentCharacter;
	public static string chosenCharacter;
	public enum character_position {
		previous,
		next,
		current
	}
	public static GameObject currentCharacterGO;
	public static GameObject tempChar;
	// Use this for initialization
	void Start () {
		All_Characters.Add ("TarCar");
		All_Characters.Add ("Skateboard");
		currentCharacter = SaveManager.GetCharacter ();
		chosenCharacter = currentCharacter;
		Debug.Log (currentCharacter);
		LoadCharacter (All_Characters.IndexOf (currentCharacter), character_position.current, true);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void LoadCharacter(int index, character_position position,bool first_in = false){

//		if (All_Characters [index] == chosenCharacter) {
//			GameObject.FindGameObjectWithTag("ChooseCharacterButton").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Ui/Characters_Chosen");
//		} else {
//			GameObject.FindGameObjectWithTag("ChooseCharacterButton").GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Ui/Characters_Choose_Up");
//		}
		if (!first_in && position == character_position.next) {
			tempChar.GetComponent<Animator> ().Play ("CharacterMenu_CharacterChoice_RemoveToLeft");
		} else if(!first_in && position == character_position.previous){
			tempChar.GetComponent<Animator> ().Play ("CharacterMenu_CharacterChoice_RemoveToRight");
		}
		tempChar = Resources.Load ("Sprites/Player/Prefabs/CharacterChoice") as GameObject;
		tempChar.GetComponentInChildren<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Player/" + All_Characters [index] + "/" + All_Characters [index]);
		//tempChar.GetComponentInChildren<TextMesh> ().text = All_Characters [index];
		tempChar = Instantiate (tempChar, new Vector3 (20f, 0f, 0f), MainGameManager.identity) as GameObject;
		if (position == character_position.next || position == character_position.current) {
			tempChar.GetComponent<Animator> ().Play ("CharacterMenu_CharacterChoice_EnterFromRight");
		} else if(position == character_position.previous){
			tempChar.GetComponent<Animator> ().Play ("CharacterMenu_CharacterChoice_EnterFromLeft");
		}
		currentCharacter = All_Characters [index];
		//sewt character as current character
		SaveManager.SetCharacter (currentCharacter);
		chosenCharacter = currentCharacter;
	}
	public static void RightScroll(){
		if (All_Characters.IndexOf (currentCharacter) == All_Characters.Count - 1) {
			CharacterMenuManager.LoadCharacter (0, character_position.next);
		} else {
			CharacterMenuManager.LoadCharacter (All_Characters.IndexOf(currentCharacter) + 1, character_position.next);
		}
	}
	public static void LeftScroll(){
		if (All_Characters.IndexOf (currentCharacter) == 0) {
			CharacterMenuManager.LoadCharacter (All_Characters.Count - 1, character_position.previous);
		} else {
			CharacterMenuManager.LoadCharacter (All_Characters.IndexOf(currentCharacter) - 1, character_position.previous);
		}
	}
}
