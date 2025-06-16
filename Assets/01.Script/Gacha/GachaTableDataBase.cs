using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GachaRateMapping
{
    public GachaType type;
    public GachaTableSO rateTable;
}

[CreateAssetMenu(fileName = "GachaTableList", menuName = "ScriptableObjects/GachaTableList")]
public class GachaTableDataBase : ScriptableObject
{

    public List<GachaRateMapping> gachaRateMappingList;
}
