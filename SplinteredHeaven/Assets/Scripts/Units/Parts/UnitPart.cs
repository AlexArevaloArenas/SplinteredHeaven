using UnityEngine;

public class UnitPart
{
    [Range(0, 100)]
    public int size;
    public UnitPartData unitPartData;
    public float maxHealth;
    public float currentHealth;
    public Unit unit;
    public ModuleData[] modules;
    public UnitPart(UnitPartData unitPartData, Unit unit)
    {
        this.unitPartData = unitPartData;
        maxHealth = unitPartData.maxHealth;
        currentHealth = maxHealth;
        size = unitPartData.size;
        this.unit = unit;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public virtual void Destroy()
    {
        // Handle destruction logic here
        Debug.Log("Unit part destroyed");
    }
}
