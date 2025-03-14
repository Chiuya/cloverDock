using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class ItemObject : ScriptableObject
{
    public int ID;
    public string itemName;
    public string itemType;
    public string description;
    public Sprite artwork;
    public int value;

}
