using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Declare variables
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public Transform characterMesh;
    public Transform cam;
    public Transform orientation;
    public Transform rotationPoint;
    public float speed = 1000;
    public float maxSpeed = 1000;
    public float counterMovement = 0.5f;
    public float cameraSensitivityX = 0.1f;
    public float cameraSensitivityY = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        CounterMovement(movement);
        //rb.AddForce(movement.x * speed, 0, movement.z * speed);

        rb.AddForce(orientation.transform.forward * movement.z * speed);
        rb.AddForce(orientation.transform.right * movement.x * speed);

        // print("X Vel: " + rb.velocity.x + " Mag: " + movement.x + " mmX: " + movementX + " --- Y Vel: " + rb.velocity.z + " Mag: " + movement.z + " mmY: " + movementY);
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

        Vector2 lookVector = lookValue.Get<Vector2>();

        cam.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
        cam.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);
        orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
        orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);
        characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
        characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);
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
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float xMag = magnitude * Mathf.Cos(v * Mathf.Rad2Deg);
        float yMag = magnitude * Mathf.Cos(u * Mathf.Rad2Deg);

        return new Vector2(xMag, yMag);
    }

}
