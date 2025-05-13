using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCInstance NPCInstance { get; private set; }
    public NPCData npcData;

    private void Awake()
    {
        // Initialize the NPC instance
        NPCInstance = new NPCInstance(npcData,this);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NarrativeEventManager.Instance.npcInstances.Add(NPCInstance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum NPC
{
    Commander,
    Scientist,
    Pilot1,
    Pilot2,
    Pilot3,
    // Add more characters as needed
}
