using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //calculate and return exp required to level up from input level
    //assume level >= 1
    public int maxExp(int level) 
    {
        if (level == 1) 
        {
            Debug.Log("Need 50 exp");
            return 50;
        } 
        else 
        {
            //runescape formula
            int firstPass = (int)Mathf.Floor(level + (300.0f * Mathf.Pow(2.0f, level / 7.0f)));
            Debug.Log("Need " + firstPass / 4 + " exp");
            return firstPass / 4;
        }
    }
}
