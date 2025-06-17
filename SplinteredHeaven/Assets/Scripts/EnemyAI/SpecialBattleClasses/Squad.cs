using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class Squad
{
    public List<UnitManager> members; // Array of members in the squad
    public string squadName; // Name of the squad
    public List<SquadType> squadTypes; // Array of squad types this tag can represent

    public Squad(List<UnitManager> members, string squadName, SquadType[] squadTypetTags)
    {
        this.members = members;
        this.squadName = squadName;
        this.squadTypes = new List<SquadType>(squadTypetTags); // Initialize with the provided squad types
    }
}

public class  SquadTag : MonoBehaviour
{
    public SquadType[] squadTypes; // Array of squad types this tag can represent
    public string squadName; // Name of the squad tag
}

public enum SquadType
{
    Scout, // Squad focused on exploration
    Support, // Squad focused on support and healing
    Recon, // Squad focused on reconnaissance and intelligence gathering
    Assault, // Squad focused on direct assaults and heavy firepower
    Defense, // Squad focused on defensive operations and holding positions
}
