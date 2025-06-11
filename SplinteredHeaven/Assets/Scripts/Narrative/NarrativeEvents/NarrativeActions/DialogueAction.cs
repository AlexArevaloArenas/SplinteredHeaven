using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAction", menuName = "Narrative/Actions/DialogueAction")]
public class DialogueAction : NarrativeAction
{
    public TextAsset inkFile;
    // You can add more parameters here if needed, such as position or other settings for the dialogue
    public Vector3 dialoguePosition = Vector3.zero; // Default position for the dialogue

    public override void Execute(NarrativeContext context)
    {
        context.dialogueManager.EnterDialogueMode(inkFile, dialoguePosition);
    }
}
