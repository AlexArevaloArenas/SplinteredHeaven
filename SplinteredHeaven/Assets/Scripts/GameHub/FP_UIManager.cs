using TMPro;
using UnityEngine;

public class FP_UIManager : MonoBehaviour
{
    private bool interactionAvailable;
    [SerializeField] private GameObject interactionInformer;
    private string interactionName;


    //Dialogue
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.FPDialogueEvent += StartDialogue;
        EventManager.Instance.FPChangeInteractionSymbolEvent += ShowSymbolStatus;
    }

    private void Update()
    {
        if (interactionAvailable)
        {
            interactionInformer.GetComponent<TextMeshProUGUI>().text = interactionName;
        }
        interactionInformer.SetActive(interactionAvailable);
    }

    private void ShowSymbolStatus(bool show, string interactionText)
    {
        interactionAvailable = show;
        interactionName = interactionText;
    }

    private void StartDialogue()
    {

    }
}
