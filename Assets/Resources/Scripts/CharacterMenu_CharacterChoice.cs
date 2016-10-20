using UnityEngine;
using System.Collections;

public class CharacterMenu_CharacterChoice : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void RemoveChoice(){
		CharacterMenuManager.currentCharacterGO = CharacterMenuManager.tempChar;
		Destroy (gameObject);
	}
}
