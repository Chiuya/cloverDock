using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HutongGames.PlayMaker;

public class TankManager : MonoBehaviour
{
    private const int MaxAmountHeld = 99;
    public FishDataManager fishDataManager;
    public PlayerManager playerManager;
    public InventoryManager inventoryManager;
    private TankData tankData;
    private Sprite lockedTankSprite, emptyTankSprite, fullTankSprite;
    public GameObject[] tankObjectArray; //fishTank (child: fish (child: text))
    public GameObject unlockToolTipObj;
    public GameObject displayToolTipObj;
    // Start is called before the first frame update
    void Start()
    {
        lockedTankSprite = Resources.Load<Sprite>("Tanks/lockedTank");
        emptyTankSprite = Resources.Load<Sprite>("Tanks/emptyTank");
        fullTankSprite = Resources.Load<Sprite>("Tanks/fishTank");
        LoadTanksFromJSON();
        //resetTest(); //FOR TEST PURPOSES ONLY
    }

    private void LoadTanksFromJSON() {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/tankData.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            tankData = JsonUtility.FromJson<TankData>(jsonData);
            //Debug.Log("tankdata.json successfully read");
            for (int i = 0; i < 5; i++) {
                initTankListener(i);
            }
        } else
        {
            //Debug.Log("no tankdata saved, creating new tankdata");
            resetTankData();
        }
        for (int i = 0; i < 5; i++) {
            updateTankObject(i);
        }
    }

    public void resetTest() {
        //FOR TEST PURPOSES ONLY
        resetTankData();
        for (int i = 0; i < 5; i++) {
            updateTankObject(i);
        }
        SaveTanksToJSON();
    }

    private void resetTankData() {
        Tank[] newTankArray = new Tank[]
            {
                new Tank(true, "", 0),
                new Tank(true, "", 0),
                new Tank(true, "", 0),
                new Tank(true, "", 0),
                new Tank(true, "", 0),
            };
        tankData = new TankData(newTankArray);
        for (int i = 0; i < 5; i++) {
            tankObjectArray[i].gameObject.GetComponent<Button>().onClick.AddListener(unlockToolTip);
        }
    }

    private void initTankListener(int index) {
        //Debug.Log("1");
        if (getIsLocked(index)) {
            //Debug.Log("2");
            //tankObjectArray[index].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            tankObjectArray[index].gameObject.GetComponent<Button>().onClick.AddListener(unlockToolTip);
        } else {
            tankObjectArray[index].gameObject.GetComponent<Button>().onClick.AddListener(() => tankToolTip(index));
        }
    }

    private void SaveTanksToJSON() {
        string jsonData = JsonUtility.ToJson(tankData);
        string filePath = Application.persistentDataPath + "/tankData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public int getIndexByName(string _fishName) {
        return Array.FindIndex(tankData.tanks, tank => tank.fishName == _fishName);
    }

    public bool getIsLocked(int tankIndex) {
        return tankData.tanks[tankIndex].isLocked;
    }

    public string getFishName(int tankIndex) {
        //Debug.Log("fish name: " + tankData.tanks[tankIndex].fishName);
        return tankData.tanks[tankIndex].fishName;
    }

    public int getAmountHeld(int tankIndex) {
        return tankData.tanks[tankIndex].amountHeld;
    }

    public void setIsLocked(int tankIndex, bool _isLocked) {
        if (tankIndex < 5 && tankIndex >= 0) {
            tankData.tanks[tankIndex].isLocked = _isLocked;
            updateTankObject(tankIndex);
            SaveTanksToJSON();
        } else {
            Debug.Log("tankIndex not valid");
        }
    }

    private void setFishName(int tankIndex, string _fishName) {
        if (tankIndex < 5 && tankIndex >= 0) {
            tankData.tanks[tankIndex].fishName = _fishName;
            updateTankObject(tankIndex);
            SaveTanksToJSON();
        } else {
            Debug.Log("tankIndex not valid");
        }
    }

    private void setAmountHeld(int tankIndex, int _amount) {
        if (tankIndex < 5 && tankIndex >= 0) {
            tankData.tanks[tankIndex].amountHeld = _amount;
            updateTankObject(tankIndex);
            SaveTanksToJSON();
        } else {
            Debug.Log("tankIndex not valid");
        }
    }

    public void addAmountHeld(int tankIndex, int amountToAdd) {
        if (tankIndex < 5 && tankIndex >= 0) {
            tankData.tanks[tankIndex].amountHeld += amountToAdd;
            updateTankObject(tankIndex);
            SaveTanksToJSON();
        } else {
            Debug.Log("tankIndex not valid");
        }
    }

    public bool sellOneFish(string fishName) {
        int index = getIndexByName(fishName);
        if (index >= 0) {
            //Debug.Log("sell fish index valid");
            int currAmt = getAmountHeld(index);
            if (currAmt > 0) {
                setAmountHeld(index, currAmt - 1);
                if (getAmountHeld(index) == 0) {
                    setFishName(index, "");
                } else {
                    updateTankObject(index);
                    SaveTanksToJSON();
                }
                
                //int value = fishDataManager.getFishValueByName(fishName);
                float value = (float) Math.Floor((fishDataManager.getFishValueByName(fishName) / 2.0f));
                int price = fishDataManager.getFishPrice(fishName);
                playerManager.addCoins(price);
                playerManager.addExperience(value);
                //Debug.Log("fish sale success: " + fishName);
                return true;
            }
        }
        //Debug.Log("cant sell this fish: " + fishName);
        return false;
    }

    public void replaceTank(int tankIndex, string fishName, int amount) {
        if (tankIndex < 5 && tankIndex >= 0) {
            setFishName(tankIndex, fishName);
            setAmountHeld(tankIndex, amount);
            updateTankObject(tankIndex);
            SaveTanksToJSON();
        } else {
            Debug.Log("tankIndex not valid");
        }
    }
    private void updateTankObject(int tankIndex) {
        //Debug.Log("update go");
        if (getIsLocked(tankIndex)) {
            tankObjectArray[tankIndex].gameObject.GetComponent<Image>().sprite = lockedTankSprite;
            tankObjectArray[tankIndex].transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        int fishHeld = getAmountHeld(tankIndex);
        if (fishHeld > 0) {
            tankObjectArray[tankIndex].transform.GetChild(0).gameObject.SetActive(true);
            string fishName = getFishName(tankIndex);
            tankObjectArray[tankIndex].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = fishDataManager.getFishSpriteByName(fishName);
            tankObjectArray[tankIndex].transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController = fishDataManager.getAnimator(fishName);
            tankObjectArray[tankIndex].transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text = getAmountHeld(tankIndex).ToString();
            tankObjectArray[tankIndex].gameObject.GetComponent<Image>().sprite = fullTankSprite;
        } else {
            tankObjectArray[tankIndex].transform.GetChild(0).gameObject.SetActive(false);
            tankObjectArray[tankIndex].gameObject.GetComponent<Image>().sprite = emptyTankSprite;
        }
    }

    public void unlockToolTip() {
        //Debug.Log("1");
        int tankCost = 50;
        for (int i = 1; i < 5; i++) {
            if (getIsLocked(i)) {
                //Debug.Log("locked tank: " + i);
                break;
            } else { //what the hell is this pricing idk
                //Debug.Log("price adjustment " + i);
                if (i == 1) {
                    //Debug.Log("tankCost i at 2 = " + tankCost);
                    tankCost = 500;
                } else if (i == 2) {
                    //Debug.Log("tankCost i at 3 = " + tankCost);
                    tankCost = 1000;
                } else if (i == 3) {
                    //Debug.Log("tankCost i at 4 = " + tankCost);
                    tankCost = 8000;
                }
            }
        }
        if (getIsLocked(0)) {
            tankCost = 0;
        }
        //Debug.Log("tankCost = " + tankCost);
        unlockToolTipObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("tankCost").Value = tankCost;
        unlockToolTipObj.GetComponent<PlayMakerFSM>().SendEvent("OPEN");
    }

    public void unlockTank() { //PREREQ: UNLOCK OF NEXT TANK SUCCESSFUL, unlock next available tank
        //Debug.Log("1");
        int i = Array.FindIndex(tankData.tanks, tank => tank.isLocked);
        tankObjectArray[i].gameObject.GetComponent<Button>().onClick.RemoveListener(unlockToolTip);
        tankObjectArray[i].gameObject.GetComponent<Button>().onClick.AddListener(() => tankToolTip(i));
        setIsLocked(i, false);
        setAmountHeld(i, 0);
        //updateTankObject(i);
    }

    public void tankToolTip(int tankIndex) {
        //Debug.Log("5");
        displayToolTipObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("occupiedFishID").Value = fishDataManager.getFishIndex(getFishName(tankIndex));
        displayToolTipObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("currTankQuantity").Value = getAmountHeld(tankIndex);
        displayToolTipObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("tankIndex").Value = tankIndex;
        //Debug.Log("tank index for tooltip: " + tankIndex);
        displayToolTipObj.GetComponent<PlayMakerFSM>().SendEvent("OPEN");
    }

    public void replaceStockTank(int quantity, int tankIndex, int fishID) { //replace existing fish with new fish
        //Debug.Log("replace");
        quantity = Math.Min(MaxAmountHeld, quantity);
        string originalName = getFishName(tankIndex);
        string newFishName = fishDataManager.getFishName(fishID);
        int existingIndex = Array.FindIndex(tankData.tanks, (tank => tank.fishName == newFishName)); //check if tank already exists
        if (existingIndex > -1) {
            if (getAmountHeld(existingIndex) > 0) {
                stockTank(quantity, existingIndex);
                //Debug.Log("stocked currently existing tank instead");
                return;
            }
        }
        int originalAmount = getAmountHeld(tankIndex);
        inventoryManager.addFishByName(originalName, originalAmount);
        inventoryManager.removeFishFromInventory(newFishName, quantity);
        replaceTank(tankIndex, newFishName, quantity);
    }

    public void stockTank(int quantity, int tankIndex) { //stock existing fish with same fish
        //Debug.Log("same stock");
        quantity = Math.Min(MaxAmountHeld, quantity);
        string currFishName = getFishName(tankIndex);
        inventoryManager.removeFishFromInventory(currFishName, quantity);
        addAmountHeld(tankIndex, quantity);
    }

    public void stockNewTank(int quantity, int tankIndex, int fishID) { //stock empty tank with new fish
        //Debug.Log("new tank stock");
        quantity = Math.Min(MaxAmountHeld, quantity);
        string fishName = fishDataManager.getFishName(fishID);
        int existingIndex = Array.FindIndex(tankData.tanks, (tank => tank.fishName == fishName)); //check if tank already exists
        if (existingIndex > -1) {
            if ((getAmountHeld(existingIndex) + quantity) > MaxAmountHeld) { //reduce quantity
                quantity = MaxAmountHeld - getAmountHeld(existingIndex);
            }
            stockTank(quantity, existingIndex);
            //Debug.Log("stocked currently existing tank instead");
            return;
        }
        if (tankIndex < 5 && tankIndex >= 0) {
            inventoryManager.removeFishFromInventory(fishName, quantity);
            tankData.tanks[tankIndex].fishName = fishName;
            tankData.tanks[tankIndex].amountHeld = quantity;
            updateTankObject(tankIndex);
            SaveTanksToJSON();
        } else {
            //Debug.Log("tankIndex not valid");
        }
    }

}

[System.Serializable]
public class TankData
{
    public Tank[] tanks; //5

    public TankData(Tank[] tankArray)
    {
        tanks = tankArray;
    }
}
[System.Serializable]
public class Tank {
    public bool isLocked;
    public string fishName;
    public int amountHeld;
    public Tank(bool _isLocked, string _fishName, int _amountHeld) {
        isLocked = _isLocked;
        fishName = _fishName;
        amountHeld = _amountHeld;
    }
}
