using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    doubleJump
}

public class PowerUpController : MonoBehaviour
{
    public PowerUpType powerUpType = new PowerUpType();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if it is the player
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            //Check power up to use
            switch (powerUpType)
            {
                case PowerUpType.doubleJump:
                    other.gameObject.GetComponent<PlayerController>().doubleJumpActive = true;
                    break;
            }

            //Disable powerup
            gameObject.SetActive(false);
        }
    }
}
