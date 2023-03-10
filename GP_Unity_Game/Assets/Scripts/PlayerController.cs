using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Declare variables
    private Rigidbody rb;
    private Animator animator;
    private float movementX;
    private float movementY;
    private bool grounded;
    private bool doubleJump;

    public Transform characterMesh;
    public Transform cam;
    public Transform orientation;
    public Transform rotationPoint;
    public Transform attackInteractionZone;
    public float speed = 4000;
    public float maxSpeed = 5000;
    public float jumpHeight = 20000;
    public float counterMovement = 500;
    public float cameraSensitivityX = 0.1f;
    public float cameraSensitivityY = 0.1f;
    public bool doubleJumpActive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        grounded = true;
        doubleJump = false;
        doubleJumpActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        CounterMovement(movement);

        rb.AddForce(orientation.transform.forward * movement.z * speed);
        rb.AddForce(orientation.transform.right * movement.x * speed);

        // print("X Vel: " + rb.velocity.x + " Mag: " + movement.x + " mmX: " + movementX + " --- Y Vel: " + rb.velocity.z + " Mag: " + movement.z + " mmY: " + movementY);

        // Update animation
        if ((rb.velocity.x > 0.2 || rb.velocity.x < -0.2 || rb.velocity.z > 0.2 || rb.velocity.z < -0.2) && animator.GetBool("isMoving") == false)
        {
            animator.SetBool("isMoving", true);
        }
        else if (((rb.velocity.x < 0.2 && rb.velocity.x > -0.2) || (rb.velocity.z < 0.2 && rb.velocity.z > -0.2)) && animator.GetBool("isMoving") == true)
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Methods
    private void OnMove(InputValue movementValue)
    {
        /*
         * Called when the player inputs a movement direction on their keyboard/controller
         * Applies input value to movement value
         */

        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

        // Restrict if at max speed
        if (movementX > 0 && movementX > maxSpeed) movementX = 0;
        if (movementX < 0 && movementX < -maxSpeed) movementX = 0;
        if (movementY > 0 && movementY > maxSpeed) movementY = 0;
        if (movementY < 0 && movementY < -maxSpeed) movementY = 0;
    }

    private void OnLook(InputValue lookValue)
    {
        /*
         * Called when the player inputs a camera direction on their keyboard/controller
         * Moves camera
         */

        // If not paused
        if (Time.timeScale != 0)
        {
            Vector2 lookVector = lookValue.Get<Vector2>();

            cam.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
            //cam.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);

            orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
            //orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY); Specifically not this
            characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
            //characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);
        }

    }

    private void OnJump(InputValue jumpValue)
    {
        if (grounded)
        {
            rb.AddForce(0, jumpHeight, 0);
            grounded = false;

            if (doubleJumpActive == false)
            {
                doubleJump = false;
            }

            //jump animation
            animator.SetBool("isJumping", true);
        }
    }

    private void OnAttack(InputValue attackValue)
    {
        if (!animator.GetBool("isAttacking"))
        {
            //Check if interactions available
            if (attackInteractionZone.GetComponent<AttackInteractionZoneController>().IsInteract())
            {
                //AttackInteractionZoneController will handle the rest
            }
            //If no interactions do attack animation
            else
            {
                animator.SetBool("isAttacking", true);
            }
        }
    }

    private void CounterMovement(Vector3 input)
    {
        /*
         * Applies velocity opposite to the players current velocity when there is no movement input
         * (V This note is definitely wrong V)
         * (V This is probably wrong V)
         * Might have to be relitive to players looking direction
         * 
         * Nvm none of this old code matters
         * (Scroll down a bit you'll get to it)
         */

        /*
        // rb.velocity.x > 0 && mag.x < 0.5 || rb.velocity.x < 0 && mag.x > -0.5
        if (rb.velocity.x > 0 && Mathf.Abs(mag.x) == 0 || rb.velocity.x < 0 && Mathf.Abs(mag.x) == 0)
        {
            //rb.AddForce(-rb.velocity.x * speed * counterMovement, 0, 0);
            rb.AddForce(-orientation.transform.right * speed * counterMovement);
        }
        if (rb.velocity.z > 0 && Mathf.Abs(mag.z) == 0 || rb.velocity.z < 0 && Mathf.Abs(mag.z) == 0)
        {
            //rb.AddForce(0, 0, -rb.velocity.z * speed * counterMovement);
            rb.AddForce(-orientation.transform.forward * speed * counterMovement);
        }
        */

        //Vector2 mag = FindVelRelativeToLook();

        //print(mag.x + ", " + mag.y);

        //print(orientation.forward);

        //print(orientation.forward);

        /*
        if (mag.x > 0 && Mathf.Abs(input.x) == 0)
        {
            print("Right");
            //Vector3 aaaaa1 = -orientation.transform.right * speed * counterMovement;
            //print("ACTIVE X: " + aaaaa1.x.ToString() + ", " + aaaaa1.y.ToString() + ", " + aaaaa1.z.ToString());
            //rb.AddForce(-rb.velocity.x * speed * counterMovement, 0, 0);
            rb.AddForce(-orientation.transform.right * speed * counterMovement);
        }
        if (mag.x < 0 && Mathf.Abs(input.x) == 0)
        {
            print("Left");
            rb.AddForce(orientation.transform.right * speed * counterMovement);
        }
        if (mag.y > 0 && Mathf.Abs(input.z) == 0)
        {
            print("Forward");
            //Vector3 aaaaa2 = -orientation.transform.forward * speed * counterMovement;
            //print("ACTIVE Y: " + aaaaa2.x.ToString() + ", " + aaaaa2.y.ToString() + ", " + aaaaa2.z.ToString());
            //rb.AddForce(0, 0, -rb.velocity.z * speed * counterMovement);
            rb.AddForce(-orientation.transform.forward * speed * counterMovement);
        }
        if(mag.y < 0 && Mathf.Abs(input.z) == 0)
        {
            print("Backward");
            rb.AddForce(orientation.transform.forward * speed * counterMovement);
        }
        */

        if (rb.velocity.x > 0 || rb.velocity.x < 0)
        {
            rb.AddForce(-rb.velocity.x * counterMovement, 0, 0);
        }
        if (rb.velocity.z > 0 || rb.velocity.z < 0)
        {
            rb.AddForce(0, 0, -rb.velocity.z * counterMovement);
        }
    }

    private Vector2 FindVelRelativeToLook()
    {
        /*
         * I might need this if i want to change how countermovement is calculated
         */
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float xMag = magnitude * Mathf.Cos(v * Mathf.Rad2Deg);
        float yMag = magnitude * Mathf.Cos(u * Mathf.Rad2Deg);

        return new Vector2(xMag, yMag);
    }

    private void OnCollisionStay(Collision collision)
    {
        /*
         * Handle ground collision detection on all objects with the "Ground" layer
         */

        if (doubleJump)
        {
            // Only check if the layer is the ground layer
            int layer = collision.gameObject.layer;
            if (layer != 6)
            {
                return;
            }

            grounded = true;

            doubleJump = false;

            //Land animation
            animator.SetBool("isJumping", false);
        }

        doubleJump = true;
    }

}
