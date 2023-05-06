using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AIController;

public class DetectionAreaController : MonoBehaviour
{
    private AIController parentScript;

    // Start is called before the first frame update
    void Start()
    {
        parentScript = this.transform.parent.gameObject.GetComponent<AIController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player enters enemy view range
        if (parentScript.agent.isActiveAndEnabled && other.tag == "Player")
        {
            parentScript.targetState = TargetState.Spotted;
            parentScript.agent.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Player exits view range
        if (parentScript.agent.isActiveAndEnabled && other.tag == "Player")
        {
            parentScript.targetState = TargetState.Patroling;
            parentScript.agent.isStopped = false;
            parentScript.agent.SetDestination(parentScript.nextWaypoint.position);
        }
    }
}
