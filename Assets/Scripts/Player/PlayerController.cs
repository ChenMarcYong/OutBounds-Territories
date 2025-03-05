using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;


    void Awake() 
    {
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }



    // ------------On Functions------------


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        UnityEngine.Debug.Log("Jump");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        //UnityEngine.Debug.Log("Grounded");
    }
}
