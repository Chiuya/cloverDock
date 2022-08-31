using CI.QuickSave;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    public IntSO coinsSO;
    public FloatSO energySO;
    public FloatSO energyTimerSO;
    public FloatSO experienceSO;
    public ArrayIntSO invIdArraySO;
    public ArrayIntSO invQuantityArraySO;
    public IntSO levelSO;

    public void Save()
    {
        QuickSaveWriter.Create("PlayerData")
                       .Write("coins", coinsSO.Value)
                       .Write("energy", energySO.Value)
                       .Write("energyTimer", energyTimerSO.Value)
                       .Write("experience", experienceSO.Value)
                       .Write("invIdArray", invIdArraySO.Value)
                       .Write("invQuantityArray", invQuantityArraySO.Value)
                       .Write("level", levelSO.Value)
                       .Commit();
    }

    public void Load()
    {
        //check if file exists
        QuickSaveReader.Create("PlayerData")
                       .Read<int>("coins", (r) => { coinsSO.Value = r; })
                       .Read<float>("energy", (r) => { energySO.Value = r; })
                       .Read<float>("energyTimer", (r) => { energyTimerSO.Value = r; })
                       .Read<float>("experience", (r) => { experienceSO.Value = r; })
                       .Read<int[]>("invIdArray", (r) => { invIdArraySO.Value = r; })
                       .Read<int[]>("invQuantityArray", (r) => { invQuantityArraySO.Value = r; })
                       .Read<int>("level", (r) => { levelSO.Value = r; });

        //else debug.log file doesnt exist, return null
    }
}
