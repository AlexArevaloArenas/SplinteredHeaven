using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Narrative/Actions/PatrolCharacterAction")]
public class PatrolCharacterAction : NarrativeAction
{
    public string characterID;
    //public Vector3[] destinations;

    public override void Execute(NarrativeContext context)
    {
        context.PatrolCharacter(characterID);
    }
}