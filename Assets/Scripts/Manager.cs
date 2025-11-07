using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float timeScale = 1.0f;
    private float lastTimeScale;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeScaler();
    }

    private void TimeScaler()
    {
        Time.timeScale = timeScale;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timeScale == 0)
            {
                timeScale = lastTimeScale;
                Debug.Log("Resume Time");
            }
            else
            {
                lastTimeScale = timeScale;
                timeScale = 0;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeScale = 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            timeScale = 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            timeScale = 1;
        }
    }
}
