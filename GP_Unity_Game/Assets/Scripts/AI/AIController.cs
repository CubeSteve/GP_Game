using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AIController : MonoBehaviour
{
    public WaypointController waypointController;

    [HideInInspector] public Transform nextWaypoint;
    private int nextWaypointIndex;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public TargetState targetState;
    private GameObject player;
    private Rigidbody rb;
    private bool grounded;

    private int hp;
    private float damageTimer;
    private int attackRange;
    private float attackDelay;

    public enum TargetState
    {
        Patroling,
        Spotted,
        Chasing,
        Attacking
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetState = TargetState.Patroling;
        nextWaypointIndex = 0;
        nextWaypoint = waypointController.GetWaypoint(nextWaypointIndex);
        rb = GetComponent<Rigidbody>();
        grounded = true;

        agent.SetDestination(nextWaypoint.position);

        player = GameObject.FindWithTag("Player");

        if (this.transform.localScale == new Vector3(5, 5, 5))
        {
            hp = 3;
            attackRange = 10;
        }
        damageTimer = 0;
        attackDelay = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //Damage colour
        if (damageTimer < 0)
        {
            damageTimer = 0;
            this.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 0.8f);
        }
        else if (damageTimer != 0)
        {
            damageTimer -= Time.deltaTime;
        }

        //Attack delay
        if (attackDelay < 0)
        {
            attackDelay = 0;
        }
        else if (attackDelay != 0)
        {
            attackDelay -= Time.deltaTime;
        }

        switch (targetState)
        {
            case TargetState.Patroling:
                //Check if waypoint reached
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.stoppingDistance <= agent.stoppingDistance)
                    {
                        //Target next waypoint
                        nextWaypointIndex = waypointController.GetNextWaypointIndex(nextWaypointIndex);
                        nextWaypoint = waypointController.GetWaypoint(nextWaypointIndex);
                        agent.SetDestination(nextWaypoint.position);
                    }
                }
                break;

            case TargetState.Spotted:
                //Attack animation here
                targetState = TargetState.Chasing;
                agent.isStopped = false;
                agent.SetDestination(player.GetComponent<Transform>().position);
                break;

            case TargetState.Chasing:
                //If player out of range of next destination, update it
                Vector3 a = agent.destination;
                if (Vector3.Distance(agent.destination, player.GetComponent<Transform>().position) < 1)
                {
                    agent.SetDestination(player.GetComponent<Transform>().position);
                }

                //If next to player and no attack delay, attack them
                if (attackDelay == 0 && Vector3.Distance(this.GetComponent<Transform>().position, player.GetComponent<Transform>().position) < attackRange)
                {
                    targetState = TargetState.Attacking;
                    agent.enabled = false;
                    rb.isKinematic = false;

                    rb.AddRelativeForce(new Vector3(0,200,200));
                    grounded = false;
                }
                break;

            case TargetState.Attacking:
                break;
        }
    }

    public void TakeDamage()
    {
        hp--;
        //Damage color
        damageTimer = 1;
        this.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.8f);

        //Death check
        if (hp <= 0)
        {
            if (this.transform.localScale == new Vector3(5, 5, 5))
            {
                this.transform.localScale = new Vector3(3, 3, 3); // Scale
                this.GetComponent<NavMeshAgent>().baseOffset = 0.35f; // Stop floating
                hp = 2; // New hp
                attackRange = 6; // Update range to scale
                this.transform.GetChild(0).GetComponent<CapsuleCollider>().radius = 8; // Update detection radius to scale

                agent.enabled = true;
                rb.isKinematic = true;
                grounded = true;
                targetState = TargetState.Chasing;

                GameObject.Instantiate(this, this.transform.position, this.transform.rotation); // Create another AI
            }
            else if (this.transform.localScale == new Vector3(3, 3, 3))
            {
                this.transform.localScale = new Vector3(1, 1, 1);
                this.GetComponent<NavMeshAgent>().baseOffset = 0f;
                hp = 1;
                attackRange = 2;
                this.transform.GetChild(0).GetComponent<CapsuleCollider>().radius = 25;

                agent.enabled = true;
                rb.isKinematic = true;
                grounded = true;
                targetState = TargetState.Chasing;

                GameObject.Instantiate(this, this.transform.position, this.transform.rotation);
            }
            else
            {
                player.GetComponent<PlayerController>().UpdateTriggerList(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!grounded)
        {
            // All objects with the "Ground" layer
            int layer = collision.gameObject.layer;
            if (layer == 6)
            {
                agent.enabled = true;
                rb.isKinematic = true;
                grounded = true;
                targetState = TargetState.Chasing;
                attackDelay = 2;
            }

            // Player collsion
            else if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }
}
