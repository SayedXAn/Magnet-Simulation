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
        GetComponent<Renderer>().material = mats[0];
    }

    private void ActicvateSouthPole()
    {
        // North pole will be represented by "Red" color and the South pole by "Blue" color
    }
}

