using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DoorController : MonoBehaviour
{
    public GameObject director;

    public void OpenDoor()
    {
        //gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
        //gameObject.GetComponent<BoxCollider>().enabled = !gameObject.GetComponent<BoxCollider>().enabled;

        //Play animation
        director.GetComponent<PlayableDirector>().Play();
        //Disable player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().enabled = false;
    }

    public void AnimationEnd()
    {
        //Enable player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().enabled = true;
    }
}
