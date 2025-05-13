using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveCharacterAction", menuName = "Narrative/Action/MoveCharacter")]
public class NarrativeActionMoveCharacter : NarrativeEventAction
{
    public List<NPC> Characters;
    public Vector3[] Location;
    public override void Execute(NarrativeContext context)
    {
        Debug.Log($"Executing MoveCharacterAction for {Characters.Count} characters.");
        foreach (NPCInstance npc in context.narrativeEventManager.npcInstances)
        {
            if (Characters.Contains(npc.NPCData.npcEnum))
            {
                npc.MoveToLocation(Location[Characters.IndexOf(npc.NPCData.npcEnum)]);
            }
        }
    }
}
