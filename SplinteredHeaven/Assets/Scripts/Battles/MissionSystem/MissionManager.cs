using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public bool isMissionActive = false;
    //HUB
    public List<MissionData> availableMissions;

    public MissionData selectedMissionData;
    
    //BATTLE
    public MissionInstance currentMission;

    //SINGLETON PATTERN
    public static MissionManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isMissionActive)
        {
            currentMission?.Update(Time.deltaTime);
        }
        if(currentMission != null && currentMission.IsComplete)
        {
            CompleteMission();
        }
    }

    public void StartMission(MissionData mission)
    {
        TimeManager.Instance.Stop();
        currentMission = new MissionInstance(mission);
        Fader.Instance.StartFade("LoadCurrentMissionSceneEvent");
        Fader.Instance.DoWhenLoaded( () =>EventManager.Instance.StartMission(currentMission));
    }

    public void StartSelectedMission()
    {
        StartMission(selectedMissionData);
    }

    public void CompleteMission()
    {
        TimeManager.Instance.Play();
        EventManager.Instance.EndMission(currentMission);
    }
}
