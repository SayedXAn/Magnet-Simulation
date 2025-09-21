using UnityEngine;

public class Detector : MonoBehaviour
{
    public string pole = "";

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("south"))
        {
            pole = "south";
        }

        if (other.gameObject.CompareTag("north"))
        {
            pole = "north";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("south"))
        {
            pole = "";
        }

        if (other.gameObject.CompareTag("north"))
        {
            pole = "";
        }
    }
}
