using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public Transform GetWaypoint(int waypointIndex)
    {
        // Returns waypoint Transform from given index
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        // Returns next waypoint index in list
        int nextWaypointIndex = currentWaypointIndex + 1;

        // If at end of waypoint list return first waypoint
        if (nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }

        return nextWaypointIndex;
    }
}
