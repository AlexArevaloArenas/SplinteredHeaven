using UnityEngine;

public class NPC_Interaction : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionText;

    [SerializeField] TextAsset inkJSON;
    [SerializeField] public Transform transformNPCView;

    private bool isInteracting = false; // Flag to check if the NPC is currently being interacted with

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.EndFPDialogueEvent += Exit; // Subscribe to exit event to reset interaction state
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject interactor)
    {
        if (isInteracting) return; // If already interacting, do nothing
        EventManager.Instance.StartFirstPersonDialogue(inkJSON, transformNPCView.position);
        isInteracting = true; // Set the flag to true when interaction starts
    }

    public string GetInteractionText()
    {
        if (DialogueManager.Instance.dialogueIsPlaying) return ""; // If dialogue is already playing, return empty string to avoid interaction text showing up again
        return interactionText;
    }

    public void Exit()
    {
        isInteracting = false; // Reset the interaction flag when exiting
    }
}
