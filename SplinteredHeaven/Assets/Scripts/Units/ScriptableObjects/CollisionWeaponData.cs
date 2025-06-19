using UnityEngine;

[CreateAssetMenu(fileName = "ColliderWeaponData", menuName = "Modules/ColliderWeaponData")]
public class CollisionWeaponData : WeaponData
{
    public LayerMask hitMask;
    public GameObject projectile; // Optional prefab for visual representation
    public float inaccuracyAngle = 0f; // Optional spread

    public override void ApplyEffects(UnitManager user, UnitManager target, UnitPart fallbackPart, Transform attackPoint)
    {
        Transform origin = attackPoint != null ? attackPoint : user.transform;
        Vector3 direction = GetInaccurateDirection(fallbackPart.transform.position-origin.position, inaccuracyAngle);

        GameObject objProjectile = Instantiate(projectile, origin.position, Quaternion.LookRotation(direction));
        objProjectile.GetComponent<MissileComponent>().Init(user, damage);
    }

    private Vector3 GetInaccurateDirection(Vector3 forward, float angleDegrees)
    {
        if (angleDegrees <= 0f) return forward;

        float angleRad = angleDegrees * Mathf.Deg2Rad;
        Vector2 spread = Random.insideUnitCircle * Mathf.Tan(angleRad);
        Vector3 inaccurate = new Vector3(spread.x, spread.y, 1);
        return (Quaternion.LookRotation(forward) * inaccurate).normalized;
    }
}
