using UnityEngine;

public class Detector : MonoBehaviour
{

    public string pole = "";
    public Vector3 targetVector = Vector3.zero;
    public float forceAmount = 10f;
    private Rigidbody rb;
    public Rigidbody parentRB;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<FixedJoint>().connectedBody = parentRB;
    }

    private void OnTriggerStay(Collider other)
    {
        

        if(other.gameObject.CompareTag("south"))
        {
            targetVector = other.transform.position;
            pole = "south";            
        }
        if (other.gameObject.CompareTag("north"))
        {
            targetVector = other.transform.position;
            pole = "north";
            Vector3 forceDirection = transform.position - other.transform.position;
            forceDirection = forceDirection.normalized;
            //Debug.Log("Dir: " + forceDirection);
            //Vector3 forceVector = forceDirection / (Vector3.Distance(transform.position, other.transform.position) * Vector3.Distance(transform.position, other.transform.position)) * forceAmount;
            Vector3 forceVector = forceDirection * forceAmount * other.GetComponent<Magnets>().forceAmount;
            Debug.Log("forV: " + forceVector);
            rb.AddForce(forceVector);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag("south"))
        {
            targetVector = Vector3.zero;
            pole = "";
        }

        if (other.gameObject.CompareTag("north"))
        {
            targetVector = Vector3.zero;
            pole = "";
        }
    }
}
