using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private const int MaxAmountHeld = 999;
    public FishDataManager fishDataManager;
    private List<ItemSlot> playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        LoadInventoryFromJSON();
        //TESTresetInv();
    }

    public void LoadInventoryFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/playerInventory.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            playerInventory = JsonUtility.FromJson<Inventory>(jsonData).items;
            //Debug.Log("playerInventory.json successfully read");
        } else
        {
            //Debug.Log("no player inventory saved, creating empty inventory");
            playerInventory = new List<ItemSlot>();
            SaveInventoryToJSON();
        }
        
    }

    public void SaveInventoryToJSON()
    {
        string jsonData = JsonUtility.ToJson(new Inventory(playerInventory));
        string filePath = Application.persistentDataPath + "/playerInventory.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public void TESTresetInv() { //FOR TEST PURPOSES ONLY
        playerInventory = new List<ItemSlot>();
        SaveInventoryToJSON();
    }

    public string getItemSlotName(int index)
    {
        return playerInventory[index].itemName;
    }

    public int getItemSlotIndex(string _itemName)
    {
        return playerInventory.FindIndex(slot => slot.itemName == _itemName);
    }

    public string getItemSlotDescription(int index)
    {
        return playerInventory[index].itemDescription;
    }

    public Sprite getItemSlotFishSprite(int index)
    {
        return Resources.Load<Sprite>("Fish/" + playerInventory[index].spritePath);
    }

    public int getItemSlotAmount(int index)
    {
        return playerInventory[index].amountHeld;
    }

    public int getInventoryLength()
    {
        return playerInventory.Count;
    }

    public void addFishByName(string fishName, int amount) {
        //Debug.Log("adding: " + fishName + amount);
        int i = fishDataManager.getFishIndex(fishName);
        ItemSlot toAdd = new ItemSlot(fishName, fishDataManager.getFishDescription(i), fishDataManager.getFishSpritePath(i), amount);
        ItemSlot alreadyExists = playerInventory.Find(slot => slot.itemName == toAdd.itemName);
        if (alreadyExists != null)
        {
            alreadyExists.amountHeld += amount;
            if (alreadyExists.amountHeld > MaxAmountHeld) {
                alreadyExists.amountHeld = MaxAmountHeld;
                Debug.Log("already holding max amount in inventory");
            }
        } else {
            playerInventory.Add(toAdd);
        }
        SaveInventoryToJSON();
    }

    public void addFishToInventory(Fish _fish, int amount)
    {
        addFishByName(_fish.fishName, amount);

        // ItemSlot toAdd = new ItemSlot(_fish.fishName, _fish.description, _fish.spriteName, amount);
        // ItemSlot alreadyExists = playerInventory.Find(slot => slot.itemName == toAdd.itemName);
        // if (alreadyExists != null)
        // {
        //     alreadyExists.amountHeld += amount;
        //     if (alreadyExists.amountHeld > MaxAmountHeld) {
        //         alreadyExists.amountHeld = MaxAmountHeld;
        //         Debug.Log("already holding max amount in inventory");
        //     }
        // } else {
        //     playerInventory.Add(toAdd);
        // }
        // SaveInventoryToJSON();
    }

    public void removeFishFromInventory(string fishName, int amount)
    {
        int index = playerInventory.FindIndex(slot => slot.itemName == fishName);
        if (index > -1) { //item exists
            if (playerInventory[index].amountHeld >= amount) {
                playerInventory[index].amountHeld -= amount;
                if (playerInventory[index].amountHeld == 0) {
                    playerInventory.RemoveAt(index);
                }
                SaveInventoryToJSON();
            }
        } else {
            Debug.Log("not enough " + fishName + " to remove from inventory");
        }
    }

}

[System.Serializable]
public class ItemSlot
{
    public string itemName;
    public string itemDescription;
    public string spritePath;
    public int amountHeld;

    public ItemSlot(string _itemName, string _itemDescription, string _spritePath, int _amountHeld)
    {
        itemName = _itemName;
        itemDescription = _itemDescription;
        spritePath = _spritePath;
        amountHeld = _amountHeld;
    }
}

[System.Serializable]
public class Inventory
{
    public List<ItemSlot> items;

    public Inventory(List<ItemSlot> _items)
    {
        items = _items;
    }
}
