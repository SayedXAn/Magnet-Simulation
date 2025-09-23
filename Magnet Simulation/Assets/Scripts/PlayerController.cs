using UnityEngine;
using UnityEngine.UI;
using Terresquall;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public float moveSpeed = 4f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    private Vector3 moveVector = Vector3.zero;
    public Material[] mats;
    public Detector detector;
    private bool isGrounded = false;


    private Vector2 startTouchPos;
    private bool isSwipe;
    public float minSwipeDist = 100f;
    public float jumpForce = 7f;
    private bool isJumping = false;
    public float jumpGravity = 3f;

    private float lastTapTime = 0f;
    public float doubleTapTime = 0.3f;
    private bool isSprinting = false;

    public Image fadeImage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        GetComponent<FixedJoint>().connectedBody = detector.gameObject.GetComponent<Rigidbody>();

        StartCoroutine(FadeInEffect());
    }

    void Update()
    {
        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");
        moveVector = new Vector3(moveX, 0f, moveZ);

        foreach (Touch touch in Input.touches)
        {
            // Only check touches on the right side of the screen
            if (touch.position.x > Screen.width / 2f)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startTouchPos = touch.position;

                        // Check double tap
                        if (Time.time - lastTapTime <= doubleTapTime)
                        {
                            StartCoroutine(Sprint());
                        }
                        lastTapTime = Time.time;
                        break;

                    case TouchPhase.Moved:
                        Vector2 swipeVector = touch.position - startTouchPos;
                        if (swipeVector.magnitude > minSwipeDist && swipeVector.y > Mathf.Abs(swipeVector.x))
                        {
                            Jump();
                            startTouchPos = touch.position; // prevent multiple jumps in one swipe
                        }
                        break;

                    case TouchPhase.Ended:
                        isSwipe = false;
                        break;
                }
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("end"))
        {
            FadeOutAndLoadScene();
        }
    }

    private System.Collections.IEnumerator Sprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            float originalSpeed = moveSpeed;
            moveSpeed = sprintSpeed;

            yield return new WaitForSeconds(1f);

            moveSpeed = originalSpeed;
            isSprinting = false;
        }
    }

    public void FadeOutAndLoadScene()
    {
        StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeInEffect()
    {
        fadeImage.gameObject.SetActive(true);
        Color startColor = fadeImage.color;
        startColor.a = 1f;
        fadeImage.color = startColor;

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / 1f);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator FadeOutEffect()
    {
        fadeImage.gameObject.SetActive(true); 
        Color startColor = fadeImage.color;
        startColor.a = 0f; 
        fadeImage.color = startColor;

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / 1f);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

        SceneManager.LoadScene("MagnetSimulation");
    }
}

