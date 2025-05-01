using UnityEngine;

public class NPC_Interaction : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionText;
    [SerializeField] TextAsset inkJSON;

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
        EventManager.Instance.StartFirstPersonDialogue(inkJSON);
    }

    public string GetInteractionText()
    {
        return interactionText;
    }
}
