using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HutongGames.PlayMaker;

public class BuffsManager : MonoBehaviour
{
    private const int MaxAmountOfBait = 99;
    private const int BAIT_TIMER = 300;
    public FishDataManager fishDataManager;
    public GameObject activeBaitGO;
    private Buff activeBait;
    private GameObject baitSpriteGO, baitTimerGO;
    private List<Bait> baitInventory;

    void Awake()
    {
        getActiveBaitChildrenGO();
        LoadCurrBaitFromJSON();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadInventoryFromJSON();
        updateActiveBaitGO();
        //setBait("Clownfish"); //TEST PURPOSES ONLY
        
    }

    public void LoadCurrBaitFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/activeBait.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            activeBait = JsonUtility.FromJson<Buff>(jsonData);
            activeBaitGO.SetActive(true);
            //Debug.Log("activeBait.json successfully read");
        } else
        {
            //Debug.Log("no activeBait saved, creating null");
            resetActiveBait();
        }
    }

    public void resetActiveBait() {
        activeBait = null;
        SaveActiveBaitToJSON();
    }
    
    public void SaveActiveBaitToJSON()
    {
        string jsonData = JsonUtility.ToJson(activeBait);
        string filePath = Application.persistentDataPath + "/activeBait.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    private void LoadInventoryFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/baitInventory.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            baitInventory = JsonUtility.FromJson<BaitInventory>(jsonData).baitInventory;
            //Debug.Log("baitInventory.json successfully read");
        } else
        {
            //Debug.Log("no player inventory saved, creating empty inventory");
            baitInventory = new List<Bait>();
            SaveInventoryToJSON();
        }
    }

    public void SaveInventoryToJSON()
    {
        string jsonData = JsonUtility.ToJson(new BaitInventory(baitInventory));
        string filePath = Application.persistentDataPath + "/baitInventory.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    ////////////////////////BUFF INVENTORY MANAGER////////////////////

    public void addOneBaitToInventory(string fishName) {
        addBaitToInventory(fishName, 1);
        SaveInventoryToJSON();
    }

    public void addBaitToInventory(string fishName, int amount) {
        Bait alreadyExists = baitInventory.Find(bait => bait.fishName == fishName);
        if (alreadyExists != null)
        {
            alreadyExists.amountHeld += amount;
        } else {
            baitInventory.Add(new Bait(fishName, amount));
        }
        SaveInventoryToJSON();
    }

    public int getBaitCount() {
        return baitInventory.Count;
    }

    public string getBaitFishName(int index) {
        return baitInventory[index].fishName;
    }

    public int getAmountHeld(int index) {
        return baitInventory[index].amountHeld;
    }

    public int getAmountHeldByName(string _fishName) {
        int index = baitInventory.FindIndex(bait => bait.fishName == _fishName);
        if (index == -1) {
            Debug.Log("no bait of: " + _fishName + " in buff inventory");
            return -1;
        }
        return getAmountHeld(index);
    }

    public int getMaxAmountBait() {
        return MaxAmountOfBait;
    }

    public void removeBaitFromInventory(string _fishName, int amount)
    {
        int index = baitInventory.FindIndex(bait => bait.fishName == _fishName);
        if (index > -1) { //exists
            if (baitInventory[index].amountHeld >= amount) {
                baitInventory[index].amountHeld -= amount;
                if (baitInventory[index].amountHeld == 0) {
                    baitInventory.RemoveAt(index);
                }
                SaveInventoryToJSON();
            } else {
                Debug.Log("not enough " + _fishName + " bait to remove");
            }
        }
    }
    ///////////////////////////ACTIVE BUFF MANAGER///////////////////////////////

    private void getActiveBaitChildrenGO() {
        baitSpriteGO = activeBaitGO.gameObject.transform.GetChild(0).gameObject;
        baitTimerGO = activeBaitGO.gameObject.transform.GetChild(1).gameObject;
        
    }
    
    public string getCurrBaitName() {
        if (activeBait != null) {
            return activeBait.buffName;
        } else {
            //Debug.Log("no active bait");
            return "";
        }
    }

    public int getCurrBaitTimer() {
        if (activeBait != null) {
            return activeBait.timer;
        } else {
            //Debug.Log("no active bait");
            return BAIT_TIMER;
        }
    }

    public bool isBaitActive() {
        return (activeBait != null);
    }

    public void setBaitTimer(int seconds) {
        if (isBaitActive()) {
            activeBait.timer = seconds;
            SaveActiveBaitToJSON();
        }
    }

    public void setBait(string fishName) { //CANT SET SAME BAIT AS CURR ACTIVE BAIT
        if (isBaitActive() && (getCurrBaitName() == fishName)) {
            //Debug.Log("cant set the same bait: " + fishName); //maybe extend timer?
            return;
        }
        activeBait = new Buff(fishName, BAIT_TIMER);
        removeBaitFromInventory(fishName, 1);
        SaveActiveBaitToJSON();
        updateActiveBaitGO();
        activeBaitGO.SetActive(true);
        tellFSMStartTimer();
    }

    public void removeActiveBait() {
        if (isBaitActive()) {
            activeBait = null;
            activeBaitGO.SetActive(false);
            SaveActiveBaitToJSON();
        }
    }

    private void tellFSMStartTimer() {
        this.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("baitTimer").Value = getCurrBaitTimer();
        this.GetComponent<PlayMakerFSM>().SendEvent("COUNTDOWN");
    }

    private void updateActiveBaitGO() {
        if (isBaitActive()) {
            updateBaitSpriteGO();
            activeBaitGO.SetActive(true);
            updateActiveBaitTimerGO();
            tellFSMStartTimer();
        } else {
            activeBaitGO.SetActive(false);
        }
    }

    private void updateBaitSpriteGO() {
        baitSpriteGO.GetComponent<Image>().sprite = fishDataManager.getFishSpriteByName(getCurrBaitName());
    }

    private void updateActiveBaitTimerGO() {
        string timerString = TimeSpan.FromSeconds(getCurrBaitTimer()).ToString("mm\\:ss"); //cuts off hour part
        baitTimerGO.GetComponent<TMP_Text>().text = timerString;
    }


}

[System.Serializable]
public class Bait
{
    public string fishName;
    public int amountHeld;

    public Bait(string _fishName, int _amountHeld)
    {
        fishName = _fishName;
        amountHeld = _amountHeld;
    }
}

[System.Serializable]
public class BaitInventory
{
    public List<Bait> baitInventory;

    public BaitInventory(List<Bait> _baitInventory)
    {
        baitInventory = _baitInventory;
    }
}

[System.Serializable]
public class Buff
{
    public string buffName;
    public int timer;
    public Buff(string _buffName, int _timer)
    {
        buffName = _buffName;
        timer = _timer;
    }
}