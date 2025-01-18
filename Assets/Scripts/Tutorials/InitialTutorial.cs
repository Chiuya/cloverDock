using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InitialTutorial : MonoBehaviour
{
    public GameObject arrowUp, arrowDown;
    public GameObject[] objectsArray;
    public PlayerManager playerManager;
    private int tutIndex;
    private string currScene;
    void Awake() {
        int isDone = PlayerPrefs.GetInt("isInitTutDone", 0);
        //isDone = 0; //TESTING PURPOSES ONLY
        if (isDone == 1) {
            Destroy(this);
            return;
        }
        tutIndex = PlayerPrefs.GetInt("initTutIndex", 0);
        //tutIndex = 0; //TESTING PURPOSES ONLY
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("tutindex: " + tutIndex);
        currScene = SceneManager.GetActiveScene().name;
        if (currScene == "Fish1") {
            if (tutIndex < 2 || tutIndex > 6) {
                //correct
                if (objectsArray.Length != 6) {
                    Debug.LogError("incorrect gameobj number for Fish1");
                }
            } else {
                Debug.LogError("incorrect tutorial index for Fish1: " + tutIndex);
            }
        } else if (currScene == "Dock") {
            if (tutIndex == 2 || tutIndex == 6) {
                //correct
                if (objectsArray.Length != 3) {
                    Debug.LogError("incorrect gameobj number for Dock");
                }
            } else {
                Debug.LogError("incorrect tutorial index for Dock: " + tutIndex);
            }
        } else if (currScene == "Gazebo") {
            if (tutIndex > 2 && tutIndex < 6) {
                //correct
                if (objectsArray.Length != 7) {
                    Debug.LogError("incorrect gameobj number for Gazebo");
                }
            } else {
                Debug.LogError("incorrect tutorial index for Gazebo: " + tutIndex);
            }
        }
        setupCurrInstruction();
    }

    public void incrTutIndex() {
        tutIndex++;
        PlayerPrefs.SetInt("initTutIndex", tutIndex);
        //Debug.Log("incrementing tut index: " + tutIndex);
    }

    public void nextStep() {
        tutIndex++;
        PlayerPrefs.SetInt("initTutIndex", tutIndex);
        setupCurrInstruction();
    }

    public void setupCurrInstruction() { //depending on index and currScene, setup step of tutorial
        //Debug.Log("step of tutIndex: " + tutIndex);
        switch (tutIndex) 
        {
            case 0: //Fish1: await fish
                foreach(Transform child in objectsArray[0].transform) { //deactivate everything in canvasMother except inventory (index 2)
                    child.gameObject.SetActive(false);
                }
                objectsArray[0].transform.GetChild(2).gameObject.SetActive(true);
                objectsArray[1].SetActive(false); //returnButton
                break;
            case 1: //Fish1: await return button
                objectsArray[1].SetActive(true);
                objectsArray[2].SetActive(false); //fishingEventHandler
                GameObject case1Arrow = Instantiate(arrowDown, this.transform);
                case1Arrow.transform.position = new Vector3(8.1f, -2.8f, 0f);
                objectsArray[1].gameObject.GetComponent<Button>().onClick.AddListener(() => incrTutIndex());
                break;
            case 2: //Dock, await scene change
                objectsArray[0].SetActive(false); //canvasMother
                objectsArray[1].SetActive(false); //boatSceneLoad
                GameObject case2Arrow = Instantiate(arrowDown, this.transform);
                case2Arrow.transform.position = new Vector3(5.8f, 0f, 0f);
                objectsArray[2].gameObject.GetComponent<Button>().onClick.AddListener(() => incrTutIndex());
                break;
            case 3: //Gazebo, arrow on gazebo shop
                objectsArray[0].transform.GetChild(0).gameObject.SetActive(false); //ui canvas of canvasMother
                objectsArray[1].SetActive(false); //dockSceneLoad
                objectsArray[2].SetActive(false); //shopExteriorSceneLoad
                GameObject case3Arrow = Instantiate(arrowUp, this.transform);
                case3Arrow.transform.position = new Vector3(-8.5f, -6f, 0f);
                objectsArray[4].gameObject.GetComponent<Button>().onClick.AddListener(() => nextStep());
                if (playerManager == null) {
                    Debug.LogError("no playermanager! case 3");
                } else {
                    //playerManager.addCoins(50); //TEMP
                    playerManager.setCoins(40); //clownfish bait price
                }
                break;
            case 4: //Gazebo, bait purchase
                foreach(Transform child in this.transform) {
                    Destroy(child.gameObject);
                }
                objectsArray[3].gameObject.GetComponent<Button>().onClick.AddListener(() => nextStep());
                break;
            case 5: //Gazebo, go back to Dock
                foreach(Transform child in this.transform) {
                    Destroy(child.gameObject);
                }
                objectsArray[4].SetActive(false); //gazebo shop button
                objectsArray[6].transform.GetChild(0).gameObject.SetActive(false); //shop interface
                objectsArray[1].SetActive(true); //dockSceneLoad
                GameObject case5Arrow = Instantiate(arrowDown, this.transform);
                case5Arrow.transform.position = new Vector3(-14.2f, -2.8f, 0f);
                objectsArray[1].gameObject.GetComponent<Button>().onClick.AddListener(() => incrTutIndex());
                break;
            case 6: //Dock, go back to Fish1
                objectsArray[0].SetActive(false); //canvasMother
                objectsArray[2].SetActive(false); //gazeboSignSceneLoad
                GameObject case6Arrow = Instantiate(arrowDown, this.transform);
                case6Arrow.transform.position = new Vector3(-3f, -2f, 0f);
                objectsArray[1].gameObject.GetComponent<Button>().onClick.AddListener(() => incrTutIndex()); //boat
                break;
            case 7: //Fish1, open inv
                objectsArray[1].SetActive(false); //returnButton
                objectsArray[2].SetActive(false); //fishingEventHandler
                GameObject case7Arrow = Instantiate(arrowUp, this.transform);
                case7Arrow.transform.position = new Vector3(6.6f, 3f, 0f);
                objectsArray[3].gameObject.GetComponent<Button>().onClick.AddListener(() => nextStep()); //inv button
                break;
            case 8: //Fish1, use bait
                foreach(Transform child in this.transform) {
                    Destroy(child.gameObject);
                }
                objectsArray[4].gameObject.GetComponent<Button>().onClick.AddListener(() => finishTutorial());
                objectsArray[5].gameObject.GetComponent<Button>().onClick.AddListener(() => finishTutorial());
                break;
            case 9:
                finishTutorial();
                break;
            default:
                Debug.Log("no tutorial step to carry out");
                break;
        }
    }

    private void finishTutorial() {
        PlayerPrefs.SetInt("isInitTutDone", 1);
        PlayerPrefs.SetInt("initTutIndex", 0);
        SceneManager.LoadScene("Fish1");
    }
}
