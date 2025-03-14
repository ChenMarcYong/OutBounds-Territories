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
    public float rotationSpeed = 0.67f;
    public float runSpeedMultiplyer = 1.5f;


    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool isRunning;
    private float speedMult = 1f;
    public GameObject LookAt;

    
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
        isRunning = false;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

        //if (isGrounded) moveDirection = transform.forward * moveInput.y * runSpeedMultiplyer + transform.right * moveInput.x;
        //UnityEngine.Debug.Log(moveInput.y);
        if(moveInput.y == 1) moveDirection = transform.forward * moveInput.y * speedMult + transform.right * moveInput.x;
        else moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        //virtualCamera.transform.position = transform.position;
        //virtualCamera.transform.rotation = transform.rotation;

        xRotation -= mouseY; // Inverser car dans Unity, +Y baisse la caméra
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);


        if (Cursor.lockState == CursorLockMode.Locked) 
        {
            
            LookAt.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        } 

        HandleCursorLock();
        //cameraTransform.localRotation.x = Quaternion.Euler(xRotation, 0f, 0f);
    }


    void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    // ------------On Functions------------


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            isRunning = !isRunning;
            speedMult = isRunning ? runSpeedMultiplyer : 1f;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        //UnityEngine.Debug.Log("Jump");
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
