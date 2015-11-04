using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static GameManager _instance;

	public GameObject player;
	public GameObject ball;
	public int redScore = 0;
	public int blueScore = 0;

	private Text redScoreText;
	private Text blueScoreText;
	public GameObject redScoreObj;
	public GameObject blueScoreObj;

	private Text holdTimerText;
	public GameObject holdTimerObj;

	public List<GameObject> redStartPositions = new List<GameObject>();
	public List<GameObject> blueStartPositions = new List<GameObject>();
	public List<GameObject> players = new List<GameObject>();

	public bool isInputAllowed = false;
	private float freezeTimer = 3;

	public float holdBallTime = 3;
	private float thrustTimer;
	public bool isThrustAllowed = true;
	public bool isBallHold = false;

	
	static public bool isActive { 
		get { 
			return _instance != null; 
		} 
	}
	
	static public GameManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
				
				if (_instance == null)
				{
					GameObject go = new GameObject("_gamemanager");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<GameManager>();
				}
			}
			return _instance;
		}
	}

	void Start() {
		redScoreText = redScoreObj.GetComponent<Text>();
		blueScoreText = blueScoreObj.GetComponent<Text>();
		holdTimerText = holdTimerObj.GetComponent<Text>();
		thrustTimer = holdBallTime;
		CreatePlayers();
	}

	void Update() {
		//Controls
		if (Input.GetKeyDown(KeyCode.R)) 
		    Application.LoadLevel("Main2D");

		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit();

		//UI
		redScoreText.text = "" + redScore;
		blueScoreText.text = "" + blueScore;
		holdTimerText.text = "" + Mathf.CeilToInt(thrustTimer);

		if (thrustTimer <= 0)
			holdTimerText.color = Color.red;
		else
			holdTimerText.color = Color.white;

		//Start freeze
		if (!isInputAllowed) {
			freezeTimer -= Time.deltaTime;
			if (freezeTimer <= 0) {
				freezeTimer = 3;
				isInputAllowed = true;
			}
		}

		//Thrust timer
		if (isBallHold) {
			if (thrustTimer > 0)
				thrustTimer -= Time.deltaTime;
			//Debug.Log("Timer: " + thrustTimer);
		}
		else {
			thrustTimer = holdBallTime;
		}

		if (thrustTimer <= 0) {
			thrustTimer = 0;
			isThrustAllowed = false;
			//Debug.Log("Thrust time over!");
		}
		else
		{
			isThrustAllowed = true;
		}

	}

	void CreatePlayers() {
		GameObject playerObj01 = Instantiate(player, redStartPositions[0].transform.position, redStartPositions[0].transform.rotation) as GameObject;
		GameObject playerObj02 = Instantiate(player, blueStartPositions[0].transform.position, blueStartPositions[0].transform.rotation) as GameObject;
		GameObject playerObj03 = Instantiate(player, redStartPositions[1].transform.position, redStartPositions[1].transform.rotation) as GameObject;
		GameObject playerObj04 = Instantiate(player, blueStartPositions[1].transform.position, blueStartPositions[1].transform.rotation) as GameObject;

		players.Add(playerObj01);
		players.Add(playerObj02);
		players.Add(playerObj03);
		players.Add(playerObj04);

		playerObj01.GetComponent<ShipBehavior>().teamSide = ShipBehavior.TeamSide.Red;
		playerObj02.GetComponent<ShipBehavior>().teamSide = ShipBehavior.TeamSide.Blue;
		playerObj03.GetComponent<ShipBehavior>().teamSide = ShipBehavior.TeamSide.Red;
		playerObj04.GetComponent<ShipBehavior>().teamSide = ShipBehavior.TeamSide.Blue;

		playerObj01.GetComponent<ShipBehavior>().playerNumber = ShipBehavior.PlayerNumber.Player1;
		playerObj02.GetComponent<ShipBehavior>().playerNumber = ShipBehavior.PlayerNumber.Player2;
		playerObj03.GetComponent<ShipBehavior>().playerNumber = ShipBehavior.PlayerNumber.Player3;
		playerObj04.GetComponent<ShipBehavior>().playerNumber = ShipBehavior.PlayerNumber.Player4;

	}

	private void PositionPlayer() {
		players[0].transform.position = redStartPositions[0].transform.position;
		players[1].transform.position = blueStartPositions[0].transform.position;
		players[2].transform.position = redStartPositions[1].transform.position;
		players[3].transform.position = blueStartPositions[1].transform.position;

		players[0].transform.rotation = redStartPositions[0].transform.rotation;
		players[1].transform.rotation = blueStartPositions[0].transform.rotation;
		players[2].transform.rotation = redStartPositions[1].transform.rotation;
		players[3].transform.rotation = blueStartPositions[1].transform.rotation;
	}

	public void ScoreRed () {
		redScore += 1;
		PositionPlayer();
		ball.transform.position = Vector3.zero;
		ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		foreach (GameObject player in players) {
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		isInputAllowed = false;
	}

	public void ScoreBlue () {
		blueScore += 1;
		PositionPlayer();
		ball.transform.position = Vector3.zero;
		ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		foreach (GameObject player in players) {
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		isInputAllowed = false;
	}

}