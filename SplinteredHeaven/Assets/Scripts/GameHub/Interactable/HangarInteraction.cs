using UnityEngine;

public class HangarInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] string interactionText;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraHangar;
    public GameObject unitBuilderUI;

    //[SerializeField] UnitBuilderUI unitBuilderUI;

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
        unitBuilderUI.SetActive(true); // Activate unit builder UI
        EventManager.Instance.StartOpenBuildMecha(); // Close any open build mecha UI
        
        /*
        Fader.Instance.FadeAndDo(() => 
        {
            player.SetActive(false); // Disable player
            cameraHangar.SetActive(true); // Disable player
        }, 0.3f);
        */
    }

    public string GetInteractionText()
    {
        if (DialogueManager.Instance.dialogueIsPlaying) return ""; // If dialogue is already playing, return empty string to avoid interaction text showing up again
        return interactionText;
    }
}
