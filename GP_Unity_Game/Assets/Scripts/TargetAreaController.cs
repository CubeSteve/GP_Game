using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetAreaController : MonoBehaviour
{
    private PlayerController parentScript;
    private bool wait;
    private float waitTime;

    private void Start()
    {
        //Parent script
        parentScript = this.transform.parent.gameObject.GetComponent<PlayerController>();
        wait = false;
        waitTime = 1;
    }

    private void Update()
    {
        if (wait)
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //For targeting
        //8 = Target
        wait = false;
        waitTime = 1;
        if (other.gameObject.layer == 8)
        {
            parentScript.target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Start wait time

        //For targeting
        if (other.gameObject == parentScript.target)
        {
            wait = true;
            if (waitTime <= 0)
            {
                wait = false;
                waitTime = 1;
                stopTargeting();
            }
        }
    }

    private void stopTargeting()
    {
        parentScript.target = null;
        parentScript.targeting = false;
        parentScript.cam.LookAt(this.transform.parent.gameObject.transform);
        parentScript.cam.Rotate(-22, 0, 0);
        //parentScript.cam.rotation = Quaternion.Euler(15, parentScript.cam.rotation.y, parentScript.cam.rotation.z);
    }
}
