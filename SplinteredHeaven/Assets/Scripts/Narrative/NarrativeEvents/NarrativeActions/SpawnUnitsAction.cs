using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Narrative/Actions/SpawnUnits")]
public class SpawnUnitsAction : NarrativeAction
{
    public GameObject[] units;
    public Vector3[] positions;


    public override void Execute(NarrativeContext context)
    {
        int i = 0;
        foreach (var unit in units)
        {
            context.narrativeEventManager.Spawn(unit, positions[i]);
        }
       
    }
}