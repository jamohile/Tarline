using UnityEngine;
using System.Collections;

public class SettingsHandler : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
		//calculate movement of finger, move camera accordingly
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y + Input.GetTouch(0).deltaPosition.y,Camera.main.transform.position.y);
		}
	}
}
