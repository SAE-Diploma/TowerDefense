using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float runSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] bool moveInAir;
    bool isGrounded;
    bool isMoving;

    Vector3 inputDir;
    Vector3 forward;
    RaycastHit groundHit;
    Rigidbody m_rigidbody;
    CapsuleCollider m_collider;
    float rayLength;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        rayLength = m_collider.bounds.extents.y + 0.2f;
    }

    void Update()
    {
        
        inputDir = (forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized;

        // multiply inputDir with correct speed
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputDir *= runSpeed;
            }
            else
            {
                inputDir *= speed;
            }
        } else { inputDir *= airSpeed; }

        //Debug.DrawLine(transform.position, transform.position + forward * 10, Color.red, Time.deltaTime);

    }

    private void FixedUpdate()
    {
        // check for the ground and set the forward vector parallel to it
        if (Physics.Raycast(transform.position, -transform.up, out groundHit,rayLength))
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
                    m_rigidbody.MovePosition(transform.position + inputDir * Time.deltaTime);
                    isMoving = true;
                }
                else { isMoving = false; }
            }
            else
            {
                isMoving = true;
                m_rigidbody.MovePosition(transform.position + inputDir * Time.deltaTime);
            }
        } else { isMoving = false; }

        // jumping when on the ground
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (isMoving)
            {
                m_rigidbody.AddForce(Vector3.up * jumpForce + inputDir.normalized*jumpForce, ForceMode.Impulse);
            }
            else
            {
                m_rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

}
