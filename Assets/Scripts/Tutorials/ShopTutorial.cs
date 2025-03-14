using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using HutongGames.PlayMaker;

public class ShopTutorial : MonoBehaviour
{
    public GameObject arrowUp, arrowDown, tap;
    public CustomerPoolManager customerPoolManager;
    public GameObject[] objectsArray;
    private int tutIndex;
    void Awake() {
        //PlayerPrefs.SetInt("isShopTutDone", 0); //TEST
        int isDone = PlayerPrefs.GetInt("isShopTutDone", 0);
        //isDone = 0; //TESTING PURPOSES ONLY
        if (isDone == 1) {
            Destroy(this);
            return;
        }
        tutIndex = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        setupCurrInstruction();
    }

    public void nextStep() {
        tutIndex++;
        setupCurrInstruction();
    }

    public void setupCurrInstruction() { //depending on index and currScene, setup step of tutorial
        //Debug.Log("step of tutIndex: " + tutIndex);
        switch (tutIndex) 
        {
            case 0: //Await unlock tank
                objectsArray[4].GetComponent<Button>().enabled = false; //disable all other tank buttons (4-7)
                objectsArray[5].GetComponent<Button>().enabled = false;
                objectsArray[6].GetComponent<Button>().enabled = false;
                objectsArray[7].GetComponent<Button>().enabled = false;
                GameObject case0Arrow = Instantiate(arrowUp, this.transform);
                case0Arrow.transform.position = new Vector3(2f, -4.5f, 0f);
                objectsArray[0].SetActive(false); //door
                objectsArray[2].GetComponent<PlayMakerFSM>().enabled = false; //spawn customer FSM
                objectsArray[3].SetActive(false); //ui canvas
                objectsArray[1].gameObject.GetComponent<Button>().onClick.AddListener(() => nextStep());
                break;
            case 1: //Await Stock tank with wanted fish
                GameObject custGO = customerPoolManager.spawnSpecificCustomer("Little Boy");
                GameObject case1Arrow = Instantiate(arrowDown, custGO.transform, false);
                case1Arrow.transform.position = case1Arrow.transform.position + new Vector3(0f, 3f, 0f);
                case1Arrow.transform.localScale = case1Arrow.transform.localScale - new Vector3(0.5f, 0.5f, 0f);
                this.GetComponent<PlayMakerFSM>().SendEvent("COUNTDOWN");
                objectsArray[8].gameObject.GetComponent<Button>().onClick.AddListener(() => nextStep());
                break;
            case 2: //Await tap on customer
                foreach(Transform child in this.transform) { //get rid of arrow pointing at tank
                    Destroy(child.gameObject);
                }
                GameObject tapPrompt = Instantiate(tap, objectsArray[9].transform.GetChild(0), false);
                tapPrompt.transform.position = tapPrompt.transform.position + new Vector3(0.4f, 0f, 0f);
                tapPrompt.transform.localScale = tapPrompt.transform.localScale - new Vector3(0.75f, 0.75f, 0f);
                objectsArray[9].transform.GetChild(0).GetComponents<PlayMakerFSM>()[0].FsmVariables.GetFsmGameObject("tutGO").Value = this.gameObject;
                break;
            case 3:
                finishTutorial();
                break;
            default:
                Debug.Log("no shop tutorial step to carry out");
                break;
        }
    }

    private void finishTutorial() {
        PlayerPrefs.SetInt("isShopTutDone", 1);
        SceneManager.LoadScene("ShopInterior");
    }
}
