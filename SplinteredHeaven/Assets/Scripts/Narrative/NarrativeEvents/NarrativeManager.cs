using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NarrativeManager : MonoBehaviour
{
    public static NarrativeManager Instance { get; private set; }

    public List<NarrativeEventData> allEvents;
    public Dictionary<string, NarrativeEventInstance> activeInstances = new();
    private NarrativeContext context;

    public List<NPCInstance> npcInstances;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        npcInstances = new List<NPCInstance>();
    }

    private void Start()
    {
        EventManager.Instance.StartGameEvent+= Init; // Initialize narrative events when the game starts
    }

    public void Init()
    {
        context = new NarrativeContext
        {
            dialogueManager = (DialogueManager)DialogueManager.Instance,
            narrativeEventManager = this,
            timeManager = TimeManager.Instance
        };



        foreach (var evt in allEvents)
        {
            var instance = new NarrativeEventInstance(evt, context);
            activeInstances[evt.eventID] = instance;
        }
    }

    public void LoadNewEvents(List<NarrativeEventData> newEvents)
    {
        /*
        activeInstances.Clear();
        allEvents.Clear();
        */
        allEvents.AddRange(newEvents);
        foreach (var evt in newEvents)
        {
            var instance = new NarrativeEventInstance(evt, context);
            activeInstances[evt.eventID] = instance;
        }
    }

    public void TriggerEvent(NarrativeEventData def)
    {
        if (activeInstances.TryGetValue(def.eventID, out var instance))
        {
            instance.MarkTriggered();
            if (def.removeAfterExecution)
            {
                activeInstances.Remove(def.eventID);
                allEvents.Remove(def);
            }
            
        }
    }

    public void Spawn(GameObject unit, Vector3 pos)
    {
        Instantiate(unit, pos, Quaternion.identity);
    }

    public void RegisterNPC(NPCInstance npcInstance)
    {
        npcInstances.Add(npcInstance);
    }

    

}

public class NarrativeContext
{
    public DialogueManager dialogueManager;
    public NarrativeManager narrativeEventManager;
    public TimeManager timeManager;
    public PlayableDirector timelineDirector; // Reference to the PlayableDirector for Timeline events
    // Add more as needed (flags, cutscene system, etc.)

    public void MoveCharacter(string id, Vector3 destination)
    {
        foreach (var npc in narrativeEventManager.npcInstances)
        {
            if (npc.NPCData.characterID == id)
            {
                npc.npcManager.MoveTo(null);
                return;
            }
        }
    }

    public void PatrolCharacter(string id)
    {
        foreach (var npc in narrativeEventManager.npcInstances)
        {
            if (npc.NPCData.characterID == id)
            {
                npc.npcManager.StartPatrol();
                return;
            }
        }
    }
}