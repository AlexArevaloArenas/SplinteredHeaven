using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Mission")]
public class MissionData : ScriptableObject
{
    public string missionID;
    public string title;
    public string description;
    public List<ObjectiveData> objectives;

}
