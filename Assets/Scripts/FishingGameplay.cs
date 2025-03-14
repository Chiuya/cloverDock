using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FishingGameplay : MonoBehaviour
{
    private const int IncreasedFishChance = 80; //80% chance
    private const int UsableFishChance = 90; //90% chance
    //private const int DefaultFishChance = 25; //25% chance within usableFishChance
    private const int RepThreshold = 25;
    private Fish[] fishData;
    private List<Fish> fishPool = new List<Fish>();
    private List<Fish> nonPriorityPool = new List<Fish>();
    
    public InventoryManager inventoryManager;
    public PlayerManager playerManager;
    public FishDataManager fishDataManager;
    public CustomerDataManager customerDataManager;
    public BuffsManager buffsManager;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        fishData = fishDataManager.getFishData();
        initFishPool();
    }

    private void initFishPool() { 
        int numFish = fishDataManager.getFishDataLength();
        for (int i = 0; i < numFish; i++) {
            Fish fish = fishDataManager.getFishByIndex(i);
            if (customerDataManager.isFishUnlocked(fish.fishName) && !customerDataManager.isGifted(i)) {
                fishPool.Add(fish);
            } else if (customerDataManager.isFishUnlocked(fish.fishName)) {
                nonPriorityPool.Add(fish);
            }
        }
    }

    public int getNumFishInPool() {
        return fishPool.Count;
    }

    public Sprite getFishSpriteFromPoolIndex(int index) {
        if (index == -1) {
            return null;
        }
        //Debug.Log("fish pool index: " + index);
        return fishDataManager.getFishSprite(index);
    }

    public string getFishNameFromPoolIndex(int index) {
        if (index == -1) {
            return "";
        }
        return fishDataManager.getFishName(index);
    }

    public int getFishValueFromPoolIndex(int index) {
        if (index == -1) {
            //Debug.Log("fish index invalid");
            return 0;
        }
        return fishDataManager.getFishValue(index);
    }

    public int getFishBaitPriceFromValue(int value) {
        return (int)value*10;
    }


    public void updateFishPool() { //not needed?
        string lastFish = fishPool[fishPool.Count - 1].fishName;
        string customer = customerDataManager.getCustomerByFish(lastFish);
        if ((fishPool.Count < fishDataManager.getFishDataLength() - 1) && (customerDataManager.getRepCurrByName(customer) >= RepThreshold)) {
            fishPool.Add(fishDataManager.getFishByIndex(fishPool.Count));
        }
    }

    public string fishUpRandom()
    {
        //add a random fish to inventory and return its name
        Fish resultFish = returnRandomFish();
        addFishToInventory(resultFish);
        float value = (float) Math.Floor((resultFish.value / 2.0f));
        playerManager.addExperience(value);
        //Debug.Log("fish caught");
        return resultFish.fishName;
    }

    public Fish returnRandomFish()
    {
        if (buffsManager.isBaitActive()) {
            string targetFish = buffsManager.getCurrBaitName();
            if (fishPool.Exists(fish => fish.fishName == targetFish)) {
                int chance = UnityEngine.Random.Range(0, 100);
                if (chance <= IncreasedFishChance) {
                    Debug.Log("fishing up increased chance: " + targetFish);
                    return fishPool.Find(fish => fish.fishName == targetFish);
                }
            } else {
                Debug.Log("current fish bait is not in pool!");
            }
        }
        int chanceOfUsable = UnityEngine.Random.Range(0, 100);
        if ((chanceOfUsable <= UsableFishChance) || nonPriorityPool.Count == 0) {
            int i = UnityEngine.Random.Range(0, fishPool.Count);
            return fishPool[i];
        } else {
            int i = UnityEngine.Random.Range(0, nonPriorityPool.Count);
            return nonPriorityPool[i];
        }
    }

    //got a fish, talk to inventory and experience in playerData
    public void addFishToInventory(Fish fish)
    {
        inventoryManager.addFishToInventory(fish, 1);
        //playerManager.addExperience(fish.value);
    }

    public string tutorialFishUpClownfish() {
        Fish resultFish = fishDataManager.getFishByIndex(fishDataManager.getFishIndex("Clownfish"));
        addFishToInventory(resultFish);
        //Debug.Log("fish caught");
        return resultFish.fishName;
    }

    public void vibrate() {
        //Handheld.Vibrate();
    }
}
