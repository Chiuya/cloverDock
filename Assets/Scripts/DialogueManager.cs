using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform optionParent;
    private DialogueConvo[] littleBoyData, brideData, boxerData, ballerinaData, samuraiData;

    void Awake() {
        //InitDialogue("Little Boy", 1); //idk?? temp
        littleBoyData = LoadDialogueFromJson("Little Boy");
        brideData = LoadDialogueFromJson("Bride");
        boxerData = LoadDialogueFromJson("Boxer");
        ballerinaData = LoadDialogueFromJson("Ballerina");
        samuraiData = LoadDialogueFromJson("Samurai");
    }

    private DialogueConvo[] LoadDialogueFromJson(string npcName)
    {
        //string jsonFilePath = "Assets/Resources/Dialogue/" + npcName + ".json";
        //string jsonContent = File.ReadAllText(jsonFilePath);

        TextAsset mytxtData = (TextAsset) Resources.Load("Dialogue/" + npcName);
        string jsonContent = mytxtData.text;
        return JsonUtility.FromJson<DialogueData>(jsonContent).convoArray;
    }

    public int getConvoIndex(int currRep) {
        int ans = 0;
        switch (currRep)
        {
            case 1:
                ans = 0;
                break;
            case 10:
                ans = 1;
                break;
            case 20:
                ans = 2;
                break;
            case 40:
                ans = 3;
                break;
            case 60:
                ans = 4;
                break;
            case 80:
                ans = 5;
                break;
            case 100:
                ans = 6;
                break;
            default:
                ans = 0;
                break;
        }
        return ans;
    }

    private DialogueConvo[] getData(string npcName) {
        DialogueConvo[] currNPC;
        switch (npcName) {
            case "Little Boy":
                currNPC = littleBoyData;
                break;
            case "Bride":
                currNPC = brideData;
                break;
            case "Boxer":
                currNPC = boxerData;
                break;
            case "Ballerina":
                currNPC = ballerinaData;
                break;
            case "Samurai":
                currNPC = samuraiData;
                break;
            default:
                currNPC = littleBoyData;
                break;
        }
        return currNPC;
    }

    public int getConvoLength(string customerName, int repCurr) {
        DialogueConvo[] data = getData(customerName);
        int i = getConvoIndex(repCurr);
        //Debug.Log("i at getconvolength: " + i);
        return data[i].length;
    }

    public bool[] getIsActionArray(string customerName, int repCurr) {
        DialogueConvo[] data = getData(customerName);
        int index = getConvoIndex(repCurr);
        return data[index].isAction;
    }

    public string[] getTextArray(string customerName, int repCurr) {
        DialogueConvo[] data = getData(customerName);
        int index = getConvoIndex(repCurr);
        int length = getConvoLength(customerName, repCurr);
        return data[index].text;
        
    }

    public bool isOptionsEmptyAtLine(string customerName, int repCurr, int currLine) {
        return (getOptionAtLine(customerName, repCurr, currLine) == "");
    }

    public string getOptionAtLine(string customerName, int repCurr, int currLine) {
        DialogueConvo[] data = getData(customerName);
        int index = getConvoIndex(repCurr);
        //Debug.Log(data[index].options == null);
        return data[index].options[currLine];
    }

    public void clearOptionChilds(Transform parent) {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void instantiateOptionChilds(string customerName, int repCurr, int currLine) {
        string option = getOptionAtLine(customerName, repCurr, currLine);
        GameObject optionObj = Instantiate(optionPrefab, optionParent);
        optionObj.GetComponentInChildren<TextMeshProUGUI>().text = option;
        optionObj.GetComponent<ButtonFunctions>().assignGoTarget(this.gameObject);
        
    }

}

[System.Serializable]
public class DialogueData
{
    public DialogueConvo[] convoArray; // 7 items in this array
    public DialogueData(DialogueConvo[] _convoArray) {
        convoArray = _convoArray;
    }
}

[System.Serializable]
public class DialogueConvo
{
    public int id; //id number of convo, 0-6
    public int length; //each array should be the same length - num of lines
    public bool[] isAction;
    public string[] text;
    public string[] options;
}