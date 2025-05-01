using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetter : MonoBehaviour
{
    [Header("General UI Items")]
    public GameObject dialoguePanel;
    public GameObject continueIcon;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI displayNameText;
    public Animator portraitAnimator;
    public Animator layoutAnimator;

    [Header("Choices Button List")]
    public GameObject[] choices;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the dialogueUI properties
        DialogueManager.instance.dialogueText = dialogueText;
        DialogueManager.instance.displayNameText = displayNameText;
        DialogueManager.instance.dialoguePanel = dialoguePanel;
        DialogueManager.instance.continueIcon = continueIcon;
        DialogueManager.instance.portraitAnimator = portraitAnimator;
        DialogueManager.instance.layoutAnimator = layoutAnimator;
        DialogueManager.instance.choices = choices;

    }

    public void SelectChoice(int choiceIndex)
    {
        DialogueManager.instance.SelectChoice(choiceIndex);
    }

}
