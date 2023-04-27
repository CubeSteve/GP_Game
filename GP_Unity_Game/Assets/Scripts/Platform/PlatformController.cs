using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed = 0.1f;
    public WaypointController waypointController;

    private Transform previousWaypoint;
    private Transform nextWaypoint;
    private float waypointPercentage;

    private int nextWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        nextWaypointIndex = 0;
        TargetNextWaypoint();
        waypointPercentage = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, waypointPercentage);
        waypointPercentage += 0.01f * speed;

        if (waypointPercentage >=1)
        {
            TargetNextWaypoint();
            waypointPercentage = 0;
        }
    }

    private void TargetNextWaypoint()
    {
        //Targets the next waypoint in the list
        previousWaypoint = waypointController.GetWaypoint(nextWaypointIndex);
        nextWaypointIndex = waypointController.GetNextWaypointIndex(nextWaypointIndex);
        nextWaypoint = waypointController.GetWaypoint(nextWaypointIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Attach player to platform
        if (other.tag == "Player")
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Detach player from platform
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
