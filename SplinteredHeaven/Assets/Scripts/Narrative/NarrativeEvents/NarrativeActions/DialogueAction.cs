using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAction", menuName = "Scriptable Objects/DialogueAction")]
public class DialogueAction : NarrativeAction
{
    public TextAsset inkFile;

    public override void Execute(NarrativeContext context)
    {
        context.dialogueManager.EnterDialogueMode(inkFile);
    }
}
