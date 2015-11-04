using UnityEngine;
using System.Collections;

public class ballCollider : MonoBehaviour {

    public GameObject playerObj;
    private PlayerBehaviour player;
    private Collider col;

	// Use this for initialization
	void Start () {
        player = playerObj.GetComponent<PlayerBehaviour>();
        col = gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.hasPossession)
        {
            col.enabled = true;
        }
        else
            col.enabled = false;
	}
}
