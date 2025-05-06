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

    public GameObject obj;
    public string name;
    public string description;
    public float currentHealth;
    public float maxHealth;
    public int armorClass;
    public ModuleData[] modules;

    //Private variables
    protected Image _hpBar;

    public Unit(UnitData unitData, GameObject _obj)
    {
        obj = _obj;
        List<UnitPart> partsList = new List<UnitPart>();

        modules = unitData.modules;

        foreach (var part in unitData._partData)
        {
            maxHealth += part.maxHealth;
            UnitPart unitPart = new UnitPart(part, this);
            unitPart.OnHealthChanged += HandlePartHealthChanged;
            partsList.Add(unitPart);
        }
        Parts = partsList;

        currentHealth = maxHealth;
        //currentHealth => Parts.Sum(p => p.currentHealth);

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
        foreach (UnitPart p in Parts)
        {
            currentHealth += p.currentHealth;
        }
        _hpBar.fillAmount = currentHealth / maxHealth;
    }

    public List<WeaponModuleInstance> GetAvailableWeapons() =>
        Parts.SelectMany(p => p.Modules)
             .OfType<WeaponModuleInstance>()
             .Where(w => w.IsAvailable())
             .ToList();

    protected void Die()
    {
        // Handle unit death
        Debug.Log($"{name} has died.");
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
