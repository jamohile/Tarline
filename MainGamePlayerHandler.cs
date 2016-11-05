using UnityEngine;
using System.Collections;


public class MainGamePlayerHandler : MonoBehaviour {
	enum contact_position{
		Back_Of_Back,
		Front_Of_Back,
		Back_Of_Front,
		Front_Of_Front
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D coll){
		ContactPoint2D contact = coll.contacts[0];
		contact.collider.GetComponentInParent<MainGameObstacleHandler> ().forceDestroy = true;
		Instantiate (MainGameManager.destroyed_particle, contact.collider.gameObject.transform.position, new Quaternion (0f, 0f, 0f, 1f));
		//MainGameManager.Game_Over ();
		if (MainGameManager.coins >= Mathf.RoundToInt(Mathf.Sqrt(MainGameManager.score) * (Mathf.Sqrt(MainGameManager.score))/5) && MainGameManager.score >= 25) {
			GameObject revive_notification = Resources.Load ("Sprites/Ui/Prefabs/Notification_Revive") as GameObject;
			revive_notification = Instantiate (revive_notification) as GameObject;
			revive_notification.GetComponentInChildren<TextMesh> ().text = Mathf.RoundToInt(Mathf.Sqrt(MainGameManager.score) * (Mathf.Sqrt(MainGameManager.score))/5).ToString ();
			revive_notification.GetComponent<NotificationManager> ().revivePlayer (Mathf.RoundToInt(Mathf.Sqrt(MainGameManager.score) * (Mathf.Sqrt(MainGameManager.score))/5));//start the prompt process
		}else{
			MainGameManager.Game_Over();
		}
		


	}


		
		
}
