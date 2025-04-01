using Unity.VisualScripting;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;
    public float jumpHeight;
    float jumpMultiplier;
    public float gravity;
    
    public GameObject cam;

    public LayerMask groundLayer;
    public float groundDistance;

    public bool isGrounded;
    Vector2 inputs;
    public Rigidbody rb;

    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 velocity = new Vector3(0f, gravity, inputs.y);
        velocity = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * velocity;

        rb.AddForce(velocity * moveSpeed * 1000f * Time.deltaTime);

        rb.angularVelocity = new Vector3(rb.angularVelocity.x, inputs.x * rotSpeed * Mathf.Deg2Rad, rb.angularVelocity.z);
    }

    void Jump()
    {
        if (Input.GetKey("space") && isGrounded)
        {
            if (jumpMultiplier < 1f)
            {
                jumpMultiplier += Time.deltaTime * 2f;
            }
        }
        if (Input.GetKeyUp("space"))
        {
            rb.AddForce(Vector3.up * jumpHeight * jumpMultiplier * 1000f);
            jumpMultiplier = 0f;
        }

        if (Physics.Raycast(rb.transform.position, Vector3.down, out hit, groundDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(rb.transform.position, Vector3.down * groundDistance);
    }
}
