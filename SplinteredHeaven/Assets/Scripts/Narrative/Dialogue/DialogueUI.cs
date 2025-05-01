using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueUI", menuName = "UI/DialogueUI")]
public class DialogueUI : ScriptableObject
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
}
