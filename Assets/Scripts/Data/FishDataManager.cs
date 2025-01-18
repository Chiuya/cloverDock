using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FishDataManager : MonoBehaviour
{
    private Fish[] fishData;
    private Sprite clownfishSprite, pufferfishSprite, shrimpSprite, squidSprite, tunaSprite, goldfishSprite, seahorseSprite;
    private RuntimeAnimatorController clownfishAnimator, pufferfishAnimator, shrimpAnimator, squidAnimator, tunaAnimator, goldfishAnimator, seahorseAnimator;

    void Awake()
    {
        string jsonFilePath = "Assets/Resources/Data/fishData.json";
        string jsonContent = File.ReadAllText(jsonFilePath);
        fishData = JsonUtility.FromJson<FishData>(jsonContent).fishData;
        //Debug.Log("fishData successfully loaded");
        initFishSprites();
        initAnimators();
    }

    private void initFishSprites() {
        clownfishSprite = Resources.Load<Sprite>("Fish/Clownfish");
        pufferfishSprite = Resources.Load<Sprite>("Fish/Pufferfish");
        shrimpSprite = Resources.Load<Sprite>("Fish/Shrimp");
        squidSprite = Resources.Load<Sprite>("Fish/Squid");
        tunaSprite = Resources.Load<Sprite>("Fish/Tuna");
        goldfishSprite = Resources.Load<Sprite>("Fish/Goldfish");
        seahorseSprite = Resources.Load<Sprite>("Fish/Seahorse");
    }
    private void initAnimators() {
        clownfishAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Clownfish");
        pufferfishAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Pufferfish");
        shrimpAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Shrimp");
        squidAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Squid");
        tunaAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Tuna");
        goldfishAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Goldfish");
        seahorseAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Seahorse");
    }

    public RuntimeAnimatorController getAnimator(string name) {
        RuntimeAnimatorController ans;
        switch(name) 
        {
        case "Clownfish":
            ans = clownfishAnimator;
            break;
        case "Pufferfish":
            ans = pufferfishAnimator;
            break;
        case "Shrimp":
            ans = shrimpAnimator;
            break;
        case "Squid":
            ans = squidAnimator;
            break;
        case "Tuna":
            ans = tunaAnimator;
            break;
        case "Goldfish":
            ans = goldfishAnimator;
            break;
        case "Seahorse":
            ans = seahorseAnimator;
            break;
        default:
            ans = clownfishAnimator;
            Debug.Log("misspelled fish name for animator");
            break;
        }
        return ans;
    }

    public Fish[] getFishData()
    {
        return fishData;
    }

    public int getFishIndex(string _fishName) //same as fishID
    {
        return Array.FindIndex(fishData, fish => fish.fishName == _fishName);
    }

    public Fish getFishByIndex(int index) {
        return fishData[index];
    }

    public string getFishName(int fishIndex)
    {
        //Debug.Log("fishIndex: " + fishIndex);
        return fishData[fishIndex].fishName;
    }

    public int getFishID(int fishIndex) //same as fishIndex why did i do this
    {
        return fishData[fishIndex].ID;
    }

    public string getFishType(int fishIndex)
    {
        return fishData[fishIndex].fishType;
    }

    public string getFishDescription(int fishIndex)
    {
        //Debug.Log("1");
        return fishData[fishIndex].description;
    }

    public Sprite getFishSprite(int fishIndex)
    {
        if (fishIndex == -1) {
            return null;
        }
        Sprite ans;
        string name = getFishName(fishIndex);
        switch(name) 
        {
        case "Clownfish":
            ans = clownfishSprite;
            break;
        case "Pufferfish":
            ans = pufferfishSprite;
            break;
        case "Shrimp":
            ans = shrimpSprite;
            break;
        case "Squid":
            ans = squidSprite;
            break;
        case "Tuna":
            ans = tunaSprite;
            break;
        case "Goldfish":
            ans = goldfishSprite;
            break;
        case "Seahorse":
            ans = seahorseSprite;
            break;
        default:
            ans = clownfishSprite;
            Debug.Log("misspelled fish name for sprite");
            break;
        }
        return ans;
    }

        public Sprite getFishSpriteByName(string fishName)
    {
        //Debug.Log("sprite of: " + fishName);
        Sprite ans;
        switch(fishName) 
        {
        case "Clownfish":
            ans = clownfishSprite;
            break;
        case "Pufferfish":
            ans = pufferfishSprite;
            break;
        case "Shrimp":
            ans = shrimpSprite;
            break;
        case "Squid":
            ans = squidSprite;
            break;
        case "Tuna":
            ans = tunaSprite;
            break;
        case "Goldfish":
            ans = goldfishSprite;
            break;
        case "Seahorse":
            ans = seahorseSprite;
            break;
        default:
            ans = clownfishSprite;
            Debug.Log("misspelled fish name for sprite");
            break;
        }
        return ans;
    }

    public string getFishSpritePath(int fishIndex) {
        return fishData[fishIndex].spriteName;
    }

    public int getFishValue(int fishIndex)
    {
        return fishData[fishIndex].value;
    }

    public int getFishValueByName(string _fishName)
    {
        return fishData[getFishIndex(_fishName)].value;
    }

    public int getFishDataLength()
    {
        return fishData.Length;
    }

    public int getFishPrice(string _fishName) {
        return (int) Math.Floor((getFishValueByName(_fishName) / 2.0));
    }
}

[System.Serializable]
public class FishData
{
    public Fish[] fishData;

    public FishData(Fish[] _fishData)
    {
        fishData = _fishData;
    }
}

[System.Serializable]
public class Fish
{
    public int ID;
    public string fishName;
    public string fishType;
    public string description;
    public string spriteName;
    public int value;
}
