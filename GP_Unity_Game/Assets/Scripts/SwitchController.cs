using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Transform linkedDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ActivateDoor()
    {
        linkedDoor.GetComponent<DoorController>().OpenDoor();
        return true;
    }
}
