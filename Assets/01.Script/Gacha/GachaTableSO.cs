using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GachaRate
{
    public Rank rank;
    public float rate;
}

[CreateAssetMenu(fileName = "GachaTable", menuName = "ScriptableObjects/GachaTable")]
public class GachaTableSO : ScriptableObject
{
    public List<GachaRate> gachaRateList;
}
