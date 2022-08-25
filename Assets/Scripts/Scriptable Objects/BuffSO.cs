using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Item/Buff")]
public class BuffSO : ScriptableObject
{
    public int ID;
    public string itemName;
    public string description;
    public Sprite artwork;
    public int price;
    public int quantityHeld;
}
