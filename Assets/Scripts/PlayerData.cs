using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins;
    public float energy;
    //public float energyTimer; //???
    public float experience;
    public int[] invIdArray;
    public int[] invQuantityArray;
    public int level;
    public int[] specialCustomerRep; //0=little boy, 1=bride, 2= boxer, 3=ballerina, 4=samurai

    public PlayerData(Player player)
    {
        coins = player.coinsSO.Value;
        energy = player.energySO.Value;
        //energyTimer = player.energyTimerSO.Value;
        experience = player.experienceSO.Value;
        invIdArray = player.invIdArraySO.Value;
        invQuantityArray = player.invQuantityArraySO.Value;
        level = player.levelSO.Value;
        for (int i = 0; i < player.customerSOArray.Length; i++)
        {
            //get player.customerSOArray[i].rep and store in specialCustomerRep[i]
        }
    }
}
