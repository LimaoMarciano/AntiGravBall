using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {

	//Object setup variables
	public float accPower = 200;
	public float turnSpeed = 50;
	public float shotForce = 200;
	public float brakeDrag = 2;
	public GameObject playerSprite;
	public GameObject forceField;
	public GameObject ballCollider;
	public GameObject kickTriggerObj;
	public GameObject repulsionParticle;
	public GameObject ThrusterParticleObj;

	//Input variables
	private string iHorizontal;
	private string iVertical;
	private string iGrab;
	private string iShoot;
	private string iBrake;
	private float hInput;
	private float vInput;
	public enum PlayerNumber {
		Player1,
		Player2,
		Player3,
		Player4
	};
	public PlayerNumber playerNumber;

	private Vector2 movement;
	private bool isPressingShoot = false;
	private bool isMovingDirectional = false;
	private bool isPressingKick = false;
	private bool isPressingBrake = false;
	private bool isPressingHold = false;

	//Internal variables
	public enum TeamSide {
		Red,
		Blue
	};
	
	public TeamSide teamSide;
	private float initDrag;
	private Rigidbody2D rb;
	private ForceField ff;
	private GameObject ball;
	private Collider2D bCollider;
	private KickTrigger kickTrigger;
	private ParticleSystem repulsionEmitter;
	private ParticleSystem thrusterParticles;

	private bool hasPossession = false;


	// Use this for initialization
	void Start () {

		SetInputStrings();

		rb = GetComponent<Rigidbody2D>();
		ff = forceField.GetComponent<ForceField>();
		bCollider = ballCollider.GetComponent<Collider2D>();
		ball = GameManager.instance.ball;
		kickTrigger = kickTriggerObj.GetComponent<KickTrigger>();
		repulsionEmitter = repulsionParticle.GetComponent<ParticleSystem>();
		thrusterParticles = ThrusterParticleObj.GetComponent<ParticleSystem>();
		initDrag = rb.drag;

		if (teamSide == TeamSide.Blue)
			playerSprite.GetComponent<SpriteRenderer>().color = Color.blue;
		else
			playerSprite.GetComponent<SpriteRenderer>().color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {

		//Get input
		hInput = Input.GetAxis(iHorizontal);
		vInput = Input.GetAxis(iVertical);

		if (GameManager.instance.isInputAllowed) {

			//Add force based on inpur
			movement = new Vector2(hInput, vInput) * accPower;

			if (hInput != 0 || vInput != 0) {
				isMovingDirectional = true;
				thrusterParticles.enableEmission = true;
				if (hasPossession && !GameManager.instance.isThrustAllowed)
					thrusterParticles.enableEmission = false;
			}
			else {
				thrusterParticles.enableEmission = false;
			}

			if (Input.GetButtonDown(iShoot)) {
				isPressingShoot = true;
			}

			if (Input.GetButton(iGrab)) {
				isPressingHold = true;
			}

			if (Input.GetButton(iBrake)) {
				rb.drag = brakeDrag;
			}
			else {
				rb.drag = initDrag;
			}
		}
	}

	void FixedUpdate () {

		thrusterParticles.enableEmission = false;

		if (isMovingDirectional) {

			//Rotate sprite
			Vector3 vectorToTarget = new Vector3(movement.x, movement.y, 0) - transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);

			if (hasPossession) {
				if (GameManager.instance.isThrustAllowed) {
					rb.AddForce(movement, ForceMode2D.Force);
				}
			}
			else {
				rb.AddForce(movement, ForceMode2D.Force);
			}
			isMovingDirectional = false;
		}

		if (isPressingShoot) {
			repulsionEmitter.Play();
			if (hasPossession) {
				ReleaseBall();
			}
			else {
				foreach (GameObject go in kickTrigger.triggerList) {
					Vector3 kickDirection = go.transform.position - transform.position;
					Vector2 kickForce = new Vector2(kickDirection.x, kickDirection.y);
					kickForce = kickForce.normalized * shotForce;
					go.GetComponent<Rigidbody2D>().AddForce(kickForce, ForceMode2D.Impulse);
					if (go.tag == "Player") {
						go.BroadcastMessage("ForceFieldKick");
					}
				}
			}
			isPressingShoot = false;
		}

		if (isPressingHold) {	
			if (ff.isBallInForceField) {
				HoldBall();
				foreach (GameObject go in GameManager.instance.players) {
					go.BroadcastMessage("GotBallPossession");
				}
			}
			isPressingHold = false;
		}
	}

	void SetInputStrings () {

		string pNumber = "";

		if (playerNumber == PlayerNumber.Player1) pNumber = "P1";
		if (playerNumber == PlayerNumber.Player2) pNumber = "P2";
		if (playerNumber == PlayerNumber.Player3) pNumber = "P3";
		if (playerNumber == PlayerNumber.Player4) pNumber = "P4";

		iHorizontal = pNumber + "Horizontal";
		iVertical = pNumber + "Vertical";
		iGrab = pNumber + "Grab";
		iShoot = pNumber + "Shoot";
		iBrake = pNumber + "Brake";
	}

	public void HoldBall() {
		ball.GetComponent<Rigidbody2D>().isKinematic = true;
		ball.transform.parent = gameObject.transform;
		ball.transform.position = forceField.transform.position;
		ball.GetComponent<Collider2D>().enabled = false;
		bCollider.enabled = true;
		hasPossession = true;
		
		GameManager.instance.isBallHold = true;
	}

	public void ReleaseBall () {
		ball.GetComponent<Rigidbody2D>().isKinematic = false;
		ball.transform.parent = null;
		ball.GetComponent<Collider2D>().enabled = true;
		bCollider.enabled = false;
		hasPossession = false;
		
		Vector2 shotDirection = playerSprite.transform.right;
		ball.GetComponent<Rigidbody2D>().velocity = rb.velocity;
		ball.GetComponent<Rigidbody2D>().AddForce(shotDirection * shotForce, ForceMode2D.Impulse);
		
		GameManager.instance.isBallHold = false;
	}

	public void ForceFieldKick () {
		if (hasPossession) {
			ball.GetComponent<Rigidbody2D>().isKinematic = false;
			ball.transform.parent = null;
			ball.GetComponent<Collider2D>().enabled = true;
			bCollider.enabled = false;
			hasPossession = false;
			
			Vector2 shotDirection = playerSprite.transform.right;
			ball.GetComponent<Rigidbody2D>().velocity = rb.velocity;
			ball.GetComponent<Rigidbody2D>().AddForce(shotDirection * shotForce * 0.25f, ForceMode2D.Impulse);

			GameManager.instance.isBallHold = false;
		}
	}
}
