using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AIController : MonoBehaviour
{
    public WaypointController waypointController;

    private Transform nextWaypoint;
    private int nextWaypointIndex;

    private NavMeshAgent agent;
    private TargetState targetState;
    private GameObject player;

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

        agent.SetDestination(nextWaypoint.position);

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(targetState)
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

                //If next to player, attack them
                if (Vector3.Distance(this.GetComponent<Transform>().position, player.GetComponent<Transform>().position) < 3)
                {
                    targetState = TargetState.Attacking;
                }
                break;

            case TargetState.Attacking:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player enters enemy view range
        if (other.tag == "Player")
        {
            targetState = TargetState.Spotted;
            agent.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Player exits view range
        if (other.tag == "Player")
        {
            targetState = TargetState.Patroling;
            agent.isStopped = false;
            agent.SetDestination(nextWaypoint.position);
        }
    }
}
