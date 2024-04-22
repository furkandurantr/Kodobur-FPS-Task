using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float sprintSpeed = 18f;
    public float gravity = -9.81f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float bufferDistance = 4f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;

    bool jumpBuffer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        RaycastHit isJumpBuffer;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Physics.SphereCast(transform.position, groundDistance, Vector3.down * bufferDistance, out isJumpBuffer, groundMask);

        if (Input.GetButton("Jump") && isJumpBuffer.transform != null && velocity.y < 0) 
        {
            jumpBuffer = true;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if (jumpBuffer && isGrounded)   
        {
            jumpBuffer = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Vector3 move = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1f);

        //Sprinting to int
        int sprinting = Input.GetButton("Sprint") ? 1 : 0;

        //Find max speed
        float mySpeed = Mathf.Max(speed,  sprinting * sprintSpeed);
        controller.Move(move * mySpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
    }
}
