using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    doubleJump,
    speedBoost
}

public class PowerUpController : MonoBehaviour
{
    public PowerUpType powerUpType = new PowerUpType();

    // Start is called before the first frame update
    void Start()
    {
        //Check power up to use
        switch (powerUpType)
        {
            case PowerUpType.doubleJump:
                this.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case PowerUpType.speedBoost:
                this.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
        }
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
                case PowerUpType.speedBoost:
                    other.gameObject.GetComponent<PlayerController>().speed *= 2;
                    other.gameObject.GetComponent<PlayerController>().maxSpeed *= 2;

                    //"Run" animation
                    other.gameObject.GetComponent<Animator>().SetBool("isRunning", true);
                    break;
            }

            //Disable powerup
            gameObject.SetActive(false);
        }
    }
}
