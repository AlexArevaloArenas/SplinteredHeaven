using System.Collections.Generic;
using UnityEngine;

public class NPCInstance
{
    public NPCData NPCData { get; private set; }
    public NPCManager npcManager { get; private set; } // Reference to the NPCManager

    // Dynamic values
    public Location currentLocation;
    public int currentTrust { get; private set; }
    public int currentHealth { get; private set; }

    public Dictionary<string, bool> flags = new();


    public NPCInstance(NPCData npcData, NPCManager _npcManager)
    {
        NPCData = npcData;
        currentTrust = npcData.baseTrust;
        currentHealth = npcData.baseHealth;
        npcManager = _npcManager;
    }

    public void MoveToLocation(Vector3 location)
    {
        npcManager.transform.position = location;
    }

    public void SetFlag(string flagName, bool value)
    {
        flags[flagName] = value;
    }

    public bool GetFlag(string flagName)
    {
        return flags.TryGetValue(flagName, out bool value) && value;
    }

}
