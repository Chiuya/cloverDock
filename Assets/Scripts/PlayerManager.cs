using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerData playerData;
    public GameObject levelSliderGO;
    public SpriteRenderer outfitSprite;
    //public SpriteRenderer petSprite;

    void Awake()
    {
        LoadPlayerDataFromJSON();
    }

    void Start()
    {
        //activateOutfitEffect(playerData.outfitSpritePath);
        //activatePetEffect(playerData.petSpritePath);
        //setIsClockUnlocked(false); //TEST PURPOSES
        //setCoins(50000); //TEST PURPOSES
        //playerData = new PlayerData(0, 0.0f); //TESTING
        //SavePlayerDataToJSON(); //TESTING
        //setLevel(1);
        //addExperience(5000.0f);
    }

    public void LoadPlayerDataFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/playerData.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            //Debug.Log("playerData.json successfully read");
        } else
        {
            //Debug.Log("no player data saved, creating new player data");
            playerData = new PlayerData(0);
            SavePlayerDataToJSON();
        }        
    }

    public void SavePlayerDataToJSON()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        string filePath = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public int getCoins()
    {
        return playerData.coins;
    }

    public void setCoins(int _coins)
    {
        playerData.coins = _coins;
        SavePlayerDataToJSON();
    }

    public bool minusCoins(int amount) {
        if (amount <= playerData.coins) {
            playerData.coins -= amount;
            SavePlayerDataToJSON();
            return true;
        } else {
            Debug.Log("not enough coins!");
            return false;
        }
    }

    public void addCoins(int _coins)
    {
        playerData.coins += _coins;
        SavePlayerDataToJSON();
    }

    public float getExperience()
    {
        return playerData.experience;
    }

    public void setExperience(float _experience)
    {
        playerData.experience = _experience;
        updateLevelSliderGO();
        SavePlayerDataToJSON();
    }

    public void addExperience(float _experience)
    {
        playerData.experience += _experience;
        levelUpCheck();
        updateLevelSliderGO();
        SavePlayerDataToJSON();
    }

    private void levelUpCheck()
    {
        if (playerData.experience >= playerData.maxExperience)
        {
            playerData.experience -= playerData.maxExperience;
            playerData.level += 1;
            playerData.maxExperience = getMaxExperience(playerData.level);
            Debug.Log("level up!");
            //tell FSM to show level up notif
            this.GetComponent<PlayMakerFSM>().SendEvent("LEVEL UP");
        }
    }

    private float getMaxExperience(int level)
    {
        int maxExp = 500*level + 2500;
        return (float) maxExp;
    }

    public float getMaxExperience()
    {
        return playerData.maxExperience;
    }

    public void setMaxExperience(float _maxExperience)
    {
        playerData.maxExperience = _maxExperience;
        updateLevelSliderGO();
        SavePlayerDataToJSON();
    }

    public int getLevel()
    {
        return playerData.level;
    }

    public void setLevel(int _level)
    {
        playerData.level = _level;
        SavePlayerDataToJSON();
    }

    public bool getIsClockUnlocked() {
        return playerData.isClockUnlocked;
    }

    public void setIsClockUnlocked(bool _isUnlocked) {
        playerData.isClockUnlocked = _isUnlocked;
        SavePlayerDataToJSON();
    }

    public Sprite getOutfitSprite()
    {
        if (playerData.outfitSpritePath == "")
        {
            Debug.Log("no outfit equipped");
            return null;
        } else {
            string spritePath = "Assets/Sprites/Cosmetics/" + playerData.outfitSpritePath + ".png";
            return Resources.Load<Sprite>(spritePath);
        }   
    }

    private void setOutfitSprite(string _outfitSpritePath)
    {
        playerData.outfitSpritePath = _outfitSpritePath;
        SavePlayerDataToJSON();
    }

    public void activateOutfitEffect(string _outfitSpritePath)
    {
        setOutfitSprite(_outfitSpritePath);
        outfitSprite.sprite = getOutfitSprite();
    }

    private void updateLevelSliderGO() { //send EXP ADDED event to FSM "Level System FSM"
        if (levelSliderGO == null) {
            Debug.Log("level slider not assigned, can't update");
            return;
        } else {
            levelSliderGO.GetComponent<PlayMakerFSM>().SendEvent("EXP ADDED");
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public int coins;
    public float experience;
    public float maxExperience;
    public int level;
    public bool isClockUnlocked;
    public string outfitSpritePath; //cosmetic outfit sprite
    //public string petSpritePath; //cosmetic pet sprite

    public PlayerData(int _coins)
    {
        coins = _coins;
        experience = 0;
        maxExperience = 3000f;
        level = 1;
        isClockUnlocked = false;
        outfitSpritePath = "";
        //petSpritePath = "";
    }
}