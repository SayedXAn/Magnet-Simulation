using UnityEngine;
using Terresquall;
public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public float moveSpeed = 4f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    private Vector3 moveVector = Vector3.zero;
    public Material[] mats;
    public Detector detector;
    public bool isGrounded = false;


    private Vector2 startTouchPos;
    private bool isSwipe;
    public float minSwipeDist = 100f;
    public float jumpForce = 7f;
    private bool isJumping = false;
    public float jumpGravity = 3f;

    private float lastTapTime = 0f;
    public float doubleTapTime = 0.3f;
    private bool isSprinting = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        GetComponent<FixedJoint>().connectedBody = detector.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");

        moveVector = new Vector3(moveX, 0f, moveZ);


        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2f)
            {
                if (Time.time - lastTapTime <= doubleTapTime)
                {
                    StartCoroutine(Sprint());
                }
                lastTapTime = Time.time;

                startTouchPos = touch.position;
                isSwipe = true;
            }

            if (touch.phase == TouchPhase.Ended && isSwipe)
            {
                Vector2 endTouchPos = touch.position;
                Vector2 swipeVector = endTouchPos - startTouchPos;

                if (swipeVector.magnitude > minSwipeDist && swipeVector.y > Mathf.Abs(swipeVector.x))
                {
                    Jump();
                }

                isSwipe = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Sprint());
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveVector.x * moveSpeed, rb.linearVelocity.y, moveVector.z * moveSpeed);

        if (!isJumping || isGrounded)
        {
            //rb.position = new Vector3(rb.position.x, 10f, rb.position.z);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -10f, rb.linearVelocity.z);
        }

        if (isJumping || !isGrounded)
        {
            rb.AddForce(Vector3.down * jumpGravity, ForceMode.Acceleration);
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
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    private System.Collections.IEnumerator Sprint()
    {
        if (!isSprinting) // prevent stacking
        {
            isSprinting = true;
            float originalSpeed = moveSpeed;
            moveSpeed = sprintSpeed;

            yield return new WaitForSeconds(1f);

            moveSpeed = originalSpeed;
            isSprinting = false;
        }
    }
}

