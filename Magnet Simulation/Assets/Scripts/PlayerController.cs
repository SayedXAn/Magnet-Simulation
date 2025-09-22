using UnityEngine;
using Terresquall;
using System.Drawing;
public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;
    private Vector3 moveVector = Vector3.zero;
    public Material[] mats;
    public Detector detector;
    public string playerPole = "south";
    public float forceAmount = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        GetComponent<FixedJoint>().connectedBody = detector.gameObject.GetComponent<Rigidbody>();
        InvokeRepeating("ChangePolarityAndMaterial", 5f, 5f);
        
    }

    void Update()
    {
        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");

        moveVector = new Vector3(moveX, 0f, moveZ);
        //if (detector.pole != "")
        //{
        //    if (detector.pole == playerPole)
        //    {
        //        ActicvateRepulsiveBehaviour();
        //    }
        //    else
        //    {
        //        ActicvateImpulsiveBehaviour();
        //    }

        //}


    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveVector * moveSpeed;

        if (moveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed );
        }

        
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0 );
    }

    private void ChangePolarityAndMaterial()
    {
        // North pole will be represented by "Red" color and the South pole by "Blue" color
        //if (playerPole == "south")
        //{
        //    playerPole = "north";
        //    GetComponent<Renderer>().material = mats[1];
        //}
        //else if(playerPole == "north")
        //{
        //    playerPole = "south";
        //    GetComponent<Renderer>().material = mats[0];
        //}
    }

    private void ActicvateImpulsiveBehaviour()
    {
        
        //Vector3 forceDirection = transform.position - detector.targetVector;
        //forceDirection = forceDirection.normalized;
        ////Debug.Log("Dir: " + forceDirection);
        //Vector3 forceVector = forceDirection /  (Vector3.Distance(transform.position, detector.targetVector) * Vector3.Distance(transform.position, detector.targetVector)) * forceAmount;
        //Debug.Log("forV: " +forceVector);
        //rb.AddForce(forceVector);


    }

    private void ActicvateRepulsiveBehaviour()
    {
        

    }
}

