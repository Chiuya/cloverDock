using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Int Array", menuName = "ArraySO/Int Array")]
public class ArrayIntSO : ScriptableObject
{
    [SerializeField]
    private int[] intArray;

    public int[] Value
    {
        get { return intArray; }
        set { intArray = value; }
    }
}
