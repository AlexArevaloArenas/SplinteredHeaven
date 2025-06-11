using UnityEngine;

public class NPC_Interaction : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionText;

    [SerializeField] TextAsset inkJSON;
    [SerializeField] public Transform transformNPCView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject interactor)
    {
        EventManager.Instance.StartFirstPersonDialogue(inkJSON, transformNPCView.position);
    }

    public string GetInteractionText()
    {
        return interactionText;
    }
}
