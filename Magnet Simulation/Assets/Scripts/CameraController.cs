using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float distanceZ = -70f;
    public float distanceY = 20f;
    public float followSpeed = 0.2f;


    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(
            player.transform.position.x,
            player.transform.position.y + distanceY,
            player.transform.position.z + distanceZ
        );

        transform.position = targetPos;
    }
}