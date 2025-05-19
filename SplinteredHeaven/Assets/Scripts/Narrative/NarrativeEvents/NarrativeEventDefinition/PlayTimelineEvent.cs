using UnityEngine.Timeline;
using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Narrative/Events/Play Timeline")]
public class PlayTimelineEvent : NarrativeEventData
{
    public TimelineAsset timelineAsset;
    private PlayableDirector playableDirector;
    public bool waitForDialogue = true;

    public void Execute(NarrativeContext context)
    {
        playableDirector = context.timelineDirector;
        context.narrativeEventManager.StartCoroutine(PlayTimelineCoroutine(context));
    }

    private IEnumerator PlayTimelineCoroutine(NarrativeContext context)
    {
        var director = context.timelineDirector;

        director.playableAsset = timelineAsset;
        director.Play();

        // If dialogue occurs during Timeline, subscribe to pause points
        if (waitForDialogue)
        {
            context.dialogueManager.OnDialogueStarted += PauseTimeline;
            context.dialogueManager.OnDialogueEnded += ResumeTimeline;
        }

        // Wait until Timeline finishes
        yield return new WaitUntil(() => director.state != PlayState.Playing);

        // Cleanup
        if (waitForDialogue)
        {
            context.dialogueManager.OnDialogueStarted -= PauseTimeline;
            context.dialogueManager.OnDialogueEnded -= ResumeTimeline;
        }
    }

    private void PauseTimeline()
    {
        if (PlayableDirectorExists())
            playableDirector.Pause();
    }

    private void ResumeTimeline()
    {
        if (PlayableDirectorExists())
            playableDirector.Resume();
    }

    private bool PlayableDirectorExists()
    {
        return playableDirector != null;
    }
}