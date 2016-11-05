using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MainGameManager : MonoBehaviour {
	public static Quaternion identity = new Quaternion (0f, 0f, 0f, 1f);
	public static Vector3 root;//serves as the root position for the first line, and for basic object positioning
	public static Vector2 cameraSize;//gets the size of the camera FoV in unity units
	public static int Number_Of_Line_Segments;//represents the number of new line segments requested by the AddLineSegment function
	public static int Max_Line_Segments;//represents the maximum number of segments that will fit on the screen
	public static float line_delay = 0.1f;
	public static Vector3 nextLinePos;
	public static Vector3 nextObstaclePos;
	public static float minimum_spacing_between_obstacles = 7f;
	public static float current_potential_space;
	public static float last_obstacle_right;
	public static float last_player_position;
	public static int obstacle_every = 3;
	public static int line_number = 0;
	public static float starting_position_scaler = 0.4f;
	public static float offset;
	public static float starting_reference;
	public enum game_state{PreStart,Playing,Paused,GameOver};
	public static game_state current_game_state = game_state.PreStart;
	public static float speed = 6;
	public static float max_speed = 15;
	public static ArrayList All_Lines;
	public static ArrayList All_Obstacles;
	public static int Number_Of_Obstacles = 0;
	//load necessary gameobjects
	public static GameObject line_segment;
	public static float line_segment_width;//padding will be added on
	public static float line_padding = 0.05f;
	public static GameObject line;
	private int number_of_obstacle_types = 7;
	public static GameObject[] Obstacle_Prefab_Array;
	public static GameObject player;
	public static string currentCharacter;
	public static GameObject scoreReadout;
	public static GameObject coinReadout;
	public static GameObject highScoreReadout;
	public static GameObject clickWall;
	public static Dictionary<string,Color> colors = new Dictionary<string, Color>();
	private static ArrayList colors_array;
	public static GameObject destroyed_particle; 
	public static bool canReset = false;
	bool coin_adding = false;
	int temp_coin_add_reference = 0;

	public SaveManager save_manager;

	AudioClip audiotemp = new AudioClip ();
	public static int score;
	public static int coins;
	public static int HighScore;
    public static int distance;

	public static bool isFirstGame = false;
	public static bool GPGS_Logged_In = false;

	// Use this for initialization
	void Start () {
		GPGS_Logged_In = Social.localUser.authenticated;
		Debug.Log (GPGS_Logged_In);
		HighScore = SaveManager.GetHighScore();
        distance = SaveManager.GetDistance();
		coins = SaveManager.GetCoins ();
		canReset = false;
		isFirstGame = SaveManager.IsFirstTime ();
		currentCharacter = SaveManager.GetCharacter ();
		//reset 
		line_number = 0;
		speed = 5;
		score = 0;

		cameraSize.y = 2 * Camera.main.orthographicSize;
		cameraSize.x = cameraSize.y * Camera.main.aspect;
		//sets the root vector to appropriate x and y values, scaled to the size of the screen.
		root = new Vector3 ((float)(0 - cameraSize.x * starting_position_scaler),(float) (0 - (cameraSize.y * 0.6 - cameraSize.y/2)));
		line_segment = Resources.Load("Sprites/Line/Prefabs/RoundedLine") as GameObject;
		line_segment_width = line_segment.GetComponent<SpriteRenderer> ().bounds.size.x + 2*line_padding;

		Number_Of_Line_Segments = 0;
		//instantiate obstacle array
		Obstacle_Prefab_Array = new GameObject[number_of_obstacle_types];
		//load in obstacles
		Obstacle_Prefab_Array[0] = Resources.Load("Sprites/Obstacles/Prefabs/Peaks_Purple") as GameObject;
		Obstacle_Prefab_Array[1] = Resources.Load("Sprites/Obstacles/Prefabs/Weight") as GameObject;
		Obstacle_Prefab_Array[2] = Resources.Load("Sprites/Obstacles/Prefabs/Pylon") as GameObject;
		Obstacle_Prefab_Array[3] = Resources.Load("Sprites/Obstacles/Prefabs/Bump") as GameObject;
		Obstacle_Prefab_Array[4] = Resources.Load("Sprites/Obstacles/Prefabs/Line") as GameObject;
		Obstacle_Prefab_Array[5] = Resources.Load("Sprites/Obstacles/Prefabs/Gear") as GameObject;
		destroyed_particle = Resources.Load("Sprites/Obstacles/Particles/Obstacle_Destroyed") as GameObject;

		//Instantiate array list
		All_Lines = new ArrayList();
		All_Obstacles = new ArrayList();
		All_Obstacles.Add (null);
		//using the root position, calculate how many lines can fit on the screen

		Max_Line_Segments = Mathf.RoundToInt((cameraSize.x - (cameraSize.x/2 + root.x))/ line_segment_width);

		//recalulate a root, so that there is equal whitespace on both sides
		float whiteSpaceLeft = cameraSize.x / 2 + root.x;
		float whiteSpaceRight = cameraSize.x -Max_Line_Segments * line_segment_width;
		root.x = (float) (0 - cameraSize.x/2 + (whiteSpaceRight + whiteSpaceLeft)/2);
		//set the line gen position to root
		nextLinePos = root;
		nextObstaclePos = root;
		nextObstaclePos.y += (float)(1.5 * (line_segment.GetComponent<SpriteRenderer> ().bounds.size.y));
		nextObstaclePos.x -= line_segment_width;
		nextLinePos.z = -1f;

		//position the car
		player = Resources.Load("Sprites/Player/Prefabs/" + currentCharacter) as GameObject;//Instantiate(Resources.Load("Sprites/Player/Prefabs/TarCar") as GameObject,new Vector3 ((float)(root.x + player.GetComponent<SpriteRenderer> ().bounds.size.x * 0.3), root.y + (player.GetComponent<SpriteRenderer> ().bounds.size.y/2)+ line_segment.GetComponent<SpriteRenderer>().bounds.size.y/2, 0f),identity) as GameObject;//GameObject.FindGameObjectWithTag ("Player");
		player = Instantiate(player,new Vector3(0f,0f,0f),identity) as GameObject;
		player.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0f, 0f, 0f);
		scoreReadout = GameObject.FindGameObjectWithTag ("Score");
		scoreReadout.GetComponent<Animator> ().Play ("MainGame_Score_In");
		highScoreReadout = GameObject.FindGameObjectWithTag ("HighScore");
		highScoreReadout.GetComponent<TextMesh> ().text = HighScore.ToString ();
		coinReadout = GameObject.FindGameObjectWithTag ("CoinBanner");
		coinReadout.GetComponentInChildren<TextMesh> ().text = coins.ToString ();
		clickWall = GameObject.FindGameObjectWithTag ("ClickWall");
		player.transform.position = new Vector3 ((float)(root.x + player.GetComponentInChildren<SpriteRenderer> ().bounds.size.x * 0.3), root.y + (player.GetComponentInChildren<SpriteRenderer> ().bounds.size.y/2)+ line_segment.GetComponentInChildren<SpriteRenderer>().bounds.size.y/2, 0f);
		starting_reference = Camera.main.transform.position.x - player.transform.position.x;
		AddLineSegments(false);
		current_game_state = game_state.PreStart;


		//Car's start up sound -Tyler
		AudioClip temp = AudioManager.Start_Sound;
		AudioManager.Play_Audio (temp);	

		//testing git upload
		offset = Camera.main.transform.position.x - player.transform.position.x;
	}


	// Update is called once per frame
	void LateUpdate(){
		//overide animation values for camera
		if (player.transform.position.x > 0 - cameraSize.x / 4 && current_game_state == game_state.Playing) {
			Camera.main.transform.position = new Vector3 (player.transform.position.x + cameraSize.x / 4, 0f, 0f);
		} else if (player.transform.position.x > 0 - cameraSize.x / 4) {
			Camera.main.transform.position = new Vector3 (player.transform.position.x + cameraSize.x / 4, Camera.main.transform.position.y, 0f);
		}
	}
	void Update () {
		//handle back button
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene("StartMenu");
		}
		//handle obstacle names

		for (int x = 0; x<All_Obstacles.Count; x++) {
			GameObject temp = All_Obstacles[x] as GameObject;
			if(temp!= null){
				temp.name = "O_" + x;
			}
		}
		for (int x = 0; x<All_Lines.Count; x++) {
			GameObject temp = All_Lines[x] as GameObject;
			if(temp!= null){
				temp.name = "LS_" + x;
			}
		}
		if(current_game_state ==game_state.Playing){
            //adjust speed of player
            if (speed <= 2 * max_speed)
            {
                speed += (float)( (speed <= max_speed) ? ((score / speed) / 1000) : (speed <= 1.5 * max_speed) ? 0.8 * ((score / speed) / 1000) : 0.6 * ((score / speed) / 1000));
                Vector3 tempVel = new Vector3(speed, 0f, 0f);
                player.GetComponent<Rigidbody2D>().velocity = tempVel;
                // adjust speed of "flip" animation
                //player.GetComponentInChildren<Animator>().speed = speed/6;//scales speed proporitanetly to movement speed
            }


            if (player.transform.position.x > 0 - cameraSize.x / 4) {
				if (offset == 0) {
					//offset = Camera.main.transform.position.x - player.transform.position.x;
				}
			}
				

				//update the measurement of distance between last and current potential obstacle
				current_potential_space += player.transform.position.x - last_player_position;
				last_player_position = player.transform.position.x;

				}
		

			//change score text
		if (current_game_state == game_state.Playing) {
			score = (int)((player.transform.position.x + starting_reference) / 3);
			//award coin for every 20 meters
			if ((float)score / 20 == Mathf.Round (score / 20) && score > 0 && coin_adding == false) {
				AddCoins (1);
				temp_coin_add_reference = score + 1;
				coin_adding = true;
			} else if (score == temp_coin_add_reference) {
				coin_adding = false;
				Destroy (GameObject.Find ("AE"));
			}
			if (score != 1) {
				scoreReadout.GetComponent<TextMesh> ().text = (score + " meters");
			} else {
				scoreReadout.GetComponent<TextMesh> ().text = (score + " meter");
			}
			if (score > HighScore) {
				//High score (animations and stuff according to jay)
				HighScore = score;
				highScoreReadout.GetComponent<TextMesh> ().text = HighScore.ToString ();
			}
		}
        //the big k
        distance += score; 
        if (distance == 1000) {
            GPS.AwardAchievement("TheBigK");
        }
		}

 void AddLineSegmentInternal(){
			GameObject temp = Instantiate (line_segment, nextLinePos, identity) as GameObject;
			temp.name = "LS_" + Number_Of_Line_Segments;
			temp.GetComponent<MainGameLineHandler> ().containsObstacle = false;
			//add line segment to array of line segments
			All_Lines.Add (temp);
			nextLinePos.x += line_segment_width;
			nextObstaclePos.x += line_segment_width;
			Number_Of_Line_Segments += 1;
			
			if (Number_Of_Line_Segments == Max_Line_Segments) {

			CancelInvoke();
		}
		
	}
	public void AddLineSegments(bool onlyOne){
		if (onlyOne == false) {
			InvokeRepeating ("AddLineSegmentInternal", 0.0f, line_delay);
		} else {
			Invoke("AddLineSegmentInternal",0.0f);
		}
		}
	void OnMouseDown(){
	//handle normal rotate clicks
		if (player.GetComponentInChildren<Animator> ().GetBool ("RotatedUp") == true) {
			player.GetComponentInChildren<Animator> ().SetBool ("RotatedUp", false);
			player.GetComponentInChildren<Animator> ().Play ("MainGame_TarCar_RotateToDown");
		} else {
			player.GetComponentInChildren<Animator> ().SetBool ("RotatedUp", true);
			player.GetComponentInChildren<Animator> ().Play ("MainGame_TarCar_RotateToUp");

		}
	}
	public static void Start_Game(){
		Camera.main.GetComponent<Animator> ().Play ("MainGame_Score_Raise");
		Vector3 tempVel = new Vector3 (speed, 0f, 0f);
		player.GetComponent<Rigidbody2D> ().velocity = tempVel;
		current_game_state = game_state.Playing;
	}
	public static bool Generate_Obstacle(GameObject requester){
			if (line_number == obstacle_every) {
			int random = Mathf.RoundToInt (Random.Range (-0.4f, 5.4f));


			
			GameObject temp = Instantiate (Obstacle_Prefab_Array [random], nextObstaclePos, identity) as GameObject;
			Number_Of_Obstacles += 1;
			temp.name = "O_" + (Number_Of_Line_Segments - 1);
			All_Obstacles.Add (temp);
			line_number = 1;
			return true;
		} else {
			line_number +=1;
			return false;
		}
	}

	public static void Game_Over(){
		if (score >= HighScore) {
			SaveManager.SetHighScore(HighScore);
			SaveManager.Save();
			GPS.ReportHighScore (score);
			//scoreReadout.GetComponent<TextMesh>().color = colors["Gold"];
		}
		//Destruction sound - Tyler
		AudioClip audiotemp = AudioManager.Destruction_Sound;
			AudioManager.Play_Audio (audiotemp);
		//Jays shit is below
		current_game_state = game_state.GameOver;
		player.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0f, 0f, 0f);
		line_number = 0;
		speed = 5;
		Camera.main.GetComponent<Animator> ().Play ("MainGame_Camera_ZoomOut");
		GameObject Colour_Particles = Instantiate (Resources.Load ("Sprites/Ui/Particles/ColourParticles") as GameObject, new Vector3 (Camera.main.transform.position.x, 4f, 11f), identity)as GameObject;

	}
	public static void Revive(){
		Vector3 tempVel = new Vector3 ((float)(speed * 0.6), 0f, 0f);
		current_game_state = game_state.Playing;
		player.GetComponent<Rigidbody2D> ().velocity = tempVel;
	}
	public static void Start_New(){
	
		player.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0f, 0f, 0f);
		SceneManager.LoadScene ("MainGame");
	}
	 void RunGameOver(){
			//award first game achievement if applicable!
		if (isFirstGame == true) {
			GPS.AwardAchievement ("FirstGame");
		}

		foreach (Object obj in All_Obstacles) {
			Destroy(obj);
		}
		canReset = true;
		StartCoroutine ("AnimateScore");
	}
	public static void AddCoins(int coinsToAdd){
		GameObject add_effect = Instantiate (Resources.Load ("Sprites/Ui/Particles/AddEffect") as GameObject, new Vector3 (Camera.main.transform.position.x, 2f, 0f), identity)as GameObject;
		//add_effect.name = "AE";
		coins += coinsToAdd;
		coinReadout.GetComponentInChildren<TextMesh> ().text = coins.ToString ();
		SaveManager.SetCoins (coins);
	}
	IEnumerator AnimateScore(){
		GameObject Medal_Gold = Resources.Load("Sprites/Ui/Prefabs/Medal_Gold") as GameObject;
		GameObject Medal_Silver = Resources.Load("Sprites/Ui/Prefabs/Medal_Silver") as GameObject;
		GameObject Medal_Bronze = Resources.Load("Sprites/Ui/Prefabs/Medal_Bronze") as GameObject;
        GameObject Medal_Platinum = Resources.Load("Sprites/Ui/Prefabs/Medal_Platinum") as GameObject;
		for (int score_tick = 1; score_tick <= score; score_tick++) {
			if (score_tick != 1) {
				scoreReadout.GetComponent<TextMesh> ().text = (score_tick + " meters");
			} else {
				scoreReadout.GetComponent<TextMesh> ().text = (score_tick + " meter");
			}
            //handle medals
            if (score_tick == 10)
            {
                //award bronze medal
                GameObject Bronze = Instantiate(Medal_Bronze, new Vector3(Camera.main.transform.position.x, 3.9f, 0f), identity) as GameObject;
                GPS.AwardAchievement("Bronze");
            }
            else if (score_tick == 25)
            {
                //award silver medal
                GameObject Silver = Instantiate(Medal_Silver, new Vector3(Camera.main.transform.position.x, 3.9f, 0f), identity) as GameObject;
                GPS.AwardAchievement("Silver");
            }
            else if (score_tick == 50)
            {
                //award gold medal
                GameObject Gold = Instantiate(Medal_Gold, new Vector3(Camera.main.transform.position.x, 3.9f, 0f), identity) as GameObject;
                GPS.AwardAchievement("Gold");
            }
            else if (score_tick == 100)
            {
                //award Platinum medal
                GameObject Platinum = Instantiate(Medal_Platinum, new Vector3(Camera.main.transform.position.x, 3.9f, 0f), identity) as GameObject;
                GPS.AwardAchievement("Platinum");
            }
                yield return new WaitForSeconds(0.1f);
		}
	}

}
