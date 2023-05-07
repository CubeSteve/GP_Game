using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    private SplineContainer spline;
    public GameObject player;

    public GameObject playerCarrier;

    private void Start()
    {
        spline = GetComponent<SplineContainer>();
    }

    private void Update()
    {
        if (player.GetComponent<PlayerController>().onSpline)
        {
            if (playerCarrier.GetComponent<SplineAnimate>().elapsedTime < 0 || playerCarrier.GetComponent<SplineAnimate>().elapsedTime > 65)
            {
                //Reset player variables
                player.transform.position = GetComponentInParent<Transform>().position;
                player.transform.SetParent(null);

                //Reset spline animation
                playerCarrier.GetComponent<SplineAnimate>().Pause();
                playerCarrier.GetComponent<SplineAnimate>().elapsedTime = 0;

                player.GetComponent<PlayerController>().EndSpline();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!player.GetComponent<PlayerController>().onSpline)
            {
                playerCarrier.GetComponent<SplineAnimate>().Play();
                player.transform.SetParent(playerCarrier.transform);
                player.transform.localPosition = Vector3.zero;

                player.GetComponent<PlayerController>().StartSpline();
            }
        }
    }

    public void SetSplineSpeed(float speed)
    {
        playerCarrier.GetComponent<SplineAnimate>().elapsedTime += speed;
    }
}
