using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<ObjectiveData> missionObjectives;
    private MissionInstance currentMission;

    private void Start()
    {
        StartMission();
    }

    private void Update()
    {
        currentMission?.Update(Time.deltaTime);
    }

    public void StartMission()
    {
        currentMission = new MissionInstance(missionObjectives);
    }

    public void CompleteMission()
    {
        Debug.Log("Mission complete!");
    }
}
