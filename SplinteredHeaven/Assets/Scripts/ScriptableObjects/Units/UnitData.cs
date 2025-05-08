using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Units/Unit", order = 1)]
[System.Serializable]
public class UnitData : ScriptableObject
{
    public string code;
    public string UnitName;
    public string description;
    //public GameObject prefab;
    public Sprite Image;
    //public ModuleData[] modules;
    public List<UnitPartData> _partData;
}
