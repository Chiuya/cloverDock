using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    private const float SceneLoadTime = 4.0f;
    private const float EnergyToAdd = 15f;
    private Energy energy;
    private DateTime currentTime = DateTime.UtcNow;
    private DateTime lastTimeSaved;
    // Start is called before the first frame update
    void Awake()
    {
        LoadEnergyFromJSON();
        loadPreviousTime();
        //Debug.Log("max amt: " + energy.maxAmount);
        //Debug.Log("curr amt: " + energy.currentAmount);
    }

    void Start() {
        //resetMaxTimer(); //TESTING
    }

    public void LoadEnergyFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/energy.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            energy = JsonUtility.FromJson<Energy>(jsonData);
            //Debug.Log("energy.json successfully read");
        } else
        {
            //Debug.Log("no energy data saved, creating new energy data");
            energy = new Energy(EnergyConstants.DEFAULTAMOUNT, EnergyConstants.DEFAULTTIMER);
            SaveEnergyToJSON();
        }
        //loadPreviousTime();
    }

    public void SaveEnergyToJSON()
    {
        string jsonData = JsonUtility.ToJson(energy);
        string filePath = Application.persistentDataPath + "/energy.json";
        System.IO.File.WriteAllText(filePath, jsonData);
        saveCurrTime();
    }

    public float getCurrentAmount()
    {
        return energy.currentAmount;
    }

    public void setCurrAmount(float amount)
    {
        energy.currentAmount = amount;
        SaveEnergyToJSON();
    }

    public void addOnceAmount() {
        if (!isFullEnergy()) {
            energy.currentAmount += EnergyToAdd;
            if ((isFullEnergy()) && (energy.currentAmount <= (getMaxAmount() + EnergyToAdd))) {
                resetFullEnergy();
            }
            SaveEnergyToJSON();
        }
    }

    public float getMaxAmount()
    {
        return energy.maxAmount;
    }

    public void setMaxAmount(float amount)
    {
        energy.maxAmount = amount;
        SaveEnergyToJSON();

    }

    public float getCurrTimer()
    {
        return energy.currTimer;
    }

    public void setCurrTimer(float amount)
    {
        energy.currTimer = amount;
        //saveCurrTime();
        SaveEnergyToJSON();

    }

    public float getMaxTimer()
    {
        return energy.maxTimer;
    }

    public void setMaxTimer(float amount)
    {
        energy.maxTimer = amount;
        if (energy.currTimer > amount) {
            energy.currTimer = amount;
        }
        SaveEnergyToJSON();

    }

    public bool isFullEnergy() {
        return energy.currentAmount >= energy.maxAmount;
    }

    public void resetCurrTimer() {
        energy.currTimer = energy.maxTimer;
        SaveEnergyToJSON();
    }

    public void resetMaxTimer()
    {
        energy.maxTimer = EnergyConstants.DEFAULTTIMER;
        SaveEnergyToJSON();
        
    }

    public void resetMaxAmount()
    {
        energy.maxAmount = EnergyConstants.DEFAULTAMOUNT;
        SaveEnergyToJSON();
    }

    public void resetFullEnergy() {
        energy.currentAmount = energy.maxAmount;
        SaveEnergyToJSON();
    }
    ///////////////ENERGY/TIMER UPDATE WHILE APP IS CLOSED FUNCTIONS////////////

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) {
            //Debug.Log("focus");
            loadPreviousTime();
        } else {
            //Debug.Log("lose focus");
            saveCurrTime();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) {
            saveCurrTime();
        }
    }
    
    private void saveCurrTime() {
        PlayerPrefs.SetString("LastTime", DateTime.UtcNow.ToString());
        //Debug.Log("saving: " + DateTime.UtcNow.ToString());
    }

    private void loadPreviousTime() {
        if (!PlayerPrefs.HasKey("LastTime")) {
            PlayerPrefs.SetString("LastTime", DateTime.UtcNow.ToString());
        }
        DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString("LastTime"));
        currentTime = DateTime.UtcNow;
        TimeSpan differenceSpan = currentTime.Subtract(lastTime);
        float difference = (float) differenceSpan.TotalSeconds;
        if (difference > SceneLoadTime) {
            //Debug.Log("prev timer: " + energy.currTimer);
            //Debug.Log("prev energy: " + energy.currentAmount);
            lastTimeSaved = lastTime;
            saveCurrTime();
            //CONVERT DIFFERENCE TO ENERGY
            float toSetEnergy = energy.currentAmount + (float) (Math.Floor(difference / getMaxTimer())*EnergyToAdd);
            float toSetTimer = energy.currTimer - (float) Math.Floor(difference % getMaxTimer());
            if (!isFullEnergy()) {
                energy.currentAmount = toSetEnergy;
                if (energy.currentAmount > energy.maxAmount) {
                    resetFullEnergy();
                }
            }
            if (toSetTimer < 0.0f) {
                addOnceAmount();
                setCurrTimer(getMaxTimer() + toSetTimer);
            } else {
                setCurrTimer(toSetTimer);
            }
            //Debug.Log("curr timer: " + energy.currTimer);
            //Debug.Log("curr energy: " + energy.currentAmount);
            this.gameObject.GetComponent<PlayMakerFSM>().SendEvent("COUNTDOWN");
            //Debug.Log("loading: " + lastTimeSaved + " with difference: " + difference);
        }
    }
}

[System.Serializable]
public class Energy
{
    public float currentAmount;
    public float maxAmount;
    public float currTimer;
    public float maxTimer;

    public Energy(float _maxAmount, float _maxTimer)
    {
        currentAmount = _maxAmount;
        maxAmount = _maxAmount;
        currTimer = _maxTimer;
        maxTimer = _maxTimer;
    }
}

public class EnergyConstants
{
    public const float DEFAULTAMOUNT = 100.0f;
    public const float DEFAULTTIMER = 180.0f;
}