using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteractionZoneController : MonoBehaviour
{
    [HideInInspector] public List<GameObject> triggerList;

    private void Start()
    {
        triggerList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerList.Contains(other.gameObject))
        {
            triggerList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other.gameObject))
        {
            triggerList.Remove(other.gameObject);
        }
    }

    public bool IsInteract()
    {
        //In case triggerList is empty
        if (triggerList.Count != 0)
        {
            foreach (var trigger in triggerList)
            {
                if (trigger == null)
                {
                    triggerList.Remove(trigger);
                    return false;
                }
                //Check it has a switch controller
                else if (trigger.gameObject.GetComponent<SwitchController>() != null && trigger.gameObject.GetComponent<SwitchController>().ActivateDoor())
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    public GameObject IsEnemy()
    {
        if (triggerList.Count != 0)
        {
            foreach (var trigger in triggerList)
            {
                if (trigger.gameObject.tag == "Enemy")
                {
                    return trigger.gameObject;
                }
            }
        }

        return null;
    }
}
