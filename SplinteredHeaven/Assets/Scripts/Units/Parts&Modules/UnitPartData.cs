using UnityEngine;

[CreateAssetMenu(fileName = "PartData", menuName = "Units/PartData")]
public class UnitPartData : ScriptableObject
{
    public string id;
    public int size;
    public int maxHealth;
    [SerializeField] public PartType partType;
    public GameObject visualPrefab;
    public ModuleData[] modules;
}

public enum PartType
{
    Head,
    Body,
    ArmRight,
    ArmLeft,
    Legs,
}