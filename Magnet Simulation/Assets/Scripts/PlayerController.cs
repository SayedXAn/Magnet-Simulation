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
    public float forceAmount = 10f;

    public float jumpForce = 7f;
    private bool isJumping = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //rb.constraints = RigidbodyConstraints.FreezePositionY;
        //rb.constraints |= RigidbodyConstraints.FreezePositionY;

        GetComponent<FixedJoint>().connectedBody = detector.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");

        moveVector = new Vector3(moveX, 0f, moveZ);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveVector.x * moveSpeed, rb.linearVelocity.y, moveVector.z * moveSpeed);

        if (!isJumping)
        {
            rb.position = new Vector3(rb.position.x, 10f, rb.position.z);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 10f, rb.linearVelocity.z);
        }

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

    public void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJumping = false;
        }
    }
}

