using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public void PauseGame()
    {
        gameObject.GetComponent<Canvas>().enabled = !gameObject.GetComponent<Canvas>().enabled;
        Time.timeScale = gameObject.GetComponent<Canvas>().enabled ? 0 : 1;
    }
}
