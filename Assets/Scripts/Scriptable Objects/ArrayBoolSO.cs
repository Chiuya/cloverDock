using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Bool Array", menuName = "ArraySO/Bool Array")]
public class ArrayBoolSO : ScriptableObject
{
    [SerializeField]
    private bool[] boolArray;

    public bool[] Value
    {
        get { return boolArray; }
        set { boolArray = value; }
    }
}
