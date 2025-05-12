using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCInstance NPCInstance { get; private set; }
    public NPCData npcData;

    private void Awake()
    {
        // Initialize the NPC instance
        NPCInstance = new NPCInstance(npcData);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
