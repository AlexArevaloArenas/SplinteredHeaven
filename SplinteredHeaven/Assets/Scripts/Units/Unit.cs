using Ink.Parsed;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Unit
{
    public List<UnitPart> Parts;
    public float Health => Parts.Sum(p => p.currentHealth);
    public UnitData unitData;
    public GameObject obj;
    public string name;
    public string description;
    public float currentHealth;
    public float maxHealth;
    public List<DamageModifier> damgMods;

    //Private variables
    protected Image _hpBar;

    //Stats
    public float speed;
    public float maxAcceleration;
    public int visionRange;
    public int detectionRange;
    public float detectionInterval;

    public Unit(UnitData unitData, GameObject _obj)
    {
        this.unitData = unitData;
        name = unitData.id;
        obj = _obj;
        List<UnitPart> partsList = new List<UnitPart>();

        //modules = unitData.modules;

        foreach (var part in unitData._partData)
        {
            maxHealth += part.maxHealth;
            UnitPart unitPart = new UnitPart(part, this);
            unitPart.OnHealthChanged += HandlePartHealthChanged;
            partsList.Add(unitPart);
        }
        Parts = partsList;
        InitStats();
        currentHealth = maxHealth;
        //currentHealth => Parts.Sum(p => p.currentHealth);

    }

    public Unit(UnitRuntimeData runtimeData, UnitData staticData, RuntimeAssetRegistry registry)
    {
        this.name = string.IsNullOrEmpty(runtimeData.unitNameOverride) ? unitData.name : runtimeData.unitNameOverride;
        unitData = staticData;
        List<UnitPart> newParts = new List<UnitPart>();

        foreach (var runtimePart in runtimeData.parts)
        {
            UnitPartData partData = registry.GetPart(runtimePart.partID);
            if (partData == null)
            {
                Debug.LogWarning($"Missing part data: {runtimePart.partID}");
                continue;
            }

            var unitPart = new UnitPart(runtimePart, partData, this, registry);
            unitPart.OnHealthChanged += HandlePartHealthChanged;
            unitPart.owner = this;
            newParts.Add(unitPart);
        }
        Parts = newParts;
        InitStats();
        
    }

    public void InitStats()
    {
        currentHealth = Parts.Sum(p => p.currentHealth);
        maxHealth = Parts.Sum(p => p.maxHealth);
        //speed = unitData.speed;
        //maxAcceleration = Parts.Sum(p => p.data.max)
        visionRange = Parts.Sum(p => p.data.visionRange);
        detectionRange = Parts.Sum(p => p.data.detectionRange);
        detectionInterval = Parts.Sum(p => p.data.detectionInterval);


    }

    public virtual void RefreshPartDamage()
    {
        /*
        foreach (var part in Parts)
        {
            _health += part.currentHealth;
        }
        */
        _hpBar.fillAmount = Health / maxHealth;
    }

    void HandlePartHealthChanged(UnitPart part)
    {
        Debug.Log("Health changed");
        currentHealth = 0;
        foreach (UnitPart p in Parts)
        {
            currentHealth += p.currentHealth;
        }
        _hpBar.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public List<WeaponModuleInstance> GetAvailableWeapons() =>
        Parts.SelectMany(p => p.Modules)
             .OfType<WeaponModuleInstance>()
             .Where(w => w.IsAvailable())
             .ToList();

    protected void Die()
    {
        // Handle unit death

        // Destroy the unit object
        if (obj != null)
        {
            UnityEngine.Object.Destroy(obj);
        }
    }

    public void InitHealthBar(Image hpBar)
    {
        _hpBar = hpBar;
        _hpBar.fillAmount = currentHealth / maxHealth;
    }
}