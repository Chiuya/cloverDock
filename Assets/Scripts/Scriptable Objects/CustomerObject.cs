using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Customer")]
public class CustomerObject : ScriptableObject
{
    public string customerName;
    public string description;
    public Sprite artwork;
    public GameObject gift;
    public ItemObject faveFish;
    public int repCurr;
    public GameObject dialogueManager;

}
