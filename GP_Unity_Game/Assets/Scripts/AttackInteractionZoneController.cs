using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteractionZoneController : MonoBehaviour
{
    List<Collider> triggerList;

    private void Start()
    {
        triggerList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerList.Contains(other))
        {
            triggerList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other))
        {
            triggerList.Remove(other);
        }
    }

    public bool IsInteract()
    {
        //In case triggerList is empty
        if (triggerList.Count != 0)
        {
            foreach (var trigger in triggerList)
            {
                //Check it has a switch controller
                if (trigger.gameObject.GetComponent<SwitchController>() != null && trigger.gameObject.GetComponent<SwitchController>().ActivateDoor())
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}
