using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public Rigidbody rb;
    public float AccForce = 300;
    public float stopDrag = 5;
    public GameObject shipMesh;
    public GameObject forceFieldObj;
    private ForceField forceField;
    private GameObject ball;
    private Collider ballSpaceCol;

	public bool hasPossession = false;


    private Transform target;
    public float turnSpeed = 50;

    private float vInput;
    private float hInput;
    private Vector3 inputDir;
    private float initDrag;

	// Use this for initialization
	void Start () {
        initDrag = rb.drag;
        forceField = forceFieldObj.GetComponent<ForceField>();
        ball = GameObject.FindGameObjectWithTag("Ball");
     
	}
	
	// Update is called once per frame
	void Update () {

        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");

        inputDir = new Vector3(hInput, 0, vInput);

        
        if (hInput != 0 || vInput != 0)
        {
            Vector3 targetDir = inputDir;
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(shipMesh.transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(shipMesh.transform.position, newDir, Color.red);
            shipMesh.transform.rotation = Quaternion.LookRotation(newDir);

            rb.AddForce(inputDir.normalized * AccForce);
        }

        if (Input.GetButton("Fire1"))
        {
            if (forceField.isBallInForceField)
            {
                ball.GetComponent<Rigidbody>().isKinematic = true;
                //ball.GetComponent<Rigidbody>().detectCollisions = false;
                ball.transform.position = forceFieldObj.transform.position;
				hasPossession = true;
            }
        }
        else
        {
            ball.GetComponent<Rigidbody>().isKinematic = false;
			hasPossession = false;
            //ball.GetComponent<Rigidbody>().detectCollisions = true;
        }

        if (Input.GetButton("Fire2"))
        {
            rb.drag = stopDrag;
        }
        else
            rb.drag = initDrag;

	}

}
