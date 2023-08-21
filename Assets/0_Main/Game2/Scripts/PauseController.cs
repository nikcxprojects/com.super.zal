using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public bool startPause;

    private void Start()
    {
        if(startPause) Pause(true);
    }
    
    public static void Pause(bool value)
    {
        Time.timeScale = value ? 0 : 1;
    }

    public void Continue()
    {
        Time.timeScale = 1;
    }
}
