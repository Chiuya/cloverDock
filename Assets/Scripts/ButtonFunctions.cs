using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField]
    GameObject infanticide;

    public void destroyChildren() 
    {
        while (transform.childCount > 0) 
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void resumeTime()
    {
        Time.timeScale = 1.0f;
    }

    public void stopTime()
    {
        Time.timeScale = 0.0f;
    }
    
    public void toggleTime()
    {
        //Time.timeScale == 1.0f? Time.timeScale = 0.0f : Time.timeScale = 1.0f;
        if (Time.timeScale == 1.0f)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
        
    }
}
