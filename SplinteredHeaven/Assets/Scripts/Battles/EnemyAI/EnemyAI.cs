using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
}

public class Context
{
    public List<UnitManager> allyUnits = new List<UnitManager>();
    public List<UnitManager> enemyUnits = new List<UnitManager>();

    public Transform tacticalWaypoints;
    public Transform coverPoints;

    public Transform enemySpawn;

}
