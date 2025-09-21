using UnityEngine;
using Terresquall;
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 4f;
    private Vector3 moveVector = Vector3.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");

        moveVector = new Vector3(moveX, 0f, moveZ);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveVector * moveSpeed;
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0 );
    }
}
