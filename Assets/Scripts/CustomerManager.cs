using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private List<AssetReference> references = new List<AssetReference>();
    private int[] specialLevels = new int[] {5, 10, 15, 20, 25};
    [SerializeField]
    private CustomerObject[] currCustomers;
    [SerializeField]
    private IntSO levelSO;
    private int level;
    
    void Start()
    {
        level = levelSO.Value;
        populateList();
    }

    void populateList()
    {
        //
    }
}
