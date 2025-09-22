using UnityEngine;

public class Detector : MonoBehaviour
{

    public string pole = "north";
    public Vector3 targetVector = Vector3.zero;
    public float forceAmount = 10f;
    private Rigidbody rb;
    public Rigidbody parentRB;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<FixedJoint>().connectedBody = parentRB;
        InvokeRepeating("ChangePolarityAndMaterial", 5f, 5f);
    }

    private void ChangePolarityAndMaterial()
    {
        //North pole will be represented by "Red" color and the South pole by "Blue" color
        if (pole == "south")
        {
            pole = "north";
            parentRB.gameObject.GetComponent<Renderer>().material = parentRB.gameObject.GetComponent<PlayerController>().mats[1];
        }
        else if (pole == "north")
        {
            pole = "south";
            parentRB.gameObject.GetComponent<Renderer>().material = parentRB.gameObject.GetComponent<PlayerController>().mats[0];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.CompareTag("south") || other.gameObject.CompareTag("north"))
        {
            Vector3 forceDirection = transform.position - other.transform.position;
            float distance = forceDirection.magnitude;
            forceDirection.Normalize();
            Debug.Log(distance);

            float scaledForce = forceAmount / Mathf.Max(distance * distance, 0.01f);

            if((pole == "north" && other.gameObject.CompareTag("south")) || (pole == "south" && other.gameObject.CompareTag("north")))
            {
                //Impulse
                rb.AddForce(-forceDirection * scaledForce, ForceMode.Force);
            }
            else
            {
                rb.AddForce(forceDirection * scaledForce, ForceMode.Force);
            }
        }        

    }

}
