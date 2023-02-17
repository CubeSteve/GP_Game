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

    public float speed = 1000;
    public float maxSpeed = 1000;
    public float counterMovement = 1.0f;

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
        rb.AddForce(movement.x * speed, 0, movement.z * speed);

        // print("X Vel: " + rb.velocity.x + " Mag: " + movement.x + " mmX: " + movementX + " --- Y Vel: " + rb.velocity.z + " Mag: " + movement.z + " mmY: " + movementY);
    }

    // Methods
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

        // Restrict if at max speed
        if (movementX > 0 && movementX > maxSpeed) movementX = 0;
        if (movementX < 0 && movementX < -maxSpeed) movementX = 0;
        if (movementY > 0 && movementY > maxSpeed) movementY = 0;
        if (movementY < 0 && movementY < -maxSpeed) movementY = 0;
    }

    private void CounterMovement(Vector3 mag)
    {
        // rb.velocity.x > 0 && mag.x < 0.5 || rb.velocity.x < 0 && mag.x > -0.5
        if (rb.velocity.x > 0 && mag.x < 0.5 || rb.velocity.x < 0 && mag.x > -0.5)
        {
            rb.AddForce(-rb.velocity.x * speed * counterMovement, 0, 0);
        }
        if (rb.velocity.z > 0 && mag.z < 0.5 || rb.velocity.z < 0 && mag.z > -0.5)
        {
            rb.AddForce(0, 0, -rb.velocity.z * speed * counterMovement);
        }
    }

}
