using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float distanceZ = -70f;
    public float distanceY = 20f;
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + distanceY, player.transform.position.z + distanceZ);
    }
}
