using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTrigger", menuName = "Narrative/Trigger/DialogueTrigger")]
public class DialogueTrigger : NarrativeCondition
{
    public string Keyword = "";
    private NarrativeEventInstance instance;

    public override void Initialize(NarrativeEventInstance instance, NarrativeContext context)
    {
        this.instance = instance;
        EventManager.Instance.DialogueTriggerEvent += OnDialogueTriggered;
    }

    public override void Dispose()
    {
        EventManager.Instance.DialogueTriggerEvent -= OnDialogueTriggered;
    }

    private void OnDialogueTriggered(string word)
    {
        if (Keyword != "" && Keyword == word)
        {
            instance.MarkTriggered();
        }
    }
}
