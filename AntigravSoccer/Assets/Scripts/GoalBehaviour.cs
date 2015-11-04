using UnityEngine;
using System.Collections;

public class GoalBehaviour : MonoBehaviour {
	
	public enum TeamSide {
		Red,
		Blue
	};
	
	public TeamSide teamSide;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.gameObject.tag == "Ball") {
			if (teamSide == TeamSide.Blue)
				GameManager.instance.ScoreRed();
			if (teamSide == TeamSide.Red)
				GameManager.instance.ScoreBlue();
		}
	}
}
