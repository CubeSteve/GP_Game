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
        //Try catch in case triggerList is empty
        try
        {
            foreach (var trigger in triggerList)
            {
                if (trigger.gameObject.GetComponent<SwitchController>().ActivateDoor())
                {
                    return true;
                }
            }

            return false;
        }

        catch
        {
            return false;
        }
    }
}
