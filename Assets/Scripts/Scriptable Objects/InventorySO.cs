using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Slot
{
    public int id;
    public int amountHeld;
    public bool isUnlocked;
}

[System.Serializable, CreateAssetMenu(fileName = "New Inventory", menuName = "Container System/Inventory")]
public class InventorySO : ScriptableObject
{
    public List<Slot> items;
}


