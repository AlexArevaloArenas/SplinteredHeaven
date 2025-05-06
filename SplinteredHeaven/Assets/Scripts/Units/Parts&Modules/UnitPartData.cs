using UnityEngine;

[CreateAssetMenu(fileName = "UnitPartData", menuName = "Unit/UnitPartData")]
public class UnitPartData : ScriptableObject
{
    [Range(0, 100)]
    public int size;
    public int maxHealth;
    public ModuleData[] modules;
}
