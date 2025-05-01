using Ink.Parsed;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Unit
{
    protected GameObject _object;
    protected string _name;
    protected string _description;
    protected float _health;
    protected float _maxHealth;
    protected int _armorClass;
    protected UnitPart[] _parts;
    protected ModuleData[] _modules;
    
    public GameObject Object => _object;
    public string Name => _name;
    public string Description => _description;
    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public ModuleData[] Modules => _modules;
    public int ArmorClass => _armorClass;
    public UnitPart[] Parts => _parts;

    //Private variables
    protected Image _hpBar;

    public Unit(UnitData unitData, Image hpBar)
    {
        List<UnitPart> partsList = new List<UnitPart>();

        _modules = unitData.modules;

        foreach (var part in unitData._partData)
        {
            _maxHealth += part.maxHealth;
            partsList.Add(new UnitPart(part, this));
        }
        _parts = partsList.ToArray();

        _health = _maxHealth;
    }

    public virtual void RefreshPartDamage()
    {
        _health = 0;
        foreach (var part in _parts)
        {
            _health += part.currentHealth;
        }
        _hpBar.fillAmount = _health / _maxHealth;
    }

    protected void Die()
    {
        // Handle unit death
        Debug.Log($"{_name} has died.");
        // Destroy the unit object
        if (_object != null)
        {
            UnityEngine.Object.Destroy(_object);
        }
    }
}
