using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ShopItem> buffsShop;
    public List<ShopItem> upgradesShop;
    public List<SavedShopSlot> savedUpgrades; //same size as upgradesShop
    public List<ShopItem> cosmeticsShop;
    public List<SavedShopSlot> savedCosmetics; //same size as cosmeticsShop
    
    // Start is called before the first frame update
    void Start()
    {
        LoadBuffsShop();
        LoadUpgradesShop();
        LoadCosmeticsShop();   
        LoadSavedShopsFromJSON();
    }

    public void LoadBuffsShop()
    {
        string jsonFilePath = "Assets/Resources/Data/buffsShop.json";
        string jsonContent = File.ReadAllText(jsonFilePath);
        buffsShop = JsonUtility.FromJson<ShopWrapper>(jsonContent).items;
        Debug.Log("buffsShop successfully loaded");
    }

    public void LoadUpgradesShop()
    {
        string jsonFilePath = "Assets/Resources/Data/upgradesShop.json";
        string jsonContent = File.ReadAllText(jsonFilePath);
        upgradesShop = JsonUtility.FromJson<ShopWrapper>(jsonContent).items;
        Debug.Log("upgradesShop successfully loaded");
    }

    public void LoadCosmeticsShop()
    {
        string jsonFilePath = "Assets/Resources/Data/cosmeticsShop.json";
        string jsonContent = File.ReadAllText(jsonFilePath);
        cosmeticsShop = JsonUtility.FromJson<ShopWrapper>(jsonContent).items;
        Debug.Log("cosmeticsShop successfully loaded");
    }

    public void LoadSavedShopsFromJSON()
    {
        // Load the JSON data from the persistent data path
        //savedUpgrades
        string filePath = Application.persistentDataPath + "/savedUpgrades.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            savedUpgrades = JsonUtility.FromJson<SavedSlotWrapper>(jsonData).savedSlots;
            Debug.Log("savedUpgrades.json successfully read");
        } else
        {
            Debug.Log("no savedUpgrades saved, creating new savedUpgrades");
            savedUpgrades = new List<SavedShopSlot>(upgradesShop.Count);
            foreach (ShopItem item in upgradesShop)
            {
                savedUpgrades.Add(new SavedShopSlot(item.name));
            }
        }
        //savedCosmetics 
        filePath = Application.persistentDataPath + "/savedCosmetics.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            savedCosmetics = JsonUtility.FromJson<SavedSlotWrapper>(jsonData).savedSlots;
            Debug.Log("savedCosmetics.json successfully read");
        } else
        {
            Debug.Log("no savedCosmetics saved, creating new savedCosmetics");
            savedCosmetics = new List<SavedShopSlot>(cosmeticsShop.Count);
            foreach (ShopItem item in cosmeticsShop)
            {
                savedCosmetics.Add(new SavedShopSlot(item.name));
            }
        }   
    }

    public void SaveShopsToJSON()
    {
        string jsonData = JsonUtility.ToJson(new SavedSlotWrapper(savedUpgrades));
        string filePath = Application.persistentDataPath + "/savedUpgrades.json";
        System.IO.File.WriteAllText(filePath, jsonData);

        jsonData = JsonUtility.ToJson(new SavedSlotWrapper(savedCosmetics));
        filePath = Application.persistentDataPath + "/savedCosmetics.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    //buff shop methods
    public int getBuffItemIndex(string _itemName)
    {
        return buffsShop.FindIndex(item => item.name == _itemName);
    }

    public string getBuffItemName(int _itemIndex)
    {
        return buffsShop[_itemIndex].name;
    }

    public string getBuffItemDescription(int _itemIndex)
    {
        return buffsShop[_itemIndex].description;
    }

    public int getBuffItemID(int _itemIndex)
    {
        return buffsShop[_itemIndex].itemID;
    }

    public int getBuffItemPrice(int _itemIndex)
    {
        return buffsShop[_itemIndex].price;
    }

    public int getBuffItemLevelUnlocked(int _itemIndex)
    {
        return buffsShop[_itemIndex].levelUnlocked;
    }

    public string getBuffItemEffectName(int _itemIndex)
    {
        return buffsShop[_itemIndex].effectName;
    }

    //upgrade shop methods, savedSlot methods
    public int getUpgradeItemIndex(string _itemName)
    {
        return upgradesShop.FindIndex(item => item.name == _itemName);
    }

    public string getUpgradeItemName(int _itemIndex)
    {
        return upgradesShop[_itemIndex].name;
    }

    public string getUpgradeItemDescription(int _itemIndex)
    {
        return upgradesShop[_itemIndex].description;
    }

    public int getUpgradeItemID(int _itemIndex)
    {
        return upgradesShop[_itemIndex].itemID;
    }

    public int getUpgradeItemPrice(int _itemIndex)
    {
        return upgradesShop[_itemIndex].price;
    }

    public int getUpgradeItemLevelUnlocked(int _itemIndex)
    {
        return upgradesShop[_itemIndex].levelUnlocked;
    }

    public string getUpgradeItemEffectName(int _itemIndex)
    {
        return upgradesShop[_itemIndex].effectName;
    }

    public bool getUpgradeIsUnlocked(string _itemName)
    {
        return savedUpgrades[getUpgradeItemIndex(_itemName)].isUnlocked;
    }

    public void setUpgradeIsUnlocked(string _itemName, bool _bool)
    {
        savedUpgrades[getUpgradeItemIndex(_itemName)].isUnlocked = _bool;
    }
    
    //cosmetics shop methods, savedSlot methods
    public int getCosmeticItemIndex(string _itemName)
    {
        return cosmeticsShop.FindIndex(item => item.name == _itemName);
    }

    public string getCosmeticItemName(int _itemIndex)
    {
        return cosmeticsShop[_itemIndex].name;
    }

    public string getCosmeticItemDescription(int _itemIndex)
    {
        return cosmeticsShop[_itemIndex].description;
    }

    public int getCosmeticItemID(int _itemIndex)
    {
        return cosmeticsShop[_itemIndex].itemID;
    }

    public int getCosmeticItemPrice(int _itemIndex)
    {
        return cosmeticsShop[_itemIndex].price;
    }

    public int getCosmeticItemLevelUnlocked(int _itemIndex)
    {
        return cosmeticsShop[_itemIndex].levelUnlocked;
    }

    public string getCosmeticItemEffectName(int _itemIndex)
    {
        return cosmeticsShop[_itemIndex].effectName;
    }

    public bool getCosmeticIsUnlocked(string _itemName)
    {
        return savedCosmetics[getCosmeticItemIndex(_itemName)].isUnlocked;
    }

    public void setCosmeticIsUnlocked(string _itemName, bool _bool)
    {
        savedCosmetics[getCosmeticItemIndex(_itemName)].isUnlocked = _bool;
    }

    public void makeBuffPurchase(string name)
    {
        //TODO
        // tell effects to do smth
        //decrease money in player
    }

    public void makeUpgradePurchase(string name)
    {
        //TODO
        //tell effects to do smth
        //decrease money in player if not already bought
        //savedUpgrades set to bought
    }

    public void makeCosmeticPurchase(string name)
    {
        //TODO
        //tell effects to do smth
        //decrease money in player if not alrdy bought
        //savedCosmetics set to bought
    }
}

[System.Serializable]
public class ShopItem
{
    public string name;
    public string description;
    public int itemID;
    public int price;
    public int levelUnlocked;
    public string effectName;
}

[System.Serializable]
public class ShopWrapper
{
    public List<ShopItem> items;
}

[System.Serializable]
public class SavedShopSlot
{
    public string itemName;
    public bool isUnlocked;

    public SavedShopSlot(string _itemName)
    {
        itemName = _itemName;
        isUnlocked = false;
    }
}

[System.Serializable]
public class SavedSlotWrapper
{
    public List<SavedShopSlot> savedSlots;

    public SavedSlotWrapper(List<SavedShopSlot> _savedSlots)
    {
        savedSlots = _savedSlots;
    }
}