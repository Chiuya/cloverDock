using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerPoolManager : MonoBehaviour
{
    public CustomerDataManager customerDataManager;
    public FishDataManager fishDataManager;
    public GameObject customerGO, dialogueManagerGO;
    private List<string> activeCustomers; //change to set
    private const int RepThreshold = 25;
    private List<string> customerNamePool = new List<string>(); //change to set
    private GameObject saleManagerGO;
    private RuntimeAnimatorController littleBoyController, brideController, boxerController, ballerinaController, samuraiController, luckyController, missyController;
    
    void Awake() {
        initAnimators();
    }

    // Start is called before the first frame update
    void Start()
    {
        saleManagerGO = this.transform.parent.gameObject;
        initPool();
        activeCustomers = new List<string>();
    }

    private void initAnimators() {
        littleBoyController = Resources.Load<RuntimeAnimatorController>("Animations/Little Boy");
        brideController = Resources.Load<RuntimeAnimatorController>("Animations/Bride");
        boxerController = Resources.Load<RuntimeAnimatorController>("Animations/Boxer");
        ballerinaController = Resources.Load<RuntimeAnimatorController>("Animations/Ballerina");
        samuraiController = Resources.Load<RuntimeAnimatorController>("Animations/Samurai");
        luckyController = Resources.Load<RuntimeAnimatorController>("Animations/Lucky");
        missyController = Resources.Load<RuntimeAnimatorController>("Animations/Missy");
    }

    private void initPool() {
        int numCust = customerDataManager.getNumCustomers();
        for (int i = 0; i < numCust; i++) {
            string name = customerDataManager.getCustomerName(i);
            if (customerDataManager.getIsGifted(name) != "") { //has already sent a gift
                break;
            }
            if (customerDataManager.isCustomerUnlocked(name)) {
                customerNamePool.Add(name);
            } else {
                break;
            }
        }
        if (customerNamePool.Count == 0) { //all have sent gift, put them all in
            for (int i = 0; i < numCust; i++) {
                string name = customerDataManager.getCustomerName(i);
                customerNamePool.Add(name);
            }
        }
    }

    private RuntimeAnimatorController getAnimator(string name) {
        RuntimeAnimatorController ans;
        switch(name) 
        {
        case "Little Boy":
            ans = littleBoyController;
            break;
        case "Bride":
            ans = brideController;
            break;
        case "Boxer":
            ans = boxerController;
            break;
        case "Ballerina":
            ans = ballerinaController;
            break;
        case "Samurai":
            ans = samuraiController;
            break;
        case "Lucky":
            ans = luckyController;
            break;
        case "Missy":
            ans = missyController;
            break;
        default:
            ans = littleBoyController;
            Debug.Log("misspelled customer name for animator");
            break;
        }
        return ans;
    }

    public void updateCustomerPool() {
        string lastName = customerNamePool[customerNamePool.Count - 1];
        if ((customerNamePool.Count < customerDataManager.getNumCustomers() - 1) && (customerDataManager.getRepCurrByName(lastName) >= RepThreshold)) {
            customerNamePool.Add(customerDataManager.getCustomerName(customerNamePool.Count));
        }
    }

    public void addActiveCustomer(string customer) {
        if (customer == "") {
            return;
        }
        activeCustomers.Add(customer);
    }

    public void removeActiveCustomer(string customer) {
        activeCustomers.Remove(customer);
    }

    private string getRandomCustomer() {
        if (customerNamePool.Count <= activeCustomers.Count) {
            return "";
        }
        var availableCustomers = customerNamePool.Except(activeCustomers);
        int count = 0;
        foreach(var item in availableCustomers) {
            count++;
        }
        int i = Random.Range(0, count);
        int curr = 0;
        string ans = "";
        foreach(var item in availableCustomers) {
            if (curr == i) {
                ans = item;
                break;
            } else {
                curr++;
            }
        }
        return ans;
    }

    public void instantiateRandomCustomer() {
        string customer = getRandomCustomer();
        if (customer == "") {
            //full pool, dont spawn anything
            //Debug.Log("all customers already in shop");
            return;
        } else {
            GameObject customerGO = spawnSpecificCustomer(customer);
            addActiveCustomer(customer);
        }
    }

    public GameObject spawnSpecificCustomer(string customer) {
        int customerIndex = customerDataManager.getCustomerIndex(customer);
        GameObject custGO = Instantiate(customerGO, this.transform);
        //Debug.Log(custGO.GetComponents<PlayMakerFSM>()[0].FsmName);
        PlayMakerFSM theFsm = custGO.GetComponents<PlayMakerFSM>()[0]; //first FSM == "CustomerFSM"
        theFsm.FsmVariables.GetFsmString("name").Value = customer;
        string fish = customerDataManager.getfaveFish(customerIndex);
        theFsm.FsmVariables.GetFsmString("faveFish").Value = fish;
        theFsm.FsmVariables.GetFsmGameObject("saleManagerGO").Value = saleManagerGO;
        custGO.gameObject.GetComponent<SpriteRenderer>().sprite = customerDataManager.getCustomerSprite(customer);
        custGO.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = fishDataManager.getFishSpriteByName(fish);
        custGO.gameObject.GetComponent<Animator>().runtimeAnimatorController = getAnimator(customer);
        if (customerDataManager.getIsAwaitingDialogue(customer)) {
            theFsm.SendEvent("DIALOGUE");
        }
        return custGO;
    }

}
