using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Narrative/Actions/TeleportPlayerAction")]
public class TeleportPlayerAction : NarrativeAction
{
    public Vector3 destination;

    public override void Execute(NarrativeContext context)
    {
        EventManager.Instance.StartTeleportPlayerEvent(destination);
    }
}