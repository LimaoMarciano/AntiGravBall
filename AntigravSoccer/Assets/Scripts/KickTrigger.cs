using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KickTrigger : MonoBehaviour {

	public List<GameObject> triggerList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (!triggerList.Contains(col.gameObject)) {
			triggerList.Add(col.gameObject);
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (triggerList.Contains(col.gameObject)) {
			triggerList.Remove(col.gameObject);
		}
	}

	public void GotBallPossession () {

		for (int i = 0; i < triggerList.Count; i++) {
			if (triggerList[i].tag == "Ball") triggerList.Remove(triggerList[i]);
		}

	}
}
