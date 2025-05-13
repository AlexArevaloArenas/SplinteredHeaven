using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Narrative/NPCData")]
public class NPCData : ScriptableObject
{
    public string characterID;
    public string displayName;
    public Sprite portrait;
    public string description;
    public NPC npcEnum;

    public int baseTrust;
    public int baseHealth;

    [System.Serializable]
    public class ScheduleEntry
    {
        public DayMoments timeOfDay; // e.g., "morning", "afternoon", "evening"
        public Location location;  // e.g., "Workshop", "Cafeteria"
    }

    public List<ScheduleEntry> defaultSchedule;
}

public enum Location
{
    Workshop,
    Cafeteria,
    Library,
    Park,
    Home,
    // Add more locations as needed
}