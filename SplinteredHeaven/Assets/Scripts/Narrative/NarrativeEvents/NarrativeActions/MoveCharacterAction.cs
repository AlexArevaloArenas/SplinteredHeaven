using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Narrative/Actions/Move Character")]
public class MoveCharacterAction : NarrativeAction
{
    public string characterID;
    public Vector3 destination;

    public override void Execute(NarrativeContext context)
    {
        context.MoveCharacter(characterID, destination);
    }
}