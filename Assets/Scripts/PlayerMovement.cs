using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public float duckHeight = 1.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private float originalHeight;
    //private bool isDucking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //originalHeight = controller.height;
    }

    void Update()
    {
        HandleTurning();
        HandleMovement();
        HandleJump();
        //HandleDuck();
        ApplyGravity();

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleTurning()
    {
        if (Input.GetKeyDown(KeyCode.A))
            transform.Rotate(0f, -90f, 0f);
        else if (Input.GetKeyDown(KeyCode.D))
            transform.Rotate(0f, 90f, 0f);
    }

    void HandleMovement()
    {
        // Forward movement only
        //Vector3 forwardMove = transform.forward * speed;
        Vector3 forwardMove = transform.forward * speed * GameManager.Instance.speedMultiplier;

        velocity.x = forwardMove.x;
        velocity.z = forwardMove.z;
    }

    void HandleJump()
    {
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
        }
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // small downward force to stay grounded
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pit"))
        {
            GameManager.Instance.GameOver("Fell into a pit!");
        }
    }


    /*void HandleDuck()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDucking)
        {
            isDucking = true;
            controller.height = duckHeight;
            controller.center = new Vector3(0, duckHeight / 2f, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isDucking)
        {
            isDucking = false;
            controller.height = originalHeight;
            controller.center = new Vector3(0, originalHeight / 2f, 0);
        }
    }
    */
}


