using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float runSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] bool moveInAir;

    private bool isGrounded;
    private bool isMoving;
    private bool canMove = true;
    private bool jump = false;
    private bool isRunning = false;
    public bool CanMove => canMove;

    Vector3 inputDir;
    Vector3 forward;
    RaycastHit groundHit;
    Rigidbody m_rigidbody;
    CapsuleCollider m_collider;
    float rayLength;

    public InputMaster inputMaster { get; private set; }
    private void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();
        inputMaster.Player.Jump.performed += OnJump;
        inputMaster.Player.Run.started += OnStartRunning;
        inputMaster.Player.Run.canceled += OnStopRunning;
    }



    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        rayLength = m_collider.bounds.extents.y + 0.2f;
        canMove = true;
    }

    void Update()
    {
        Vector2 moveDir = inputMaster.Player.Movement.ReadValue<Vector2>();
        inputDir = transform.right * moveDir.x + transform.forward * moveDir.y;

        // multiply inputDir with correct speed
        if (isGrounded)
        {
            if (isRunning)
            {
                inputDir *= runSpeed;
            }
            else
            {
                inputDir *= speed;
            }
        }
        else { inputDir *= airSpeed; }

    }

    private void FixedUpdate()
    {
        // disable movement when wanted
        if (!canMove) return;

        // check for the ground and set the forward vector parallel to it
        if (Physics.Raycast(transform.position, -transform.up, out groundHit, rayLength))
        {
            forward = Vector3.Cross(transform.right, groundHit.normal);
            if (groundHit.transform.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }
        else
        {
            forward = transform.forward;
            isGrounded = false;
        }

        // movement
        if (inputDir != Vector3.zero)
        {
            if (!isGrounded)
            {
                if (moveInAir)
                {
                    isMoving = true;
                    m_rigidbody.MovePosition(transform.position + inputDir * Time.fixedDeltaTime);
                }
                else { isMoving = false; }
            }
            else
            {
                isMoving = true;
                m_rigidbody.MovePosition(transform.position + inputDir * Time.fixedDeltaTime);
            }
        }
        else { isMoving = false; }

        // jumping when on the ground
        if (jump && isGrounded)
        {
            if (isMoving)
            {
                m_rigidbody.AddForce(Vector3.up * jumpForce + inputDir.normalized * jumpForce, ForceMode.Impulse);
            }
            else
            {
                m_rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            jump = false;
        }
    }

    /// <summary>
    /// set the canMove variable from outside
    /// </summary>
    /// <param name="canMove">true|false</param>
    public void CanPlayerMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void OnJump(InputAction.CallbackContext obj) { jump = true; }

    private void OnStartRunning(InputAction.CallbackContext obj) { isRunning = true; }

    private void OnStopRunning(InputAction.CallbackContext obj) { isRunning = false; }

}
