using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

    public bool isBallInForceField;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
			isBallInForceField = true;
    }

    void OnTriggerExit2D (Collider2D col)
    {
		if (col.gameObject.tag == "Ball")
			isBallInForceField = false;
    }

	public void GotBallPossession () {
		isBallInForceField = false;
	}
}
