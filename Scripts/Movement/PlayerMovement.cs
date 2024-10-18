using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float playerHeight;

    public Transform orientation;

    [Header("Movement")]
    public float moveSpeed;
    public float moveMultiplier;
    public float airMultiplier;
    public float counterMovement;

    public float jumpForce;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("CounterMovement")]
    public float maxSpeed;
    public float walkMaxSpeed;
    public float sprintMaxSpeed;
    public float airMaxSpeed;

    [Header("Ground Detection")]
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;

    private float horizontalInput;
    private float verticalInput;

    public bool grounded;

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    private Rigidbody rb;

    RaycastHit slopeHit;

    public TextMeshProUGUI text_speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);

        MyInput();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && grounded)
        {
            // Jump
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void ControlSpeed()
    {
        if (grounded && Input.GetKey(sprintKey))
            maxSpeed = sprintMaxSpeed;

        else if (grounded)
            maxSpeed = walkMaxSpeed;

        // no specific airMaxSpeed for now;
        //else
        //    maxSpeed = airMaxSpeed;
    }

    private void MovePlayer()
    {
        float x = horizontalInput;
        float y = verticalInput;

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        moveDirection = orientation.forward * y + orientation.right * x;

        // on slope
        if (OnSlope())
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Force);

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Force);

        // limit rb velocity
        Vector3 rbFlatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (rbFlatVelocity.magnitude > maxSpeed)
        {
            rbFlatVelocity = rbFlatVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(rbFlatVelocity.x, rb.velocity.y, rbFlatVelocity.z);
        }
    }

    private void Jump()
    {
        if (!grounded)
            return;

        // reset rb y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded) return;

        float threshold = 0.01f;

        //Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
                return true;
        }

        return false;
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
}