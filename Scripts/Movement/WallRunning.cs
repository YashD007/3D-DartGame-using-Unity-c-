using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    public Transform orientation;

    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunJumpForce;

    [Header("Detection")]
    public float wallDistance;
    public float minJumpHeight;

    [Header("Gravity")]
    public bool useGravity;
    public float customGravity;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private bool wallLeft;
    private bool wallRight;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWall();

        if (CanWallRun())
        {
            if (wallLeft || wallRight)
                StartWallRun();

            else
                StopWallRun();
        }
    }

    private void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, whatIsWall);

        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, whatIsWall);
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StartWallRun()
    {
        rb.useGravity = useGravity;

        rb.AddForce(Vector3.down * customGravity, ForceMode.Force);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;

                // reset y velocity
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100f, ForceMode.Force);
            }
            else
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;

                // reset y velocity
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100f, ForceMode.Force);
            }
        }
    }

    private void StopWallRun()
    {
        rb.useGravity = true;
    }
}
