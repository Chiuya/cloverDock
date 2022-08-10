using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventorySlot
{
    [SerializeField]
    private int _id;
    public int id
    {
        get { return _id; }
        set { _id = value; }
    }

    [SerializeField]
    private int _quantity;
    public int quantity
    {
        get { return _quantity; }
        set { _quantity = value;}
    }
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public InventorySlot[] items;
}


