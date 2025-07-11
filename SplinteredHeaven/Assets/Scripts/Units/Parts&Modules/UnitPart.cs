using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public List<WeaponModuleInstance> Weapons;
    public List<ModuleSlot> slots;
    public Unit owner;
    public event Action<UnitPart> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action OnDestroyed;

    //Shield
    public bool shield;

    //Unit Part Constructor
    public UnitPart(UnitPartData partData, Unit owner)
    {
        this.owner = owner;
        data = partData;
        name = partData.name;
        //size = partData.size;
        maxHealth = partData.maxHealth;
        currentHealth = maxHealth;
        //transform = owner.obj.transform;
        slots = new List<ModuleSlot>();
        Modules = new List<ModuleInstance>();
        foreach (ModuleData moduleData in partData.modules)
        {
            Modules.Add(moduleData.CreateInstance(owner, this));
            //ModuleInstance module = ModuleFactory.CreateModule(moduleData);
            //ModuleInstance module = new ModuleInstance(moduleData,owner,this);
            //Modules.Add(module);
        }
        Weapons = Modules.Where(m => m is WeaponModuleInstance).Cast<WeaponModuleInstance>().ToList();
    }

    public UnitPart(PartRuntimeData runtimeData, UnitPartData staticData, Unit owner, RuntimeAssetRegistry registry)
    {
        this.owner = owner;
        data = staticData;
        name = staticData.name;
        //size = staticData.size;
        maxHealth = staticData.maxHealth;
        currentHealth = maxHealth;
        slots = new List<ModuleSlot>();

        List<ModuleInstance> modules = new List<ModuleInstance>();
        foreach(var module in runtimeData.modules)
        {
            if(module.moduleID == null) { continue; }
            ModuleData moduleData = registry.GetModule(module.moduleID);
            if (moduleData == null)
            {
                Debug.LogWarning($"Missing module data: {module.moduleID}");
                continue;
            }
            var moduleInstance = moduleData.CreateInstanceFromRuntime(owner, this, module);
            modules.Add(moduleInstance);
        }
        Modules = modules;
        Weapons = new List<WeaponModuleInstance>();

        foreach (var module in Modules)
        {
            if (module is WeaponModuleInstance weapon)
            {
                Weapons.Add(weapon);
            }
        }

        // Por esta, que incluye subclases de WeaponModuleInstance:

        /*
        Modules = runtimeData.moduleIDs
            .Select(id => registry.GetModule(id))
            .Where(m => m != null)
            .Select(m => m.CreateInstance(owner, this))
            .ToList();    */
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
        if (shield)
        {
            // If the part has a shield, absorb damage
            Debug.Log($"Shield active, absorbing {dmg} damage.");
            return;
        }

        currentHealth = currentHealth - dmg;
        if (currentHealth <= 0)
        {
            Modules.ForEach(m => m.Disable());
            OnDestroy();
        }
        
        OnHealthChanged?.Invoke(this);
    }

    public bool IsDestroyed()
    {
        return currentHealth <= 0;
    }

    public void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
