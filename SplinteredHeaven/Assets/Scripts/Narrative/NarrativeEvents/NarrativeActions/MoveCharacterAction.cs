using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Narrative/Actions/Move Character")]
public class MoveCharacterAction : NarrativeAction
{
    public string characterID;
    public string destination;

    public override void Execute(NarrativeContext context)
    {
        context.MoveCharacter(characterID, destination);
    }
}