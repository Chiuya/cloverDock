using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomerDataManager : MonoBehaviour
{
    public PlayerManager playerManager;
    private const float MaxRep = 100.0f;
    private const float RepPerFish = 0.04f;
    private Customer[] customerData;
    private PlayerReputation playerRep;
    private Sprite littleBoySprite, brideSprite, boxerSprite, ballerinaSprite, samuraiSprite, luckySprite, missySprite;
    private Sprite littleBoyGiftSprite, brideGiftSprite, boxerGiftSprite, ballerinaGiftSprite, samuraiGiftSprite, luckyGiftSprite, missyGiftSprite;

    void Awake() {
        LoadCurrRepFromJSON();
        //resetRep();
        //SaveCurrRepToJSON();
        string jsonFilePath = "Assets/Resources/Data/customerData.json";
        string jsonContent = File.ReadAllText(jsonFilePath);
        customerData = JsonUtility.FromJson<CustomerData>(jsonContent).customerData;
        initSprites();
        //Debug.Log("customerData successfully loaded");
    }

    // Start is called before the first frame update
    void Start()
    {
        //resetRep(); //FOR TESTING
        //setCustomerRepTest("Little Boy", 19.98f); //TEST PURPOSES
        //setIsAwaitingDialogue("Little Boy", false); //TEST PURPOSES
    }

    private void initSprites() {
        //customer icons
        littleBoySprite = Resources.Load<Sprite>("Customers/Little Boy");
        brideSprite = Resources.Load<Sprite>("Customers/Bride");
        boxerSprite = Resources.Load<Sprite>("Customers/Boxer");
        ballerinaSprite = Resources.Load<Sprite>("Customers/Ballerina");
        samuraiSprite = Resources.Load<Sprite>("Customers/Samurai");
        luckySprite = Resources.Load<Sprite>("Customers/Lucky");
        missySprite = Resources.Load<Sprite>("Customers/Missy");
        //customer gift sprites
        littleBoyGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Winslow");
        brideGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Rose");
        boxerGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Teru teru bozu");
        ballerinaGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Music box");
        samuraiGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Origami crane");
        luckyGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Lantern");
        missyGiftSprite = Resources.Load<Sprite>("Customers/Gifts/Windchime");
    }

    public void LoadCurrRepFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/playerReputation.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            playerRep = JsonUtility.FromJson<PlayerReputation>(jsonData);
            //Debug.Log("playerReputation.json successfully read");
        } else
        {
            //Debug.Log("no player rep saved, creating new array");
            resetRep();
            //SaveCurrRepToJSON();
        }        
    }

    public void resetRep() {
        CustomerReputation[] newRepArray = new CustomerReputation[]
            {
                new CustomerReputation("Lucky", 0),
                new CustomerReputation("Missy", 0),
                new CustomerReputation("Little Boy", -1),
                new CustomerReputation("Bride", 0),
                new CustomerReputation("Boxer", 0),
                new CustomerReputation("Ballerina", 0),
                new CustomerReputation("Samurai", 0)
            };
        playerRep = new PlayerReputation(newRepArray);
        SaveCurrRepToJSON();
    }
    
    public void SaveCurrRepToJSON()
    {
        string jsonData = JsonUtility.ToJson(playerRep);
        string filePath = Application.persistentDataPath + "/playerReputation.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public int getCustomerIndex(string _customerName)
    {
        int ans = Array.FindIndex(customerData, customer => customer.customerName == _customerName);
        //Debug.Log(ans);
        return ans;
    }

    public string getCustomerName(int customerIndex)
    {
        //Debug.Log("customer number: " + customerIndex);
        return customerData[customerIndex].customerName;
    }
    
    public string getCustomerDescription(int customerIndex)
    {
        return customerData[customerIndex].description;
    }

    public Sprite getCustomerSprite(string customerName)
    {
        Sprite ans;
        switch(customerName) 
        {
        case "Little Boy":
            ans = littleBoySprite;
            break;
        case "Bride":
            ans = brideSprite;
            break;
        case "Boxer":
            ans = boxerSprite;
            break;
        case "Ballerina":
            ans = ballerinaSprite;
            break;
        case "Samurai":
            ans = samuraiSprite;
            break;
        case "Lucky":
            ans = luckySprite;
            break;
        case "Missy":
            ans = missySprite;
            break;
        default:
            ans = littleBoySprite;
            Debug.Log("misspelled customer name for sprite");
            break;
        }
        return ans;
    }

        public Sprite getGiftByCustomer(string customerName)
        {
        Sprite ans;
        switch(customerName) 
        {
        case "Little Boy":
            ans = littleBoyGiftSprite;
            break;
        case "Bride":
            ans = brideGiftSprite;
            break;
        case "Boxer":
            ans = boxerGiftSprite;
            break;
        case "Ballerina":
            ans = ballerinaGiftSprite;
            break;
        case "Samurai":
            ans = samuraiGiftSprite;
            break;
        case "Lucky":
            ans = luckyGiftSprite;
            break;
        case "Missy":
            ans = missyGiftSprite;
            break;
        default:
            ans = littleBoyGiftSprite;
            Debug.Log("misspelled customer name for gift sprite");
            break;
        }
        return ans;
    }

    public string getGiftName(int customerIndex)
    {
        return customerData[customerIndex].gift;
    }

    public string getfaveFish(int customerIndex)
    {
        return customerData[customerIndex].faveFish;
    }

    public string getFaveFishByName(string customerName) {
        int custIndex = getCustomerIndex(customerName);
        return customerData[custIndex].faveFish;
    }

    public int getRepCurrByName(string customerName)
    {
        return (int) Math.Floor(Array.Find(playerRep.playerRep, rep => rep.name == customerName).currentRep);
    }

    public int getRepCurrByIndex(int customerIndex)
    {
        float rep = playerRep.playerRep[customerIndex].currentRep;
        int ans = (int) Math.Floor(rep);
        //Debug.Log("getting rep at index: " + customerIndex + " = " + ans + ", float val: " + );
        if (rep > 0.0f && ans == 0) {
            return 1;
        } else {
            return ans;
        }
    }

    public int getNumCustomers()
    {
        //Debug.Log("num customers: " + customerData.Length);
        //Debug.Log("num reps: " + playerRep.playerRep.Length);
        return customerData.Length;
    }

    public string getCustomerByFish(string fishName) {
        Customer ans = Array.Find(customerData, customer => customer.faveFish == fishName);
        if (ans == null) {
            Debug.Log("cant find customer by fish");
            return fishName;
        }
        return ans.customerName;
    }

    public bool getIsAwaitingDialogue(string customer) {
        CustomerReputation ans = Array.Find(playerRep.playerRep, rep => rep.name == customer);
        if (ans == null) {
            Debug.Log("cant find customer for dialogue check");
            return false;
        }
        return ans.isAwaitingDialogue;
    }

    public void setIsAwaitingDialogue(string customer, bool _bool) {
        CustomerReputation ans = Array.Find(playerRep.playerRep, rep => rep.name == customer);
        if (ans == null) {
            Debug.Log("cant find customer to set dialogue");
        }
        ans.isAwaitingDialogue = _bool;
    }

    public string getIsGifted(string _customer) {
        CustomerReputation ans = Array.Find(playerRep.playerRep, rep => rep.name == _customer);
        if (ans == null) {
            Debug.Log("cant find customer to get is gifted");
            return "";
        }
        if (!ans.isAwaitingDialogue && ans.currentRep >= MaxRep) {
            return Array.Find(customerData, customer => customer.customerName == _customer).gift;
        } else {
            //Debug.Log(_customer + " has not sent a gift yet");
            return "";
        }
    }

    public bool isGifted(int customerIndex) {
        return (!playerRep.playerRep[customerIndex].isAwaitingDialogue && playerRep.playerRep[customerIndex].currentRep >= MaxRep);
    }


    public void setCustomerRepTest(string customerName, float repToSet) {
        //TEST PURPOSES ONLY
        int customerIndex = getCustomerIndex(customerName);
        playerRep.playerRep[customerIndex].currentRep = repToSet;
        SaveCurrRepToJSON();
    }

    public void addOneRep(string customerName) {
        int customerIndex = getCustomerIndex(customerName);
        if (customerIndex < 0) {
            Debug.Log("customer index error, cant add one rep");
        }
        playerRep.playerRep[customerIndex].currentRep += RepPerFish;
        float currRep = playerRep.playerRep[customerIndex].currentRep;
        if (isMilestoneRep(currRep)) {
            playerRep.playerRep[customerIndex].isAwaitingDialogue = true;
            //Debug.Log(customerName + " is awaiting dialogue!");
        } else {
            playerRep.playerRep[customerIndex].isAwaitingDialogue = false;
        }
        playerRep.playerRep[customerIndex].currentRep = (float) Mathf.Min(currRep, MaxRep);
        //Debug.Log("add one rep complete: " + playerRep.playerRep[customerIndex].currentRep);
        SaveCurrRepToJSON();
    }

    private bool isMilestoneRep(float currRep) {
        if (Mathf.Approximately(currRep, 1.0f) || Mathf.Approximately(currRep, 10.0f) || Mathf.Approximately(currRep, 20) || Mathf.Approximately(currRep, 40) ||
        Mathf.Approximately(currRep, 60) || Mathf.Approximately(currRep, 80) || Mathf.Approximately(currRep, MaxRep)) {
            return true;
        } else {
            return false;
        }
    }

    public bool isCustomerUnlocked(string _name) {
        int level = playerManager.getLevel();
        switch (_name) {
            case "Lucky":
                return true;
            case "Missy":
                return true;
            case "Little Boy":
                return true;
            case "Bride":
                return (level >= 5);
            case "Boxer":
                return (level >= 10);
            case "Ballerina":
                return (level >= 16);
            case "Samurai":
                return (level >= 23);
            default:
                Debug.Log("invalid name for customerUnlock check: " + _name);
                return false;
        }
    }

    public int customerUnlockLevel(string _name) {
        switch (_name) {
            case "Lucky":
                return 1;
            case "Missy":
                return 1;
            case "Little Boy":
                return 1;
            case "Bride":
                return 5;
            case "Boxer":
                return 10;
            case "Ballerina":
                return 16;
            case "Samurai":
                return 23;
            default:
                Debug.Log("invalid name for customerUnlock check: " + _name);
                return Int32.MaxValue;
        }
    }

    public bool isFishUnlocked(string _fish) {
        string customer = getCustomerByFish(_fish);
        return isCustomerUnlocked(customer);
    }

    public bool isDefaultCustomer(string _name) {
        return ((_name == "Lucky") || (_name == "Missy"));
    }

    public int getNumUnlockedStories(int currRep) {
        int ans;
        if (currRep < 10) {
            ans = 1;
        } else if (currRep < 20) {
            ans = 2;
        } else if (currRep < 40) {
            ans = 3;
        } else if (currRep < 60) {
            ans = 4;
        } else if (currRep < 80) {
            ans = 5;
        } else if (currRep < 100) {
            ans = 6;
        } else if (currRep >= 100) {
            ans = 7;
        } else {
            ans = 0;
            Debug.Log("numUnlockedStories error");
        }
        //Debug.Log("num unlocked stories: " + ans + " based on currRep of: " + currRep);
        return ans;
    }

    public int getRepByStoryIndex(int storyIndex) {
        int ans;
        switch (storyIndex)
        {
            case 0:
                ans = 1;
                break;
            case 1:
                ans = 10;
                break;
            case 2:
                ans = 20;
                break;
            case 3:
                ans = 40;
                break;
            case 4:
                ans = 60;
                break;
            case 5:
                ans = 80;
                break;
            case 6:
                ans = 100;
                break;
            default:
                ans = 0;
                break;
        }
        return ans;
    }

    public string repPercentString(string _customer) {
        float rep = Array.Find(playerRep.playerRep, rep => rep.name == _customer).currentRep;
        return (percentToNextRep(rep).ToString() + "%");
    }

    private int percentToNextRep(float rep) {
        var first2DecimalPlaces = (int)(((decimal)rep % 1) * 100);
        //Console.Write("{0:00}", first2DecimalPlaces);
        return (int) first2DecimalPlaces;
    }
    
}

[System.Serializable]
public class CustomerData
{
    public Customer[] customerData;

    public CustomerData(Customer[] _customerData)
    {
        customerData = _customerData;
    }
}

[System.Serializable]
public class Customer
{
    public string customerName;
    public string description;
    public string spriteName;
    public string gift;
    public string faveFish;

}


[System.Serializable]
public class CustomerReputation
{
    public string name;
    public float currentRep;
    public bool isAwaitingDialogue;

    public CustomerReputation(string _name, float _currentRep)
    {
        name = _name;
        currentRep = _currentRep;
        isAwaitingDialogue = false;
    }
}

[System.Serializable]
public class PlayerReputation
{
    public CustomerReputation[] playerRep;

    public PlayerReputation(CustomerReputation[] _customerRep)
    {
        playerRep = _customerRep;
    }
}
