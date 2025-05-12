using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class NarrativeEventManager : MonoBehaviour
{
    public static NarrativeEventManager Instance { get; private set; }

    private NarrativeContext context;
    public List<NarrativeEventTrigger> triggers;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        context = new NarrativeContext
        {
            dialogueManager = DialogueManager.instance,
            narrativeEventManager = this
        };
    }

    public void TriggerEvent(NarrativeEvent narrativeEvent)
    {
        Debug.Log($"Triggering Event: {narrativeEvent.eventName}");
        narrativeEvent.Execute(context);
    }

    private void Update()
    {
        foreach (var trigger in triggers)
        {
            trigger.CheckTrigger(context);
        }
    }

}

public class NarrativeContext
{
    public DialogueManager dialogueManager;
    public NarrativeEventManager narrativeEventManager;

    // Add more as needed (flags, cutscene system, etc.)
}