using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UnitPart
{
    public string name;
    public UnitPartData data;
    public int size;
    public float maxHealth;
    public float currentHealth;
    public Transform transform;
    public GameObject partObject;
    public List<ModuleInstance> Modules;
    public Unit owner;
    public event Action<UnitPart> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDestroyed;

    //Unit Part Constructor
    public UnitPart(UnitPartData partData, Unit owner)
    {
        this.owner = owner;
        data = partData;
        name = partData.name;
        size = partData.size;
        maxHealth = partData.maxHealth;
        currentHealth = maxHealth;
        //transform = owner.obj.transform;

        Modules = new List<ModuleInstance>();
        foreach (ModuleData moduleData in partData.modules)
        {
            Modules.Add(moduleData.CreateInstance(owner, this));
            //ModuleInstance module = ModuleFactory.CreateModule(moduleData);
            //ModuleInstance module = new ModuleInstance(moduleData,owner,this);
            //Modules.Add(module);
        }
    }

    public void AddModule(ModuleInstance module)
    {
        Modules.Add(module);
    }

    public void TickModules(float deltaTime)
    {
        foreach (var m in Modules)
            m.Tick(deltaTime);
    }

    public void TakeDamage(float dmg)
    {
        OnDamageTaken?.Invoke(dmg);
        currentHealth = Mathf.Max(0, currentHealth - dmg);
        if (currentHealth == 0)
        {
            Modules.ForEach(m => m.Disable());

        }
        
        OnHealthChanged?.Invoke(this);
    }

    public void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
