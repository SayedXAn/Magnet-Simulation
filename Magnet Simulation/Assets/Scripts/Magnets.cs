using UnityEngine;

public class Magnets : MonoBehaviour
{
    private Rigidbody rb;
    private string pole = "south";
    public float forceAmount = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.CompareTag("south"))
        {
            //pole = "south";
        }
        if (other.gameObject.CompareTag("north"))
        {
            Vector3 forceDirection = other.transform.position - transform.position;
            forceDirection = forceDirection.normalized;
            //Vector3 forceVector = forceDirection / (Vector3.Distance(transform.position, other.transform.position) * Vector3.Distance(transform.position, other.transform.position)) * forceAmount * other.GetComponent<Magnets>().forceAmount;
            Vector3 forceVector = forceDirection * forceAmount * other.GetComponent<Magnets>().forceAmount;
            Debug.Log("forV: " + forceVector);
            rb.AddForce(forceVector);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        
    }
}
