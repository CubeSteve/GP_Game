using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public WaypointController waypointController;

    private Transform nextWaypoint;
    private int nextWaypointIndex;

    private NavMeshAgent agent;
    private TargetState targetState;

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

        agent.SetDestination(nextWaypoint.position);
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
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetState = TargetState.Spotted;
            agent.isStopped = true;
        }
    }
}
