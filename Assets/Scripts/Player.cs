using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public IntSO coinsSO;
    public FloatSO energySO;
    public FloatSO energyTimerSO;
    public FloatSO experienceSO;
    public ArrayIntSO invIdArraySO;
    public ArrayIntSO invQuantityArraySO;
    public IntSO levelSO;
    public CustomerObject[] customerSOArray = new CustomerObject[5];

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        coinsSO.Value = data.coins;
        energySO.Value = data.energy;
        energyTimerSO.Value = data.energyTimer;
        experienceSO.Value = data.experience;
        invIdArraySO.Value = data.invIdArray;
        invQuantityArraySO.Value = data.invQuantityArray;
        levelSO.Value = data.level;
        for (int i = 0; i < customerSOArray.Length; i++)
        {
            customerSOArray[i].repCurr = 0;//data.specialCustomerRep[i];
        }
    }
}
