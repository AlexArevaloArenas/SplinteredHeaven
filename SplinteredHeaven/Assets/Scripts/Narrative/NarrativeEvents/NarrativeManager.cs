using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Playables;

public class NarrativeManager : MonoBehaviour
{
    public static NarrativeManager Instance { get; private set; }

    public List<NarrativeEventData> allEvents;
    private Dictionary<string, NarrativeEventInstance> activeInstances = new();
    private NarrativeContext context;

    public List<NPCInstance> npcInstances;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        context = new NarrativeContext
        {
            dialogueManager = (DialogueManager)DialogueManager.Instance,
            narrativeEventManager = this,
            timeManager = TimeManager.Instance
        };

        npcInstances = new List<NPCInstance>();

        foreach (var evt in allEvents)
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
        }
    }

}

public class NarrativeContext
{
    public DialogueManager dialogueManager;
    public NarrativeManager narrativeEventManager;
    public TimeManager timeManager;
    public PlayableDirector timelineDirector; // Reference to the PlayableDirector for Timeline events
    // Add more as needed (flags, cutscene system, etc.)

    public void MoveCharacter(string id, string destination)
    {
        // Your logic to move a character
    }
}