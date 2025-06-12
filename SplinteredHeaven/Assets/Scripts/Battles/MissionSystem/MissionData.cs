using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Mission")]
public class MissionData : ScriptableObject
{
    public string missionID;
    public string title;
    [TextArea] public string description;
    public List<ObjectiveData> objectives;
    public SceneEnum scene;
    public MissionLocation location;
}
public enum MissionLocation
{
    Place1,
    Place2,
    Place3,

}
