using UnityEngine;

public class ModuleSlot : MonoBehaviour
{
    public int slot;
    public UnitPart part;
    public bool isOccupied = false;
    public ModulePositionType modulePositionType;
    public ModuleWeightType moduleWeightType;

    public void SetUp(UnitPart assignedPart)
    {
        part = assignedPart;
    }
}
