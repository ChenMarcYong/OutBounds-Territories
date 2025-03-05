using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipController : MonoBehaviour
{
    // Start is called before the first frame update


    public InputAction playerControls;
    public float moveSpeed = 25.0f;

    public InputActionReference move;
    public InputActionReference ascend;
    public InputActionReference rotate;

    private Vector2 moveDirection = Vector2.zero;
    private float ascendSpeed = 5.0f;
    private Rigidbody rb;


    public float maxThrustSpeed = 1.5f;
    public float maxThrustEnergy = 100.0f;
    public float thrustSpeed;
    public float thrustEnergy;
    private bool isThrusting;
    private bool needsRestThrust;

    public float verticalMove;
    public float horizontalMove;
    public float mouseInputX;
    public float mouseInputY;
    public float rollInput;

    [SerializeField]
    float speedMult = 1.0f;

    [SerializeField]
    float speedMultAngle = 0.5f;

    [SerializeField]
    float speedRollMultAngle = 0.05f;

    void Start()
    {
        thrustSpeed = 1.0f;
        thrustEnergy = maxThrustEnergy;
        isThrusting = false;
        needsRestThrust = false;

        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        ascendSpeed = ascend.action.ReadValue<float>();

        float rollSpeed = 100f; // Vitesse de la roulade
        rollInput += moveDirection.x * rollSpeed * Time.deltaTime;

        // Gestion de la rotation avec la souris
        mouseInputX += rotate.action.ReadValue<Vector2>().x * speedMultAngle;
        mouseInputY += rotate.action.ReadValue<Vector2>().y * speedMultAngle;
        mouseInputY = Mathf.Clamp(mouseInputY, -90f, 90f);

        transform.rotation = Quaternion.Euler(-mouseInputY, mouseInputX, rollInput);


        if (isThrusting && !needsRestThrust)
        {
            thrustEnergy = Math.Max(thrustEnergy - 0.15f, 0.0f);
            thrustSpeed = Math.Min(thrustSpeed * 1.005f, maxThrustSpeed);

            if (thrustEnergy == 0.0f) needsRestThrust = true;


        }
        else
        {
            thrustEnergy = Math.Min(thrustEnergy + 0.075f, maxThrustEnergy);
            thrustSpeed = Math.Max(thrustSpeed * 0.995f, 1.0f);

            if (needsRestThrust && thrustEnergy >= 50.0f)
            {
                needsRestThrust = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector3(-moveDirection.x * moveSpeed, ascendSpeed * moveSpeed, -moveDirection.y * moveSpeed * thrustSpeed);

        Vector3 moveDir = transform.forward * (-moveDirection.y * moveSpeed * thrustSpeed)
                + transform.right * (-moveDirection.x * moveSpeed)
                + transform.up * (ascendSpeed * moveSpeed);

        rb.velocity = moveDir;
    }

    /*public void OnMove() 
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }*/
    public void OnThrust(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            isThrusting = true;
        }

        else if (context.canceled)
        {
            isThrusting = false;
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}