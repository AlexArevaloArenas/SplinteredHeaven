using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionText;

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
        EventManager.Instance.StartFirstPersonDialogue();
    }

    public string GetInteractionText()
    {
        return interactionText;
    }
}
