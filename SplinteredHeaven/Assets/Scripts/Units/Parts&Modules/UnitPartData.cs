using UnityEngine;

[CreateAssetMenu(fileName = "PartData", menuName = "Units/PartData")]
public class UnitPartData : ScriptableObject
{
    public string id;
    [SerializeField] public PartType partType;
    public GameObject visualPrefab;
    public GameObject destructionPrefab; // Optional prefab for destruction effects
    public ModuleData[] modules;
    public bool SupportUnit;

    [Header("Stats")]
    public int maxHealth;
    public int visionRange;
    public int detectionRange;
    public float detectionInterval;
    public float movementSpeed;

}

public enum PartType
{
    Head,
    Body,
    ArmRight,
    ArmLeft,
    Legs,
}