using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 2f;


    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isGrounded;

    private float xRotation = 0f;
    private float mouseX;
    private float mouseY;
    public float verticalClamp = 90f;


    void Awake() 
    {
        rb = GetComponent<Rigidbody>();

        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        if (virtualCamera != null)
        {
            cameraTransform = virtualCamera.transform;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {


        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        transform.Rotate(Vector3.up * mouseX);
        virtualCamera.transform.position = transform.position;
        virtualCamera.transform.rotation = transform.rotation;

        xRotation -= mouseY; // Inverser car dans Unity, +Y baisse la caméra
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        //cameraTransform.localRotation.x = Quaternion.Euler(xRotation, 0f, 0f);
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

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>(); // Stocke l'input de la souris

        mouseX = lookInput.x * rotationSpeed;
        mouseY = lookInput.y * rotationSpeed;

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
