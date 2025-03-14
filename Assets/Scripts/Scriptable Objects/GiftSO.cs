using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gift", menuName = "Item/Gift")]
public class GiftSO : ScriptableObject
{
    public string giftName;
    public Sprite artwork;
}
