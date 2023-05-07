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
    private Vector2 lookVector;
    private bool lookExecuted;

    private bool takingDamage;

    public Transform characterMesh;
    public Transform cam;
    public Transform orientation;
    public Transform rotationPoint;
    public Transform attackInteractionZone;
    public Transform pauseCanvas;
    public Transform damageCanvas;
    public Transform spline;

    public float speed = 4000;
    public float maxSpeed = 5000;
    public float jumpHeight = 20000;
    public float counterMovement = 500;
    public float cameraSensitivityX = 0.1f;
    public float cameraSensitivityY = 0.1f;
    [HideInInspector] public bool doubleJumpActive;

    [HideInInspector] public GameObject target;
    [HideInInspector] public bool targeting;

    public float iFrames = 1;
    [HideInInspector] public bool onSpline;
    private bool onSplineMoving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        grounded = true;
        doubleJump = false;
        doubleJumpActive = false;
        lookExecuted = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Targeting
        target = null;
        targeting = false;

        onSpline = false;
        onSplineMoving = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!onSpline)
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

        else if (onSpline)
        {
            if (onSplineMoving)
            {
                if (movementX < 0)
                {
                    spline.GetComponent<SplineController>().SetSplineSpeed(-movementX * Time.deltaTime * speed / 500);
                    animator.SetBool("isMoving", true);
                    characterMesh.localRotation = Quaternion.Euler(0, -90, 0);
                }
                else if (movementX > 0)
                {
                    spline.GetComponent<SplineController>().SetSplineSpeed(-movementX * Time.deltaTime * speed / 500);
                    animator.SetBool("isMoving", true);
                    characterMesh.localRotation = Quaternion.Euler(0, 90, 0);
                }
            }
            else
            {
                spline.GetComponent<SplineController>().SetSplineSpeed(-Time.deltaTime / 1.2f);
                animator.SetBool("isMoving", false);
            }
        }

        // Move camera
        // If not paused and look not executed
        if (Time.timeScale != 0 && !lookExecuted)
        {
            cam.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
            //cam.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);

            orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
            //orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY); Specifically not this
            characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
            //characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);
        }
        lookExecuted = false;

        // Targeting
        if (targeting)
        {
            //Calculate angle of enemy to player
            //Angle of enemy to player + 180 = opposite of camera to enemy
            /*
             * float angle = 
             * cam.transform.RotateAround(rotationPoint.transform.position, Vector3.up, 0);
            */
            cam.LookAt(target.transform);
        }

        // Taking damage
        if (takingDamage)
        {
            if (iFrames <= 0)
            {
                iFrames = 1;
                takingDamage = false;
                damageCanvas.gameObject.SetActive(false);
            }
            else
            {
                iFrames -= Time.deltaTime;
            }
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

        if (onSpline && movementVector.x != 0)
        {
            onSplineMoving = true;
        }
        else
        {
            onSplineMoving = false;
        }
    }

    private void OnLook(InputValue lookValue)
    {
        /*
         * Called when the player inputs a camera direction on their keyboard/controller
         * Moves camera
         */

        if (!onSpline)
        {
            lookVector = lookValue.Get<Vector2>();
            // If not paused
            if (Time.timeScale != 0)
            {
                cam.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
                //cam.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);

                orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
                //orientation.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY); Specifically not this
                characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.up, lookVector.x * cameraSensitivityX);
                //characterMesh.transform.RotateAround(rotationPoint.transform.position, Vector3.left, lookVector.y * cameraSensitivityY);

                lookExecuted = true;
            }
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
                //Animate pressing button
                animator.SetBool("isPressing", true);
                cam.LookAt(target.transform);
                //AttackInteractionZoneController will handle the rest
            }
            //If no interactions do attack animation
            else
            {
                animator.SetBool("isAttacking", true);
                GameObject enemy = attackInteractionZone.GetComponent<AttackInteractionZoneController>().IsEnemy();
                if (enemy != null)
                {
                    enemy.GetComponent<AIController>().TakeDamage();
                }
            }
        }
    }

    private void OnPause()
    {
        pauseCanvas.GetComponent<CanvasController>().PauseGame();
    }

    private void OnTarget()
    {
        if (!onSpline)
        {
            if (target != null && !targeting)
            {
                targeting = true;
                cam.LookAt(target.transform);
            }

            else if (targeting)
            {
                targeting = false;
                cam.LookAt(this.transform);
                cam.Rotate(-22, 0, 0);
                //cam.rotation = Quaternion.Euler(15, cam.rotation.y, cam.rotation.z);
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
         * Handle ground collision detection
         */

        if (doubleJump)
        {
            // All objects with the "Ground" layer
            int layer = collision.gameObject.layer;
            if (layer == 6)
            {
                grounded = true;

                doubleJump = false;

                // Land animation
                animator.SetBool("isJumping", false);
            }

            // All objects with the "Platform" layer
            else if (layer == 7)
            {
                grounded = true;

                doubleJump = false;

                // Land animation
                animator.SetBool("isJumping", false);

                // Add momentum to player
            }
        }

        doubleJump = true;
    }

    public void UpdateTriggerList(GameObject enemy)
    {
        attackInteractionZone.GetComponent<AttackInteractionZoneController>().triggerList.Remove(enemy);
    }

    public void TakeDamage()
    {
        if (!takingDamage)
        {
            takingDamage = true;
            damageCanvas.gameObject.SetActive(true);
        }
    }

    public void StartSpline()
    {
        rb.velocity = Vector3.zero;

        onSpline = true;
        movementX = 0;
        movementY = 0;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);

        cam.transform.localPosition = new Vector3(0, 3.5f, -4.5f);
        cam.LookAt(this.transform.position);
        cam.Rotate(-22, 0, 0);
        //cam.transform.rotation = Quaternion.Euler(15, 0, 0);
        //cam.RotateAround(this.transform.position, Vector3.up, 90);
        orientation.rotation = cam.rotation;
        orientation.position = cam.position;

        characterMesh.transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    public void EndSpline()
    {
        onSpline = false;

        cam.LookAt(this.transform.position);
        cam.Rotate(-22, 0, 0);
        //cam.transform.localPosition = new Vector3(0, 3.5f, -4.5f);
        //cam.transform.rotation = Quaternion.Euler(15, 0, 0);
        //cam.RotateAround(this.transform.position, Vector3.up, -90);
        orientation.rotation = cam.rotation;

        characterMesh.transform.rotation = cam.rotation;
        characterMesh.transform.Rotate(new Vector3(-15, 0, 0));
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //For targeting
        //8 = Target
        if (other.gameObject.layer == 8)
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //For targeting
        if (other.gameObject == target)
        {
            target = null;
            targeting = false;
            cam.LookAt(this.transform);
            cam.rotation = Quaternion.Euler(15, 0, 0);
        }
    }
    */

}
