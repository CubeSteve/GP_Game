using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetAreaController : MonoBehaviour
{
    PlayerController parentScript;

    private void Start()
    {
        //Parent script
        parentScript = this.transform.parent.gameObject.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //For targeting
        //8 = Target
        if (other.gameObject.layer == 8)
        {
            parentScript.target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //For targeting
        if (other.gameObject == parentScript.target)
        {
            parentScript.target = null;
            parentScript.targeting = false;
            parentScript.cam.LookAt(this.transform.parent.gameObject.transform);
            parentScript.cam.Rotate(-22, 0, 0);
            //parentScript.cam.rotation = Quaternion.Euler(15, parentScript.cam.rotation.y, parentScript.cam.rotation.z);
        }
    }
}
