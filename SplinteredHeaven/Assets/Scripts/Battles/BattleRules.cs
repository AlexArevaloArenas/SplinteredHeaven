using UnityEngine;

public static class BattleRules
{

    public static bool CheckRange(UnitManager unitMain, UnitManager unitTarget)
    {
        //return Vector3.Distance(unitMain.transform.position, unitTarget.transform.position) <= unitMain.GetAttackRange();
        return false;
    }

    public static void Attack(Unit attacker, Unit target, float distance, Weapon weapon)
    {
        //Check hit
        float checkHit = Random.Range(1, 100);
        if(checkHit > attacker.ArmorClass)
        {
            //Miss
            return;
        }
        int totalSizeSum = 0;
        UnitPart hitPart = null;
        foreach (var part in target.Parts)
        {
            totalSizeSum += part.size;
            //Check if part is in range
            if (checkHit <= totalSizeSum)
            {
                //Check damage
                hitPart = part;
                break;
            }
        }

        //Check damage
        float damage = weapon.GetAttackDamage * (10/distance);

        //Apply damage
        hitPart.TakeDamage(damage);
        hitPart.unit.RefreshPartDamage();
    }

}
